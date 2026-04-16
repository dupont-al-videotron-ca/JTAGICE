using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII;
using JTAGICEmkII.HostService;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using log4net;
using MyFramework;
using Xunit;

namespace JTAGICEmkIITest
{
    public class HostServiceTest : FrameTest
    {
        private RxFrame? _rxFrame;
        private TxFrame? _txFrame;
        private bool _timeoutOccured;
        private ISlaveResponse? _responseReceived;
        private IMasterCommand? _commandReceived;
        private int _nbResponsesReceived = 0;
        private int _nbCommandReceived = 0;
        public HostServiceTest() : base()
        {
            _timeoutOccured = false;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_rxFrame != null)
                {
                    _rxFrame.ResponseReceived -= RxFrame_ResponseReceived;
                    _rxFrame.RxTimerExpired -= RxFrame_RxTimerExpired;
                    _rxFrame.Dispose();
                }

                _txFrame?.Dispose();
            }

            base.Dispose(disposing);
        }

        [Fact]
        public void HostService_Constructor()
        {
            var test = CreateHostService();
            Assert.NotNull(test);
            Assert.True(true);
        }


        [Fact]
        public void HostService_shall_Initialise()
        {

            //--- Setup
            var hostService = CreateHostService();

            //--- Expectations

            //--- Action
            var result = hostService.Initialise();

            //--- Verification
            Assert.True(result);


        }


        private HostDeviceService CreateHostService(int timeout = -1)
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
            return hostService;
        }

        private void RxFrame_CommandReceived(object? sender, CommandReceivedEventArgs e)
        {
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
                // No response for this command, do not send anything
                Logger.Debug($"No response for command {e.Command.MessageId}, not sending anything");
                return;
            }

            _txFrame?.BuildAndSendFrameResponse((Response)response);
        }

        private ISlaveResponse? CreateResponse(IMasterCommand commandReceived)
        {
            switch (commandReceived.MessageId)
            {
                // Single byte commands
                case MasterCommandEnum.CMND_SIGN_OFF:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_GET_SIGN_ON:
                    var signOn = (ResponseSignOn)ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_SIGN_ON)!;
                    signOn.CommunicationProtocolVersion = 1;
                    return signOn;
                case MasterCommandEnum.CMND_READ_PC:
                    var readPc = (ResponseProgranCounter)ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_PC)!;
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
                    selfTest.SetSelfTestResult(0, SelfTestReponseEnum.OK);
                    selfTest.SetSelfTestResult(1, SelfTestReponseEnum.Failed);
                    selfTest.SetSelfTestResult(2, SelfTestReponseEnum.OK);
                    selfTest.SetSelfTestResult(3, SelfTestReponseEnum.OK);
                    selfTest.SetSelfTestResult(4, SelfTestReponseEnum.OK);
                    selfTest.SetSelfTestResult(5, SelfTestReponseEnum.OK);
                    selfTest.SetSelfTestResult(6, SelfTestReponseEnum.OK);
                    selfTest.SetSelfTestResult(7, SelfTestReponseEnum.SKIPPED);
                    return selfTest;
                case MasterCommandEnum.CMND_SPI_CMD:
                    var spi = (ResponseMultipleByte)ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_SPI_DATA)!;
                    spi.Data.AddRange(new byte[] { 0x01, 0x02, 0x03 });
                    return spi;
                case MasterCommandEnum.CMND_SET_PARAMETER:
                    return ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK);
                case MasterCommandEnum.CMND_GET_PARAMETER:
                    var param = (ResponseMultipleByte)ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_PARAMETER)!;
                    param.Data.AddRange(new byte[] { 0x01 });
                    return param;
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
                    _break.BreakpontType = BreakpontTypeEnum.BKPT_PRG_MEMORY;
                    _break.Address = 0x12345678;
                    _break.BreakpointMode = JTAGICEmkII.Slave.BreakpointModeEnum.BKPT_MODE_PROGRAM;
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

        private void RxFrame_RxTimerExpired(object? sender, EventArgs e) => _timeoutOccured = true;

        private void RxFrame_ResponseReceived(object? sender, ResponseReceivedEventArgs e)
        {
            if (_timeoutOccured)
            {
                _responseReceived = null;
                return;
            }

            _nbResponsesReceived++;
            _responseReceived = e.Response;
        }
    }
}
