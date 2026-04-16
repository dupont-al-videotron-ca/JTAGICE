
#pragma warning disable CS8766
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using log4net;
using log4net.Repository.Hierarchy;
namespace JTAGICEmkII.HostService
{
    public class HostDeviceService : IHostDeviceService, IDisposable
    {
        public readonly static string HostDeviceName = "JTAGICE mkII";

        #region Constructors 

        internal HostDeviceService(RxFrame rxFrame, TxFrame txFrame)
        {
            ArgumentNullException.ThrowIfNull(rxFrame);
            ArgumentNullException.ThrowIfNull(txFrame);

            this._rxFrame = rxFrame;
            this._txFrame = txFrame;
            _activityStructure = new StructureActivity(this);
            _parameters = new Parameters();
            Logger = LogManager.GetLogger(this.GetType());
            SignOnResponse = null;
            _cancellationSource = new CancellationTokenSource();

            _hostSession = new HostSession(this._activityStructure);
            _backgroundService = new MyBackgroundService<IActivityElement, bool>(this._activityStructure.RunActivity, _hostSession);
        }

        #endregion


        #region Fields 

        private CancellationTokenSource _cancellationSource;
        private readonly RxFrame _rxFrame;
        private readonly TxFrame _txFrame;
        private StructureActivity _activityStructure;

        public ILog Logger { get; private set; }
        public ResponseSignOn? SignOnResponse { get; set; }

        private bool _disposedValue;
        private CommandRequestBase<IMasterCommand, ISlaveResponse>? _request;

        private Parameters _parameters;
        private HostSession _hostSession;
        private MyBackgroundService<IActivityElement, bool> _backgroundService;

        #endregion


        #region Properties 

        public TargetState TargetMcuState { get; set; } = new TargetState();

        #endregion


        #region Delegates / Events 

        public event EventHandler<RequestEventArgs>? RequestCompleted;
        public event EventHandler<RequestEventArgs>? RequestTimeout;

        public event EventHandler<ResponseReceivedEventArgs>? ResponseReceived;
        public event EventHandler<EventReceivedEventArgs>? EventReceived;
        public event EventHandler<CommandReceivedEventArgs>? CommandReceived;

        #endregion


        #region Public Methods 


        public bool Initialise()
        {
            // Build activity diagram

            _rxFrame.Timeout = 1000;
            _rxFrame.RxTimerExpired += this._rxFrame_RxTimerExpired;
            _rxFrame!.ResponseReceived += RxFrame_Received;
            _rxFrame.StartReceiving();
            this.EventReceived += HostService_EventReceived;

            this._activityStructure.CurrentActivity = _hostSession;

            return _rxFrame.IsReceiving;
        }

        public ICommandResult RestoreTarget()
        {
            using (var request = CommandRequestFactory.CreateRequest(this._activityStructure, Master.MasterCommandEnum.CMND_RESTORE_TARGET))
            {
                return ProcessCommand(request, out ISlaveResponse? rxResponse);
            }
        }

        public ICommandResult CloseDebugSession()
        {
            Logger.Debug($"Closing session by user.");
            ICommandResult retval = this.RestoreTarget();
            if (retval.IsSuccess)
                this._hostSession.WaitEndSession();

            this._cancellationSource.Cancel();
            var task = _backgroundService.StopAsync(_cancellationSource.Token);
            task.Wait(TimeSpan.FromSeconds(60));

            Logger.Debug($"Session is closed.");

            return retval;
        }

        public ICommandResult OpenDebugSession()
        {
            Logger.Debug($"Opening session by user.");

            if (this._activityStructure.CurrentActivity != null)
            {
                _backgroundService.StartAsync(_cancellationSource.Token);
            }
            else
            {
                Logger.Error("Activity structure is not properly initialized. No current activity or current activity is not TargetConnecting.");
                throw new InvalidOperationException("Activity structure is not properly initialized. No current activity.");
            }

            return CommandResult.Successs;
        }

        public ICommandResult ClearEvents()
        {
            using (var request = CommandRequestFactory.CreateRequest(this._activityStructure, Master.MasterCommandEnum.CMND_CLEAR_EVENTS))
            {
                return ProcessCommand(request, out ISlaveResponse? rxResponse);
            }
        }

        public ICommandResult SetDeviceDescriptor()
        {
            using (var request = CommandRequestFactory.CreateRequest(this._activityStructure, Master.MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR))
            {

                // TODO: fill structure with real data
                DeviceDescriptorFields deviceDescriptorFields = new DeviceDescriptorFields();

                byte[] deviceDescriptorBytes = new byte[Marshal.SizeOf<DeviceDescriptorFields>()];
                Span<byte> deviceDescriptorSpan = new Span<byte>(deviceDescriptorBytes);

                MemoryMarshal.Write(deviceDescriptorSpan, deviceDescriptorFields);
                ((Master.CommandMultipleByte)request.Command).Data.AddRange(deviceDescriptorBytes);

                return ProcessCommand(request, out ISlaveResponse? rxResponse);
            }
        }


        public ICommandResult ClearBreakpoint(int Index, ulong Breakpoint) => throw new NotImplementedException();
        public ICommandResult EnterPrograming() => throw new NotImplementedException();
        public ICommandResult EraseDevice() => throw new NotImplementedException();
        public ICommandResult EraseMemory(int MemType, ulong Address, ulong Length) => throw new NotImplementedException();
        public ICommandResult GetBreakpoint(int Index, ulong Breakpoint, int BreakpointType, int BrakpointMode) => throw new NotImplementedException();

        public ICommandResult GetSync() => throw new NotImplementedException();
        public ICommandResult WriteMemory(int MemType, ulong Address, byte[] Values) => throw new NotImplementedException();
        public ICommandResult ReadMemory(int memType, ulong Address, ulong Length, out byte[] Values) => throw new NotImplementedException();

        public ICommandResult GetParameter(int paramId, out uint value)
        {
            return GetParameter((Master.ParameterEnum)paramId, out value);
        }

        public ICommandResult GetParameter(Master.ParameterEnum paramId, out uint value)
        {
            Parameter? parameterToRead = _parameters.Values.FirstOrDefault(p => p.ParameterId == paramId && p.IsRead && !p.IsUsed);
            if (parameterToRead != null && GetParameterLocal(parameterToRead, true))
            {
                value = parameterToRead.Value;
                return CommandResult.Successs;
            }
            else
            {
                value = 0;
                return CommandResult.Failed;
            }
        }

        public ICommandResult GetAllParameter()
        {
            var parametersToRead = _parameters.GetAllRead();
            foreach (var param in parametersToRead)
            {
                if (!GetParameterLocal(param.Value, param.Value == parametersToRead.Last().Value))
                {
                    return (CommandResult)false;
                }
            }

            return (CommandResult)true;
        }


        public ICommandResult SetParameter(int paramId, uint value)
        {
            return SetParameter((Master.ParameterEnum)paramId, value);
        }

        public ICommandResult SetParameter(Master.ParameterEnum paramId, uint value)
        {
            Parameter? parameterToWrite = _parameters.Values.FirstOrDefault(p => p.IsWrite && !p.IsUsed);

            if (parameterToWrite != null)
            {
                parameterToWrite.Value = value;
                return SetParameterLocal(parameterToWrite, true);
            }

            return (CommandResult)false;
        }

        public ICommandResult SetAllParameter()
        {
            var writeParameters = _parameters.GetAllWrite();
            foreach (KeyValuePair<Master.ParameterEnum, Parameter> param in writeParameters)
            {
                if (!SetParameterLocal(param.Value, param.Value == writeParameters.Last().Value))
                    return (CommandResult)false;
            }

            return (CommandResult)true;

        }


        //public ICommandResult GetTargetInfo(out DeviceInfo Info) => throw new NotImplementedException();
        public ICommandResult LeavePrograming() => throw new NotImplementedException();
        public ICommandResult ReadMemory(int memType, ulong Address, ulong Length, out byte Values) => throw new NotImplementedException();
        public ICommandResult ReadProgramCount(out ulong ProgramCounter) => throw new NotImplementedException();
        public ICommandResult Reconnect() => throw new NotImplementedException();
        public ICommandResult Reset()
        {
            using (var request = CommandRequestFactory.CreateRequest(this._activityStructure, MasterCommandEnum.CMND_RESET))
            {
                return ProcessCommand(request, out ISlaveResponse? rxResponse);
            }
        }

        public ICommandResult SetBreakpoint(int index, ulong Breakpoint, int BreakpointType, int BrakpointMode) => throw new NotImplementedException();
        public ICommandResult SignOff() => throw new NotImplementedException();

        public async Task<ICommandResult> SignOnAsync()
        {
            Task<CommandResult> task = Task.Run(() =>
            {
                if (SignOn(out ResponseSignOn? response).IsSuccess)
                {
                    SignOnResponse = response;
                    return (CommandResult)true;
                }

                return (CommandResult)false;
            });

            await task;

            return task.Result;
        }

        public ICommandResult SignOn(out ResponseSignOn? response)
        {
            CommandResult retval = CommandResult.Failed;

            using (var request = CommandRequestFactory.CreateRequest(this._activityStructure, MasterCommandEnum.CMND_GET_SIGN_ON))
            {
                if (ProcessCommand(request, out ISlaveResponse? response1))
                {
                    response = response1 as ResponseSignOn;
                    retval = (CommandResult)response!;
                }
                else
                {
                    response = null;
                }
            }

            return retval;
        }


        public ICommandResult StartRunning() => throw new NotImplementedException();
        public ICommandResult StartRunningUntil(ulong Breakpoint) => throw new NotImplementedException();
        public ICommandResult StepIn(ulong ProgramCounter) => throw new NotImplementedException();
        public ICommandResult StopRunning() => throw new NotImplementedException();
        public ICommandResult VerifiyPrograming() => throw new NotImplementedException();
        public ICommandResult WriteMemory(int MemType, ulong Address, byte Values)
        {
            using (var request = CommandRequestFactory.CreateRequest(this._activityStructure, MasterCommandEnum.CMND_WRITE_MEMORY))
            {
                if (ProcessCommand(request, out ISlaveResponse? response))
                {
                    return new CommandResult(response!);
                }
                else
                {
                    return CommandResult.Failed;
                }
            }


        }

        public ICommandResult WriteProgramCount(ulong ProgramCounter) => throw new NotImplementedException();
        public ICommandResult WritePrograming(ulong Address, byte Values) => throw new NotImplementedException();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    this.EventReceived -= HostService_EventReceived;
                    _rxFrame.ResponseReceived -= RxFrame_Received;
                    _rxFrame.Dispose();
                    _txFrame.Dispose();
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~HostDeviceService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public bool ModifyAllParameter()
        {
            // todo: modify parameters as necessary before setting them back to device.
            return true;
        }

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        public CommandResult SetParameterLocal(Parameter parameter, bool lastRequest)
        {
            using (var request = CommandRequestFactory.CreateRequest(this._activityStructure, Master.MasterCommandEnum.CMND_SET_PARAMETER))
            {
                ((Master.CommandParameter)request.Command).ParameterId = parameter.ParameterId;
                if (parameter.Size == 1)
                {
                    ((Master.CommandParameter)request.Command).Data.Add((byte)parameter.Value);
                }
                else if (parameter.Size == 2)
                {
                    ((Master.CommandParameter)request.Command).Data.Add((byte)(parameter.Value & 0xff));
                    ((Master.CommandParameter)request.Command).Data.Add((byte)((parameter.Value >> 8) & 0xff));
                }
                else if (parameter.Size == 4)
                {
                    ((Master.CommandParameter)request.Command).Data.Add((byte)(parameter.Value & 0xff));
                    ((Master.CommandParameter)request.Command).Data.Add((byte)((parameter.Value >> 8) & 0xff));
                    ((Master.CommandParameter)request.Command).Data.Add((byte)((parameter.Value >> 16) & 0xff));
                    ((Master.CommandParameter)request.Command).Data.Add((byte)((parameter.Value >> 24) & 0xff));
                }
                else
                {
                    Logger.Warn($"Unsupported parameter size {parameter.Size} for parameter {parameter.ParameterId}.");
                    throw new InvalidOperationException($"Unsupported parameter size {parameter.Size}.");
                }

                return ProcessCommand(request, out ISlaveResponse? response, lastRequest);
            }
        }

        private bool GetParameterLocal(Parameter parameter, bool lastRequest)
        {
            using (var request = CommandRequestFactory.CreateRequest(this._activityStructure, Master.MasterCommandEnum.CMND_GET_PARAMETER))
            {
                ((Master.CommandParameter)request.Command).ParameterId = parameter.ParameterId;
                if (ProcessCommand(request, out ISlaveResponse? response1, lastRequest))
                {
                    var response = response1 as ResponseMultipleByte;

                    if (response != null)
                    {
                        if (parameter.Size == 1)
                        {
                            parameter.Value = response.GetDataBytes()[0];
                        }
                        else if (parameter.Size == 2)
                        {
                            parameter.Value = BitConverter.ToUInt16(response.GetDataBytes(), 0); ;
                        }
                        else if (parameter.Size == 4)
                        {
                            parameter.Value = BitConverter.ToUInt32(response.GetDataBytes(), 0); ;
                        }
                        else
                        {
                            Logger.Warn($"Unsupported parameter size {parameter.Size} for parameter {parameter.ParameterId}.");
                            throw new InvalidOperationException($"Unsupported parameter size {parameter.Size}.");
                        }

                        return true;
                    }
                    else
                    {
                        Logger.Warn($"Unexpected response type received for parameter {parameter.ParameterId}.");
                        throw new InvalidOperationException($"Unexpected response type.");
                    }
                }
                else
                {
                    return false;
                }
            }
        }


        private CommandResult ProcessCommand(CommandRequestBase<IMasterCommand, ISlaveResponse> commandRequest, out ISlaveResponse? response, bool lastRequest = true)
        {
            response = null;
            CommandResult retval = CommandResult.Failed;

            _request = commandRequest;
            Command command = (Command)_request.Command;
            IActivityComElement commandElement = _request.CommandElement;

            Logger.Debug($"Processing command: {command.MessageId}");
            while (_request.RetryCount-- > 0 && !retval.IsSuccess)
            {
                Logger.Debug($"Retry count: {_request.RetryCount}");
                if (commandElement.CanSendCommand(command))
                {
                    Logger.Debug($"Building and Sending command: {command.MessageId}");
                    if (_txFrame.BuildAndSendFrameCommand(command) != 0)
                    {
                        commandElement.CommandSent();
                        Logger.Debug($"Command sent waiting for response.");

                        if (_request.WaitForResponse() && _request.Response != null)
                        {
                            Logger.Debug($"Response received for command: {_request.Response.ResponseId}");
                            if (commandElement.OnReceivedResponse(_request.Response))
                            {
                                response = _request.Response;
                                retval = (CommandResult)response;
                                OnRequestCompleted(this, new RequestEventArgs(_request));
                            }
                        }
                        else
                        {
                            Logger.Debug($"Command {_request.Command.MessageId} timeout , {_request.RetryCount} retries left.");
                        }
                    }
                    else
                    {
                        Logger.Debug($"Command {command.MessageId} failed to send, {_request.RetryCount} retries left.");
                    }
                }
                else
                {
                    Logger.Debug($"Command {command.MessageId} cannot be sent in current activity state.");
                    break;
                }
            }

            if (_request.IsRequestTimeout)
            {
                this.OnRequestTimeout(this, new RequestEventArgs(_request));
            }

            _request = null;
            return retval;
        }

        private void OnRequestCompleted(object? sender, RequestEventArgs e)
        {
            this.RequestCompleted?.Invoke(this, e);
            this._activityStructure.OnRequestCompleted(e.Request);
        }

        private void RxFrame_Received(object? sender, ResponseReceivedEventArgs e)
        {
            if (e.Response.IsEvent)
            {
                OnReceivedEvent(this, new EventReceivedEventArgs(e.Response));
                _activityStructure.OnReceivedEvent(e.Response);
            }
            else if (_request != null)
            {
                OnReceivedResponse(sender, e);
                _request.ReceivedResponse(e.Response);
            }

        }

        private void OnReceivedEvent(object? sender, EventReceivedEventArgs e)
        {
            EventReceived?.Invoke(sender, e);
        }

        private void OnReceivedResponse(object? sender, ResponseReceivedEventArgs e)
        {
            this.ResponseReceived?.Invoke(this, e);
        }

        private void OnReceivedCommand(object? sender, CommandReceivedEventArgs e)
        {
            this.CommandReceived?.Invoke(this, e);
        }

        private void _rxFrame_RxTimerExpired(object? sender, EventArgs e)
        {
            Logger.Warn("Rx timer expired.");
        }

        private void OnRequestTimeout(object? sender, RequestEventArgs e)
        {
            _activityStructure.OnRequestTimeout(e.Request);
            this.RequestTimeout?.Invoke(this, e);
        }

        private void HostService_EventReceived(object? sender, EventReceivedEventArgs e)
        {
            var response = e.Event;
            switch (response?.ResponseId)
            {
                case SlaveResponseEnum.EVT_PROGRAM_BREAK:
                case SlaveResponseEnum.EVT_BREAK:
                case SlaveResponseEnum.EVT_PDSB_BREAK:
                case SlaveResponseEnum.EVT_PDSMB_BREAK:
                    //_nextIndex = this.Nexts.FindIndex(n => n is TargetStopped);
                    break;
                case SlaveResponseEnum.EVT_RUN:
                    //_nextIndex = this.Nexts.FindIndex(n => n is TargetRunning);
                    break;
                case SlaveResponseEnum.EVT_TARGET_POWER_ON:
                    //_nextIndex = this.Nexts.FindIndex(n => n is );
                    break;

                case SlaveResponseEnum.EVT_DEBUG:
                    //_nextIndex = this.Nexts.FindIndex(n => n is TargetRunning);
                    break;
                case SlaveResponseEnum.EVT_EXTERNAL_RESET:
                    //_nextIndex = this.Nexts.FindIndex(n => n is TargetRunning);
                    break;
                case SlaveResponseEnum.EVT_TARGET_SLEEP:
                    //_nextIndex = this.Nexts.FindIndex(n => n is TargetRunning);
                    break;
                case SlaveResponseEnum.EVT_TARGET_WAKEUP:
                    //_nextIndex = this.Nexts.FindIndex(n => n is TargetRunning);
                    break;
                case SlaveResponseEnum.EVT_ICE_POWER_ERROR_STATE:
                    //_nextIndex = this.Nexts.FindIndex(n => n is TargetRunning);
                    break;
                case SlaveResponseEnum.EVT_ICE_POWER_OK:
                    //_nextIndex = this.Nexts.FindIndex(n => n is TargetRunning);
                    break;
                case SlaveResponseEnum.EVT_IDR_DIRTY:
                    //_nextIndex = this.Nexts.FindIndex(n => n is TargetRunning);
                    break;
                case SlaveResponseEnum.EVT_NONE:
                case SlaveResponseEnum.EVT_ERROR_PHY_FROECE_BREAK_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_RELEASE_BREAK_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_MAX_BIT_LENGHT_DIFF:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT_BAUD:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_OUT_OF_RANGE:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_WAIT_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_BREAK:
                case SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVE_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVED_BREAK:
                case SlaveResponseEnum.EVT_ERROR_PHY_NO_ACTIVITY:

                    break;
                default:
                    this.Logger.Debug($"Event handling not implemented for event: {response?.ResponseId}.");
                    throw new NotImplementedException($"Event handling not implemented for event: {response?.ResponseId}.");
            }
        }

        #endregion

        #region Private Classes / Enum 

        private class MyBackgroundService<P, R> : Microsoft.Extensions.Hosting.BackgroundService
            where P : class
            where R : struct
        {
            public Func<P, R> _excecution;
            public P? _param;
            public MyBackgroundService(Func<P, R> action, P? param)
            {
                _excecution = action;
                _param = param;
            }

            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                var result = _excecution(_param!);
                return Task.FromResult(result);
            }
        }

        #endregion
    }
}
