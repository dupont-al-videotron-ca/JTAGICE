
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
using JTAGICEmkII.Slave;
using log4net;
using log4net.Repository.Hierarchy;
namespace JTAGICEmkII.HostService
{
    public class HostDeviceService : IHostDeviceService, IDisposable
    {


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

        }

        #endregion


        #region Fields 

        private readonly RxFrame _rxFrame;
        private readonly TxFrame _txFrame;
        private StructureActivity _activityStructure;

        public ILog Logger { get; private set; }
        public ResponseSignOn? SignOnResponse { get; set; }

        private bool _disposedValue;
        private CommandRequest<Master.Command, ISlaveResponse>? _request;

        private Parameters _parameters;

        #endregion


        #region Properties 

        public TargetState TargetMcuState { get; set; } = new TargetState();

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 


        public bool Initialise()
        {
            // Build activity diagram
            StructureActivityBuilder.Build(this._activityStructure);

            _rxFrame.Timeout = 1000;
            _rxFrame.RxTimerExpired += this._rxFrame_RxTimerExpired;
            _rxFrame!.ResponceReceived += RxFrame_Received;
            _rxFrame.StartReceiving();

            if (this._activityStructure.CurrentActivity == null)
                throw new InvalidOperationException("Activity structure is not properly initialized. No current activity.");

            return _rxFrame.IsReceiving;
        }


        public bool ConnectToTarget()
        {
            if (this._activityStructure.CurrentActivity != null
                && this._activityStructure.CurrentActivity is TargetConnecting)
            {
                var activity = this._activityStructure.CurrentActivity;
                try
                {
                    RunActivityDiagram(activity);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error($"Failed to connect to target: {ex.Message}");
                    return false;
                }
            }
            else
            {
                Logger.Error("Activity structure is not properly initialized. No current activity or current activity is not TargetConnecting.");
                throw new InvalidOperationException("Activity structure is not properly initialized. No current activity.");
            }
        }
        public bool ClearEvents()
        {
            Master.Command command = (Master.CommandFactory.CreateCommand(Master.MasterCommandEnum.CMND_CLEAR_EVENTS) as Master.Command)!;
            return ProcessCommand(command, out ISlaveResponse? rxResponse);
        }



        public bool SetDeviceDescriptor()
        {
            Master.CommandMultipleByte command = (Master.CommandFactory.CreateCommand(Master.MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR) as Master.CommandMultipleByte)!;

            // TODO: fill structure with real data
            DeviceDescriptorFields deviceDescriptorFields = new DeviceDescriptorFields();

            byte[] deviceDescriptorBytes = new byte[Marshal.SizeOf<DeviceDescriptorFields>()];
            Span<byte> deviceDescriptorSpan = new Span<byte>(deviceDescriptorBytes);

            MemoryMarshal.Write(deviceDescriptorSpan, deviceDescriptorFields);
            command.Data.AddRange(deviceDescriptorBytes);

            if (ProcessCommand(command, out ISlaveResponse? rxResponse))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public bool ClearBreakpoint(int Index, ulong Breakpoint) => throw new NotImplementedException();
        public bool EnterPrograming() => throw new NotImplementedException();
        public bool EraseDevice() => throw new NotImplementedException();
        public bool EraseMemory(int MemType, ulong Address, ulong Length) => throw new NotImplementedException();
        public bool GetBreakpoint(int Index, ulong Breakpoint, int BreakpointType, int BrakpointMode) => throw new NotImplementedException();

        public bool GetParameter(int paramId, out uint value)
        {
            return GetParameter((Master.ParameterEnum)paramId, out value);
        }

        public bool GetParameter(Master.ParameterEnum paramId, out uint value)
        {
            Parameter? parameterToRead = _parameters.Values.FirstOrDefault(p => p.ParameterId == paramId && p.IsRead && !p.IsUsed);
            if (parameterToRead != null && GetParameterLocal(parameterToRead, true))
            {
                value = parameterToRead.Value;
                return true;
            }
            else
            {
                value = 0;
                return false;
            }
        }

        public bool GetAllParameter()
        {
            var parametersToRead = _parameters.GetAllRead();
            foreach (var param in parametersToRead)
            {
                if (!GetParameterLocal(param.Value, param.Value == parametersToRead.Last().Value))
                {
                    return false;
                }
            }

            return true;
        }


        public bool SetParameter(int paramId, uint value)
        {
            return SetParameter((Master.ParameterEnum)paramId, value);
        }

        public bool SetParameter(Master.ParameterEnum paramId, uint value)
        {
            Parameter? parameterToWrite = _parameters.Values.FirstOrDefault(p => p.IsWrite && !p.IsUsed);

            if (parameterToWrite != null)
            {
                parameterToWrite.Value = value;
                return SetParameterLocal(parameterToWrite, true);
            }

            return false;
        }

        public bool SetAllParameter()
        {
            var writeParameters = _parameters.GetAllWrite();
            foreach (KeyValuePair<Master.ParameterEnum, Parameter> param in writeParameters)
            {
                if (!SetParameterLocal(param.Value, param.Value == writeParameters.Last().Value))
                    return false;
            }

            return true;

        }


        //public bool GetTargetInfo(out DeviceInfo Info) => throw new NotImplementedException();
        public bool LeavePrograming() => throw new NotImplementedException();
        public bool ReadMemory(int memType, ulong Address, ulong Length, out byte Values) => throw new NotImplementedException();
        public bool ReadProgramCount(out ulong ProgramCounter) => throw new NotImplementedException();
        public bool Reconnect() => throw new NotImplementedException();
        public bool Reset()
        {
            Master.Command command = (Master.CommandFactory.CreateCommand(Master.MasterCommandEnum.CMND_RESET) as Master.Command)!;
            return ProcessCommand(command, out ISlaveResponse? rxResponse);
        }

        public bool SetBreakpoint(int index, ulong Breakpoint, int BreakpointType, int BrakpointMode) => throw new NotImplementedException();
        public bool SignOff() => throw new NotImplementedException();

        public async Task<bool> SignOnAsync()
        {
            Task<bool> task = Task.Run(() =>
            {
                if (SignOn(out ResponseSignOn? response))
                {
                    SignOnResponse = response;
                    return true;
                }

                return false;
            });

            await task;

            return task.Result;
        }

        public bool SignOn(out ResponseSignOn? response)
        {
            Master.Command command = (Master.CommandFactory.CreateCommand(Master.MasterCommandEnum.CMND_GET_SIGN_ON) as Master.Command)!;
            if (ProcessCommand(command, out ISlaveResponse? response1))
            {
                response = response1 as ResponseSignOn;
                return true;
            }
            else
            {
                response = null;
                return false;
            }

        }


        public bool StartRunning() => throw new NotImplementedException();
        public bool StartRunningUntil(ulong Breakpoint) => throw new NotImplementedException();
        public bool StepIn(ulong ProgramCounter) => throw new NotImplementedException();
        public bool StopRunning() => throw new NotImplementedException();
        public bool VerifiyPrograming() => throw new NotImplementedException();
        public bool WriteMemory(int MemType, ulong Address, byte Values) => throw new NotImplementedException();
        public bool WriteProgramCount(ulong ProgramCounter) => throw new NotImplementedException();
        public bool WritePrograming(ulong Address, byte Values) => throw new NotImplementedException();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _rxFrame.ResponceReceived -= RxFrame_Received;
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

        public bool SetParameterLocal(Parameter parameter, bool lastRequest)
        {
            Master.CommandParameter command = (Master.CommandFactory.CreateCommand(Master.MasterCommandEnum.CMND_SET_PARAMETER) as Master.CommandParameter)!;
            command.ParameterId = parameter.ParameterId;

            if (parameter.Size == 1)
            {
                command.Data.Add((byte)parameter.Value);
            }
            else if (parameter.Size == 2)
            {
                command.Data.Add((byte)(parameter.Value & 0xff));
                command.Data.Add((byte)((parameter.Value >> 8) & 0xff));
            }
            else if (parameter.Size == 4)
            {
                command.Data.Add((byte)(parameter.Value & 0xff));
                command.Data.Add((byte)((parameter.Value >> 8) & 0xff));
                command.Data.Add((byte)((parameter.Value >> 16) & 0xff));
                command.Data.Add((byte)((parameter.Value >> 24) & 0xff));
            }
            else
            {
                Logger.Warn($"Unsupported parameter size {parameter.Size} for parameter {parameter.ParameterId}.");
                throw new InvalidOperationException($"Unsupported parameter size {parameter.Size}.");
            }

            return ProcessCommand(command, out ISlaveResponse? response, lastRequest);
        }

        private bool GetParameterLocal(Parameter parameter, bool lastRequest)
        {
            Master.CommandParameter command = (Master.CommandFactory.CreateCommand(Master.MasterCommandEnum.CMND_GET_PARAMETER) as Master.CommandParameter)!;
            command.ParameterId = parameter.ParameterId;

            if (ProcessCommand(command, out ISlaveResponse? response1, lastRequest))
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


        private bool ProcessCommand(Master.Command command, out ISlaveResponse? response, bool lastRequest = true)
        {
            response = null;
            bool retval = false;
            using (_request = new CommandRequest<Master.Command, ISlaveResponse>(command))
            {
                Logger.Debug($"Processing command: {command.MessageId}");
                while (_request.RetryCount-- > 0 && !retval)
                {
                    Logger.Debug($"Retry count: {_request.RetryCount}");
                    if (_activityStructure.CanSendCommand(command))
                    {
                        Logger.Debug($"Building and Sending command: {command.MessageId}");
                        if (_txFrame.BuildAndSendFrameCommand(command) != 0)
                        {
                            _activityStructure.CommandSent(command);
                            Logger.Debug($"Command sent waiting for response.");

                            if (_request.WaitForResponse() && _request.Response != null)
                            {
                                Logger.Debug($"Response received for command: {_request.Response.ResponseId}");
                                if (_activityStructure.OnReceivedResponse(_request.Response))
                                {
                                    response = _request.Response;
                                    retval = _activityStructure.ExitActivity(lastRequest);
                                }
                            }
                            else
                            {
                                Logger.Debug($"Command {command.MessageId} timeout , {_request.RetryCount} retries left.");
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
            }
            _request = null;
            return retval;
        }

        private void RxFrame_Received(object? sender, ResponseReceivedEventArgs e)
        {
            if (e.Response.IsEvent)
            {
                _activityStructure.OnReceivedEvent(e.Response);
            }
            else if (_request != null)
            {
                _request.ReceivedResponse(e.Response);
            }
        }

        private void _rxFrame_RxTimerExpired(object? sender, EventArgs e)
        {
            Logger.Warn("Rx timer expired.");
        }

        private void RunActivityDiagram(IActivityElement flowElement)
        {
            Task t = Task.Run(() =>
            {
                if (flowElement != null)
                {
                    if (!flowElement.Accept(new VisitorActivityEntry()))
                        throw new InvalidOperationException($"Failed to execute activity entry for activity {flowElement.GetType()}.");
                    if (!flowElement.Accept(new VisitorActivityAction()))
                        throw new InvalidOperationException($"Failed to execute activity action for activity {flowElement.GetType()}.");
                    if (!flowElement.Accept(new VisitorActivityExit(true)))
                        throw new InvalidOperationException($"Failed to execute activity exit for activity {flowElement.GetType()}.");

                    return;
                }
                else
                {
                    throw new InvalidOperationException("Activity structure is not properly initialized. No current activity or current activity is not TargetConnecting.");
                }
            });

            t.Wait();
            return;
        }

        #endregion

        #region Private Classes / Enum 

        #endregion
    }
}
