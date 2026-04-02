using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;
using Xunit;

namespace JTAGICEmkIITest
{
    public class RxFrameTest : FrameTest
    {
        public RxFrameTest() : base()
        {

        }

        [Fact]
        public void GetByte_shall_return_bytes_in_order()
        {
            byte[] buffer = new byte[] { 0x01, 0x02, 0x03 };
            var rxFrame = new Moq.RxFrameMoq(buffer);
            Assert.Equal(0x01, rxFrame.GetByte());
            Assert.Equal(0x02, rxFrame.GetByte());
            Assert.Equal(0x03, rxFrame.GetByte());
        }
        [Fact]
        public void GetBytes_shall_return_bytes_in_order()
        {
            byte[] buffer = new byte[] { 0x01, 0x02, 0x03 };
            var rxFrame = new Moq.RxFrameMoq(buffer);
            Assert.Equal(buffer, rxFrame.GetBytes((uint)buffer.Length));
        }
        [Fact]
        public async Task GetByteAsync_shall_return_bytes_in_order()
        {
            byte[] buffer = new byte[] { 0x01, 0x02, 0x03 };
            byte[] result = new byte[buffer.Length];

            var rxFrame = new Moq.RxFrameMoq(buffer);
            result[0] = await rxFrame.GetByteAsync(CancellationToken.None);
            result[1] = await rxFrame.GetByteAsync(CancellationToken.None);
            result[2] = await rxFrame.GetByteAsync(CancellationToken.None);

            Assert.Equal(buffer, result);
        }
        [Fact]
        public async Task GetBytesAsync_shall_return_bytes_in_order()
        {
            byte[] buffer = new byte[] { 0x01, 0x02, 0x03 };
            var rxFrame = new Moq.RxFrameMoq(buffer);

            var result = await rxFrame.GetBytesAsync((uint)buffer.Length, CancellationToken.None);

            Assert.Equal(buffer, result);
        }

        [Theory]
        [InlineData(SlaveResponseEnum.RSP_OK, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_FAILED, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_PARAMETER, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_MEMORY_TYPE, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_MEMORY_RANGE, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_COMMAND, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_VALUE, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_BREAKPOINT, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_JTAG_ID, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_NO_TARGET_POWER, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_DEBUGWIRE_SYNC_FAILED, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_POWER_STATE, typeof(Response))]
        public void StartReceiving_shall_receive_Response(SlaveResponseEnum evtId, Type expectedType)
        {
            var frame = Create1ByteResponse(evtId);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(evtId, result.ResponseId);
            Assert.IsAssignableFrom(expectedType, result);
        }

        [Fact]
        public void StartReceiving_shall_receive_message_RSP_OK_and_wait_forever()
        {
            var frame = Create1ByteResponse(SlaveResponseEnum.RSP_OK);
            var test = new Moq.RxFrameMoq(frame);
            test.WaitForEver = true;

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.RSP_OK, result.ResponseId);

        }

        [Theory]
        [InlineData(SlaveResponseEnum.RSP_PARAMETER, typeof(ResponseMultipleByte))]
        [InlineData(SlaveResponseEnum.RSP_MEMORY, typeof(ResponseMultipleByte))]
        [InlineData(SlaveResponseEnum.RSP_SPI_DATA, typeof(ResponseMultipleByte))]

        public void StartReceiving_shall_receive_ResponseMultipleByte(SlaveResponseEnum responsesId, Type expectedType)
        {
            var payload = new byte[] { 0x01, 0x02, 0x03 };
            var frame = CreateBytesResponse(responsesId, payload);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(responsesId, result.ResponseId);
            Assert.IsAssignableFrom(expectedType, result);
            Assert.Equal(payload, ((ResponseMultipleByte)result).GetDataBytes());
        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseBreakpoint()
        {
            var payload = new byte[] {
                (byte)BreakpontTypeEnum.BKPT_PRG_MEMORY,
                0x01, 0x00, 0x00, 0x00, // Address 0x00000001 
                (byte)BreakpointModeEnum.BKPT_MODE_PROGRAM};

            var frame = CreateBytesResponse(SlaveResponseEnum.RSP_GET_BREAK, payload);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.RSP_GET_BREAK, result.ResponseId);

            Assert.IsAssignableFrom<ResponseBreakpoint>(result);
            ResponseBreakpoint responseBreakpoint = (ResponseBreakpoint)result;
            Assert.Equal(BreakpontTypeEnum.BKPT_PRG_MEMORY, responseBreakpoint.BreakpontType);
            Assert.Equal(BreakpointModeEnum.BKPT_MODE_PROGRAM, responseBreakpoint.BreakpointMode);
            Assert.Equal((UInt32)0x000001, responseBreakpoint.Address);
        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseMcuState_shall_time_out()
        {
            var payload = new byte[] {
                (byte)McuStateEnum.RUNNING};

            var frame = CreateBytesResponse(SlaveResponseEnum.RSP_GET_BREAK, payload);
            var test = new Moq.RxFrameMoq(frame, 1000);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.True(timerExpired);
            Assert.Null(result);

            this.VerifyLogForError = false;
            var logEntrie = base.LoggedEvents.FirstOrDefault(entry => entry.Level >= log4net.Core.Level.Fatal);
            Assert.Equal("DispathMessageBody: Specified argument was out of the range of valid values. (Parameter 'Data length is insufficient for response ResponseBreakpoint.')", logEntrie?.RenderedMessage?.ToString());
        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseMcuState()
        {
            var payload = new byte[] {
                (byte)McuStateEnum.RUNNING};

            var frame = CreateBytesResponse(SlaveResponseEnum.RSP_ILLEGAL_MCU_STATE, payload);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.RSP_ILLEGAL_MCU_STATE, result.ResponseId);

            Assert.IsAssignableFrom<ResponseMcuState>(result);
            ResponseMcuState responseMcuState = (ResponseMcuState)result;
            Assert.Equal(McuStateEnum.RUNNING, responseMcuState.State);
        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseProgranCounter()
        {
            UInt32 expectedProgramCounter = 0x04030201;
            var payload = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            var frame = CreateBytesResponse(SlaveResponseEnum.RSP_PC, payload);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.RSP_PC, result.ResponseId);

            Assert.IsAssignableFrom<ResponseProgranCounter>(result);
            ResponseProgranCounter responseProgranCounter = (ResponseProgranCounter)result;
            Assert.Equal(expectedProgramCounter, responseProgranCounter.ProgramCounter);
        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseSelfTest()
        {
            var payload = new byte[] {
                (byte)SelfTestReponseEnum.OK,
                (byte)SelfTestReponseEnum.SKIPPED,
                (byte)SelfTestReponseEnum.Failed,
                (byte)SelfTestReponseEnum.OK,
                (byte)SelfTestReponseEnum.SKIPPED,
                (byte)SelfTestReponseEnum.Failed,
                (byte)SelfTestReponseEnum.SKIPPED,
                (byte)SelfTestReponseEnum.Failed,
            };

            var frame = CreateBytesResponse(SlaveResponseEnum.RSP_SELFTEST, payload);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.RSP_SELFTEST, result.ResponseId);

            Assert.IsAssignableFrom<ResponseSelfTest>(result);
            ResponseSelfTest responseSelfTest = (ResponseSelfTest)result;


            var expectedResult = new SelfTestReponseEnum[] {
                SelfTestReponseEnum.OK,
                SelfTestReponseEnum.SKIPPED,
                SelfTestReponseEnum.Failed,
                SelfTestReponseEnum.OK,
                SelfTestReponseEnum.SKIPPED,
                SelfTestReponseEnum.Failed,
                SelfTestReponseEnum.SKIPPED,
                SelfTestReponseEnum.Failed
            };

            Assert.Equal(expectedResult, responseSelfTest.SelfTestResults);
        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseSignOn()
        {
            var payload = new byte[] {
                0x01, // CommunicationProtocolVersion
                0x02, // MasterMcuBootLoaderVersion
                0x05, // MasterMcuFirmwareVersionMinor
                0x04, // MasterMcuFirmwareVersionMajor
                0x03, // MasterMcuHwVersion
                0x06, // SlaveMcuBootLoaderVersion
                0x08, // SlaveMcuFirmwareVersionMinor
                0x07, // SlaveMcuFirmwareVersionMajor
                0x09, // SlaveMcuHwVersion
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, // SerialNumber
                (byte)'A', 0x00 // DeviceId string null terminate
            };

            var frame = CreateBytesResponse(SlaveResponseEnum.RSP_SIGN_ON, payload);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.RSP_SIGN_ON, result.ResponseId);

            Assert.IsAssignableFrom<ResponseSignOn>(result);
            ResponseSignOn responseSelfTest = (ResponseSignOn)result;

            var expectedResult = new ResponseSignOn(SlaveResponseEnum.RSP_SIGN_ON)
            {
                CommunicationProtocolVersion = 0x01,
                MasterMcuBootLoaderVersion = 0x02,
                MasterMcuHwVersion = 0x03,
                MasterMcuFirmwareVersionMajor = 0x04,
                MasterMcuFirmwareVersionMinor = 0x05,
                SlaveMcuBootLoaderVersion = 0x06,
                SlaveMcuFirmwareVersionMajor = 0x07,
                SlaveMcuFirmwareVersionMinor = 0x08,
                SlaveMcuHwVersion = 0x09,
                SerialNumber = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 },
                DeviceId = new byte[] { (byte)'A', 0x00 }
            };

            Assert.Equal(expectedResult.SerialNumber, responseSelfTest.SerialNumber);
            Assert.Equal(expectedResult.DeviceId, responseSelfTest.DeviceId);
            Assert.Equal(expectedResult.CommunicationProtocolVersion, responseSelfTest.CommunicationProtocolVersion);
            Assert.Equal(expectedResult.MasterMcuBootLoaderVersion, responseSelfTest.MasterMcuBootLoaderVersion);
            Assert.Equal(expectedResult.MasterMcuFirmwareVersionMajor, responseSelfTest.MasterMcuFirmwareVersionMajor);
            Assert.Equal(expectedResult.MasterMcuFirmwareVersionMinor, responseSelfTest.MasterMcuFirmwareVersionMinor);
            Assert.Equal(expectedResult.MasterMcuHwVersion, responseSelfTest.MasterMcuHwVersion);
            Assert.Equal(expectedResult.SlaveMcuBootLoaderVersion, responseSelfTest.SlaveMcuBootLoaderVersion);
            Assert.Equal(expectedResult.SlaveMcuHwVersion, responseSelfTest.SlaveMcuHwVersion);
            Assert.Equal(expectedResult.SlaveMcuFirmwareVersionMajor, responseSelfTest.SlaveMcuFirmwareVersionMajor);
            Assert.Equal(expectedResult.SlaveMcuFirmwareVersionMinor, responseSelfTest.SlaveMcuFirmwareVersionMinor);

            Assert.Equal("654321", responseSelfTest.SerialNumberString);
            Assert.Equal("A", responseSelfTest.DeviceIdString);
        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseEventDebug()
        {
            var payload = new byte[] {
                0x55, // EventId
                (byte) CommStateEnum.Start, // CommState
                (byte) CommErrorEnum.Timeout // CommError
            };

            var frame = CreateEventResponse(SlaveResponseEnum.EVT_DEBUG, payload);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.EVT_DEBUG, result.ResponseId);

            Assert.IsAssignableFrom<ResponseEventDebug>(result);
            ResponseEventDebug responseEventDebug = (ResponseEventDebug)result;

            var expectedResult = new ResponseEventDebug(SlaveResponseEnum.EVT_DEBUG)
            {
                EventId = 0x55,
                CommState = CommStateEnum.Start,
                CommError = CommErrorEnum.Timeout
            };

            Assert.Equal(expectedResult.EventId, responseEventDebug.EventId);
            Assert.Equal(expectedResult.CommState, responseEventDebug.CommState);
            Assert.Equal(expectedResult.CommError, responseEventDebug.CommError);
        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseEventBreak()
        {
            var payload = new byte[] {
                0xDD, 0xBB, 0xAA, 0x55, // ProgramCounter 0x55AABBDD
                (byte)EventBreakCauseEnum.PROGRAMME_BREAK // BreakCause
            };

            var frame = CreateEventResponse(SlaveResponseEnum.EVT_BREAK, payload);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.EVT_BREAK, result.ResponseId);

            Assert.IsAssignableFrom<ResponseEventBreak>(result);
            ResponseEventBreak responseEventDebug = (ResponseEventBreak)result;

            var expectedResult = new ResponseEventBreak(SlaveResponseEnum.EVT_DEBUG)
            {
                ProgramCounter = 0x55AABBDD,
                BreakCause = EventBreakCauseEnum.PROGRAMME_BREAK,
            };

            Assert.Equal(expectedResult.ProgramCounter, responseEventDebug.ProgramCounter);
            Assert.Equal(expectedResult.BreakCause, responseEventDebug.BreakCause);
        }

        [Theory]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_FROECE_BREAK_TIMEOUT, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_TARGET_POWER_ON, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_TARGET_POWER_OFF, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_EXTERNAL_RESET, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_TARGET_SLEEP, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_TARGET_WAKEUP, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ICE_POWER_ERROR_STATE, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ICE_POWER_OK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_IDR_DIRTY, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_PROGRAM_BREAK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_PDSB_BREAK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_PDSMB_BREAK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_MAX_BIT_LENGHT_DIFF, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMeOUT_BAUD, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_OUT_OF_RANGE, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_WAIT_TIMEOUT, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_TIMEOUT, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_BREAK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVE_TIMEOUT, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVED_BREAK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_NO_ACTIVITY, typeof(ResponseEvent))]
        public void StartReceiving_shall_receive_ResponseEvent(SlaveResponseEnum evtId, Type expectedType)
        {
            var payload = new byte[]
            {
            };

            var frame = CreateEventResponse(evtId, payload);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(evtId, result.ResponseId);

            Assert.IsAssignableFrom(expectedType, result);

        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseEmulatorMode()
        {
            var payload = new byte[]
            {
                    (byte)EmulatorModeEnum.MODE_JTAG,
            };

            var frame = CreateBytesResponse(SlaveResponseEnum.RSP_ILLEGAL_EMULATOR_MODE, payload);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.RSP_ILLEGAL_EMULATOR_MODE, result.ResponseId);

            Assert.IsAssignableFrom<ResponseEmulatorMode>(result);
            ResponseEmulatorMode responseEmulatorMode = (ResponseEmulatorMode)result;
            Assert.Equal(EmulatorModeEnum.MODE_JTAG, responseEmulatorMode.EmulatorMode);

        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseEventRun()
        {
            var payload = new byte[]
            {
                0x1F,
            };

            var frame = CreateBytesResponse(SlaveResponseEnum.EVT_RUN, payload);
            var test = new Moq.RxFrameMoq(frame);

            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.EVT_RUN, result.ResponseId);

            Assert.IsAssignableFrom<ResponseEventRun>(result);
            ResponseEventRun responseEventRun = (ResponseEventRun)result;
            Assert.Equal(payload[0], responseEventRun.RunCause);

        }


        [Theory]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_FROECE_BREAK_TIMEOUT, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_TARGET_POWER_ON, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_TARGET_POWER_OFF, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_EXTERNAL_RESET, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_TARGET_SLEEP, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_TARGET_WAKEUP, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ICE_POWER_ERROR_STATE, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ICE_POWER_OK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_IDR_DIRTY, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_PROGRAM_BREAK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_PDSB_BREAK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_PDSMB_BREAK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_MAX_BIT_LENGHT_DIFF, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMeOUT_BAUD, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_OUT_OF_RANGE, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_WAIT_TIMEOUT, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_TIMEOUT, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_BREAK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVE_TIMEOUT, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVED_BREAK, typeof(ResponseEvent))]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_NO_ACTIVITY, typeof(ResponseEvent))]

        [InlineData(SlaveResponseEnum.RSP_OK, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_FAILED, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_PARAMETER, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_MEMORY_TYPE, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_MEMORY_RANGE, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_COMMAND, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_VALUE, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_BREAKPOINT, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_JTAG_ID, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_NO_TARGET_POWER, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_DEBUGWIRE_SYNC_FAILED, typeof(Response))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_POWER_STATE, typeof(Response))]

        [InlineData(SlaveResponseEnum.RSP_PARAMETER, typeof(ResponseMultipleByte))]
        [InlineData(SlaveResponseEnum.RSP_MEMORY, typeof(ResponseMultipleByte))]
        [InlineData(SlaveResponseEnum.RSP_SPI_DATA, typeof(ResponseMultipleByte))]

        [InlineData(SlaveResponseEnum.RSP_SELFTEST, typeof(ResponseSelfTest))]
        [InlineData(SlaveResponseEnum.RSP_GET_BREAK, typeof(ResponseBreakpoint))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_EMULATOR_MODE, typeof(ResponseEmulatorMode))]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_MCU_STATE, typeof(ResponseMcuState))]
        [InlineData(SlaveResponseEnum.RSP_PC, typeof(ResponseProgranCounter))]
        [InlineData(SlaveResponseEnum.RSP_SIGN_ON, typeof(ResponseSignOn))]
        [InlineData(SlaveResponseEnum.EVT_BREAK, typeof(ResponseEventBreak))]
        [InlineData(SlaveResponseEnum.EVT_RUN, typeof(ResponseEventRun))]
        [InlineData(SlaveResponseEnum.EVT_DEBUG, typeof(ResponseEventDebug))]
        public void ResponseFactory_CreateReponses_shall_crete_the_right_object(SlaveResponseEnum testId, Type expectedType)
        {
            var result = ResponseFactory.CreateResponse(testId);
            Assert.IsAssignableFrom(expectedType, result);
        }

        [Theory]
        [InlineData(-1, 0x0000, 0x0000, false)]
        [InlineData(0x0000, 0x0001, 0x0001, false)]
        [InlineData(0xFFFE, 0x0000, 0x0000, false)]
        [InlineData(0xFFFD, 0xFFFE, 0xFFFE, false)]
        [InlineData(0x0000, 0x0002, 0x0002, false)]
        [InlineData(0x0005, 0x0004, 0x0005, true)]
        [InlineData(0x0005, 0x0005, 0x0005, true)]
        [InlineData(0x0005, 0xFFFE, 0x0005, true)]
        public void StartReceiving_shall_manage_sequence_number(int previousSequence, UInt16 frameSequence, UInt16 expectedSequence, bool expectedTimerExpired)
        {
            var frame = Create1ByteResponse(SlaveResponseEnum.RSP_OK, frameSequence);
            int localTimeout;

            if (expectedTimerExpired)
                localTimeout = 1000;
            else
                localTimeout = 1000 * 60 * 60;

            var test = new Moq.RxFrameMoq(frame, localTimeout);

            Assert.Equal(-1, test.PreviousSequenceNumber);
            test.PreviousSequenceNumber = previousSequence;
            ISlaveResponse? result = this.TestReceiver(test, out bool timerExpired);

            Assert.Equal(expectedTimerExpired, timerExpired);
            if (expectedTimerExpired)
            {
                Assert.Null(result);
            }
            else
            {
                Assert.NotNull(result);
                Assert.Equal(SlaveResponseEnum.RSP_OK, result.ResponseId);
                Assert.IsAssignableFrom<Response>(result);
                Assert.Equal(expectedSequence, test.PreviousSequenceNumber);
            }
        }
    }
}
