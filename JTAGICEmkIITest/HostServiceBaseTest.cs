using System.Collections.Generic;
using System.ComponentModel;
using JTAGICEmkII;
using JTAGICEmkII.HostService;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using JTAGICEmkIITest.Moq;
using MyFramework;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace JTAGICEmkIITest
{

    public class HostServiceBaseTest : FrameTest
    {
        protected RxFrame? _rxFrame;
        protected TxFrame? _txFrame;
        protected bool _timeoutOccured;
        protected ISlaveResponse? _responseReceived;
        protected IMasterCommand? _commandReceived;
        protected int _nbResponsesReceived = 0;
        protected int _nbEventReceived = 0;
        protected int _nbCommandReceived = 0;
        protected int _nbEventSent = 0;
        protected StructureActivity _activityStructure;
        protected readonly CancellationTokenSource _cancellationSource;

        protected Parameters _parameters;
        protected HostDeviceService? _hostService;
        public HostServiceBaseTest() : base()
        {
            _activityStructure = null!;
            _parameters = BuildParametersForTest();
            _cancellationSource = new CancellationTokenSource();
        }

        override protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                Logger.Info($"Disposing {this.GetType()}.");
                if (_rxFrame != null)
                {
                    _rxFrame.ResponseReceived -= RxFrame_ResponseReceived;
                    _rxFrame.CommandReceived -= RxFrame_CommandReceived;
                    _rxFrame.RxTimerExpired -= RxFrame_RxTimerExpired;
                }

                _hostService?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void RxFrame_CommandReceived(object? sender, CommandReceivedEventArgs e)
        {
            Logger.Info($"Command received: {e.Command.MessageId}");
            if (_timeoutOccured)
            {
                _commandReceived = null;
                return;
            }

            _nbCommandReceived++;
            _commandReceived = e.Command;

            ISlaveResponse? response = CreateResponse(_commandReceived);
            if (response == null)
            {
                // No eventResponse for this command, do not send anything
                Logger.Debug($"No response for command {e.Command.MessageId}, not sending anything");
                return;
            }

            _txFrame?.BuildAndSendFrameResponse((Response)response);
        }

        protected void CreateAndSendEvent(SlaveResponseEnum eventId)
        {
            ISlaveResponse? eventResponse = CreateEventResponse(eventId);

            if (eventResponse == null)
            {
                // No eventResponse for this command, do not send anything
                throw new InvalidOperationException();
            }

            SendEvent(eventResponse);
        }

        protected void SendEvent(ISlaveResponse eventResponse)
        {
            ArgumentNullException.ThrowIfNull(eventResponse);
            _nbEventSent++;
            _txFrame?.BuildAndSendFrameResponse((Response)eventResponse, true);
        }

        protected ISlaveResponse? CreateEventResponse(SlaveResponseEnum eventId)
        {
            if (eventId <= SlaveResponseEnum.EventRangeMin || eventId > SlaveResponseEnum.EventRangeMax)
            {
                throw new ArgumentException($"Invalid event id {eventId}");
            }

            ISlaveResponse eventResponse = ResponseFactory.CreateResponse(eventId)!;

            switch (eventId)
            {
                case SlaveResponseEnum.EVT_BREAK:
                    var eventBreak = (ResponseEventBreak)eventResponse;
                    eventBreak.BreakCause = EventBreakCauseEnum.PROGRAMME_BREAK;
                    eventBreak.ProgramCounter = 0x12345678;
                    break;
                case SlaveResponseEnum.EVT_RUN:
                    var eventRun = (ResponseMultipleByte)eventResponse;
                    eventRun.Data.Add(0x01);
                    break;
                case SlaveResponseEnum.EVT_DEBUG:
                    var eventDebug = (ResponseEventDebug)eventResponse;
                    eventDebug.CommError = CommErrorEnum.Timeout;
                    eventDebug.CommState = CommStateEnum.GetCRC;
                    break;
                case SlaveResponseEnum.EVT_TARGET_POWER_ON:
                case SlaveResponseEnum.EVT_TARGET_POWER_OFF:
                case SlaveResponseEnum.EVT_EXTERNAL_RESET:
                case SlaveResponseEnum.EVT_TARGET_SLEEP:
                case SlaveResponseEnum.EVT_TARGET_WAKEUP:
                case SlaveResponseEnum.EVT_ICE_POWER_ERROR_STATE:
                case SlaveResponseEnum.EVT_ICE_POWER_OK:
                case SlaveResponseEnum.EVT_IDR_DIRTY:
                case SlaveResponseEnum.EVT_NONE:
                case SlaveResponseEnum.EVT_PROGRAM_BREAK:
                case SlaveResponseEnum.EVT_PDSMB_BREAK:
                case SlaveResponseEnum.EVT_PDSB_BREAK:
                case SlaveResponseEnum.EVT_ERROR_PHY_FORCE_BREAK_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_MAX_BIT_LENGHT_DIFF:
                case SlaveResponseEnum.EVT_ERROR_PHY_NO_ACTIVITY:
                case SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVED_BREAK:
                case SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVE_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_BREAK:
                case SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_RELEASE_BREAK_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_OUT_OF_RANGE:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT_BAUD:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_WAIT_TIMEOUT:
                    break;
                default:
                    throw new ArgumentException($"Invalid event id {eventId}");
            }

            return eventResponse;
        }

        protected ISlaveResponse? CreateResponse(IMasterCommand commandReceived)
        {
            switch (commandReceived.MessageId)
            {
                // Single byte commands
                case MasterCommandEnum.CMND_SIGN_OFF:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_GET_SIGN_ON:
                    var signOn = (ResponseSignOn)ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_SIGN_ON)!;

                    signOn.CommunicationProtocolVersion = 1;
                    signOn.MasterMcuBootLoaderVersion = 1;
                    signOn.MasterMcuHwVersion = 1;
                    signOn.MasterMcuFirmwareVersionMajor = 1;
                    signOn.MasterMcuFirmwareVersionMinor = 0;
                    signOn.SlaveMcuBootLoaderVersion = 1;
                    signOn.SlaveMcuFirmwareVersionMajor = 1;
                    signOn.SlaveMcuFirmwareVersionMinor = 0;
                    signOn.SlaveMcuHwVersion = 1;
                    signOn.SerialNumber = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
                    signOn.DeviceId = new byte[] { 0x10, 0x20, 0x30, 0x40 };

                    return signOn;
                case MasterCommandEnum.CMND_READ_PC:
                    var readPc = (ResponseProgramCounter)ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_PC)!;
                    readPc.ProgramCounter = 0x12345678;
                    return readPc;
                case MasterCommandEnum.CMND_GO:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_GET_SYNC:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_CHIP_ERASE:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_ENTER_PROGMODE:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_LEAVE_PROGMODE:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_CLEAR_EVENTS:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_RESTORE_TARGET:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);

                // multiple byte commands
                case MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_SELFTEST:
                    var selfTest = (ResponseSelfTest)ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_SELFTEST)!;
                    selfTest.SetSelfTestResult(0, SelfTestReponseEnum.SELFTEST_OK);
                    selfTest.SetSelfTestResult(1, SelfTestReponseEnum.SELFTEST_FAILED);
                    selfTest.SetSelfTestResult(2, SelfTestReponseEnum.SELFTEST_OK);
                    selfTest.SetSelfTestResult(3, SelfTestReponseEnum.SELFTEST_OK);
                    selfTest.SetSelfTestResult(4, SelfTestReponseEnum.SELFTEST_OK);
                    selfTest.SetSelfTestResult(5, SelfTestReponseEnum.SELFTEST_OK);
                    selfTest.SetSelfTestResult(6, SelfTestReponseEnum.SELFTEST_OK);
                    selfTest.SetSelfTestResult(7, SelfTestReponseEnum.SELFTEST_SKIPPED);
                    return selfTest;
                case MasterCommandEnum.CMND_SPI_CMD:
                    var spi = (ResponseMultipleByte)ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_SPI_DATA)!;
                    spi.Data.AddRange(new byte[] { 0x01, 0x02, 0x03 });
                    return spi;
                case MasterCommandEnum.CMND_SET_PARAMETER:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_GET_PARAMETER:
                    var commandGetParam = (CommandParameter)commandReceived;
                    var responseParam = (ResponseMultipleByte)ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_PARAMETER)!;

                    Parameter parameter = _parameters[commandGetParam.ParameterId];
                    switch (parameter.Size)
                    {
                        case 1:
                            responseParam.Data.Add((byte)(parameter.Value & 0xFF));
                            break;
                        case 2:
                            responseParam.Data.Add((byte)(parameter.Value & 0xFF));
                            responseParam.Data.Add((byte)((parameter.Value >> 8) & 0xFF));
                            break;
                        case 4:
                            responseParam.Data.Add((byte)(parameter.Value & 0xFF));
                            responseParam.Data.Add((byte)((parameter.Value >> 8) & 0xFF));
                            responseParam.Data.Add((byte)((parameter.Value >> 16) & 0xFF));
                            responseParam.Data.Add((byte)((parameter.Value >> 24) & 0xFF));
                            break;
                        default:
                            throw new Exception($"Invalid parameter size {parameter.Size}");
                    }
                    return responseParam;
                case MasterCommandEnum.CMND_WRITE_MEMORY:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_READ_MEMORY:
                    var memory = (ResponseMultipleByte)ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_MEMORY)!;
                    var request = (CommandMemory)commandReceived;
                    for (int i = 0; i < request.ByteCount; i++)
                    {
                        memory.Data.Add((byte)(i));
                    }
                    return memory;
                case MasterCommandEnum.CMND_WRITE_PC:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_RUN_TO_ADDR:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);

                case MasterCommandEnum.CMND_SINGLE_STEP:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);

                case MasterCommandEnum.CMND_FORCED_STOP:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_RESET:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);

                case MasterCommandEnum.CMND_ERASEPAGE_SPM:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);

                case MasterCommandEnum.CMND_GET_BREAK:
                    var _break = (ResponseBreakpoint)ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_GET_BREAK)!;
                    _break.BreakpontType = BreakpointTypeEnumS.BKPT_PRG_MEMORY;
                    _break.Address = 0x12345678;
                    _break.BreakpointMode = JTAGICEmkII.Slave.BreakpointModeEnumS.BKPT_MODE_PROGRAM;
                    return _break;
                case MasterCommandEnum.CMND_SET_BREAK:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);

                case MasterCommandEnum.CMND_CLR_BREAK:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);

                case MasterCommandEnum.CMND_SET_N_PARAMETERS:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);

                default:
                    return null!;
            }
        }

        protected void RxFrame_RxTimerExpired(object? sender, EventArgs e) => _timeoutOccured = true;

        protected void RxFrame_ResponseReceived(object? sender, ResponseReceivedEventArgs e)
        {
            Logger.Info($"Response received: {e.Response.ResponseId}");
            if (_timeoutOccured)
            {
                _responseReceived = null;
                return;
            }

            _nbResponsesReceived++;
            _responseReceived = e.Response;
        }

        protected void CheckResult(ICommandResult result)
        {
            Assert.NotNull(result);
            Assert.True(result.IsSuccess, $"Expected success but got failure:  {((SlaveResponseEnum)result.ErrorCode)}, {result.ErrorDesctiption}");
            if (!result.IsSuccess)
                Logger.Debug($"Expected success but got failure:  {((SlaveResponseEnum)result.ErrorCode)}, {result.ErrorDesctiption}");

        }

        protected Parameters BuildParametersForTest()
        {
            var parameters = new Parameters();

            uint i = 1;
            foreach (var param in parameters.Values)
            {
                switch (param.Size)
                {
                    case 1:
                        param.Value = (uint)(0xFF - i);
                        break;
                    case 2:
                        param.Value = (uint)(0xFFFF - i);
                        break;
                    case 4:
                        param.Value = 0xFFFFFFFF - i;
                        break;
                    default:
                        throw new Exception($"Invalid parameter size {param.Size}");
                }
            }
            return parameters;
        }

        protected bool RunTaskActivity(IActivityElement activityInTest, Type expectedType)
        {
            return RunTaskActivity(activityInTest, expectedType, null, null);
        }

        protected bool RunTaskActivity(IActivityElement activityInTest, Type expectedType, Func<ICommandResult>? fct)
        {
            return RunTaskActivity(activityInTest, expectedType, fct, null);
        }

        protected bool RunTaskActivity(IActivityElement activityInTest, Type expectedType, Func<ICommandResult>? fct, Func<bool>? condition)
        {
            Task t = Task.Run(() => { return _activityStructure.RunActivity(activityInTest); }, _cancellationSource.Token);

            if (t != null)
            {
                var retval = false;
                if (fct != null)
                {
                    retval = fct().IsSuccess;
                }
                else
                {
                    retval = true;
                }

                // Wait for the condition to be true, or for the activity to change
                while (condition != null && !condition())
                {
                    Task.Delay(1).Wait();
                }

                while (_activityStructure.CurrentActivity?.GetType() != expectedType)
                {
                    Task.Delay(1).Wait();
                }

                try
                {
                    _cancellationSource.Cancel(true);
                    t.Wait(100);
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is TaskCanceledException)
                    {
                        // Expected exception, do nothing
                        return retval;
                    }
                    else
                    {
                        throw;
                    }
                }

                return retval;
            }

            return false;
        }



        protected HostDeviceServiceMoq CreateHostServiceMoq(int timeout = -1)
        {
            var hostService = new HostDeviceServiceMoq();
            _activityStructure = new StructureActivity(hostService);
            return hostService;
        }

        protected virtual HostDeviceService CreateHostService(int timeout = -1, bool callInit = true)
        {
            FifoBuffer<byte> buffer = new FifoBuffer<byte>();
            var rxadapt = new Moq.RxFrameFifoMemory(buffer, timeout);
            var txadapt = new Moq.TxFrameFifoMemory(buffer);
            _rxFrame = new RxFrame(rxadapt);
            rxadapt.Attach(_rxFrame);

            _rxFrame.ResponseReceived += RxFrame_ResponseReceived;
            _rxFrame.CommandReceived += RxFrame_CommandReceived;
            _rxFrame.RxTimerExpired += RxFrame_RxTimerExpired;

            _txFrame = new TxFrame(txadapt);

            var hostService = new HostDeviceService(_rxFrame, _txFrame);
            _activityStructure = hostService.ActivityStructure;
            _hostService = hostService;

            if (callInit)
            {
                Assert.True(hostService.Initialise());

            }
            return hostService;
        }

    }
}
