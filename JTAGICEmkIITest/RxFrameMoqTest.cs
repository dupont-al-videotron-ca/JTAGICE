using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using JTAGICEmkII.Frame;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using JTAGICEmkIITest.Moq;
using Xunit;

namespace JTAGICEmkIITest
{
    public class RxFrameMoqTest : FrameTest
    {
        public RxFrameMoqTest() : base()
        {

        }

        [Fact]
        public void StartReceiving_shall_receive_Command_SignOff()
        {

            var frame = Create1ByteCommand(MasterCommandEnum.CMND_SIGN_OFF);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<Command>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_SIGN_OFF, result.MessageId);

        }

        [Fact]
        public void StartReceiving_shall_receive_Command_SignOn()
        {

            var frame = Create1ByteCommand(MasterCommandEnum.CMND_GET_SIGN_ON);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<Command>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_GET_SIGN_ON, result.MessageId);

        }

        [Fact]
        public void StartReceiving_shall_receive_Command_Address()
        {
            byte[] payload = new byte[] { 0x01, 0x02, 0x03, 04 };
            var frame = CreateBytesCommand(MasterCommandEnum.CMND_ERASEPAGE_SPM, payload);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<CommandAddress>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_ERASEPAGE_SPM, result.MessageId);
            Assert.Equal(payload, BitConverter.GetBytes(((CommandAddress)result).Address));

        }
        [Fact]
        public void StartReceiving_shall_receive_Command_BreakAddress()
        {
            byte[] payload = new byte[] {0x55, 0x01, 0x02, 0x03, 04 };
            var frame = CreateBytesCommand(MasterCommandEnum.CMND_CLR_BREAK, payload);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<CommandBreakAddress>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_CLR_BREAK, result.MessageId);
            Assert.Equal(payload[0],(byte)((CommandBreakAddress)result).BreakNumber);
            Assert.Equal(payload.AsSpan<byte>().Slice(1, 4).ToArray(), BitConverter.GetBytes(((CommandBreakAddress)result).Address));
        }

        [Fact]
        public void StartReceiving_shall_receive_Command_BreakNumber()
        {
            byte[] payload = new byte[] { 0x55 };
            var frame = CreateBytesCommand(MasterCommandEnum.CMND_GET_BREAK, payload);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<CommandBreakNumber>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_GET_BREAK, result.MessageId);
            Assert.Equal(payload[0], (byte)((CommandBreakNumber)result).BreakNumber);
        }

        [Fact]
        public void StartReceiving_shall_receive_Command_Breakpoint()
        {
            byte[] payload = new byte[] { (byte)BreakpointTypeEnum.BKPT_PRG_MEMORY, 
                            0x55, 
                            0x01, 0x02, 0x03, 0x04, 
                            (byte)JTAGICEmkII.Master.BreakpointModeEnum.BKPT_MODE_PROGRAM };
            var frame = CreateBytesCommand(MasterCommandEnum.CMND_SET_BREAK, payload);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<CommandBreakpoint>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_SET_BREAK, result.MessageId);
            Assert.Equal(payload[0], (byte)((CommandBreakpoint)result).Type);
            Assert.Equal(payload[1], (byte)((CommandBreakpoint)result).BreakNumber);
            Assert.Equal(payload.AsSpan<byte>().Slice(2, 4).ToArray(), BitConverter.GetBytes(((CommandBreakpoint)result).Address));
            Assert.Equal(payload[6], (byte)((CommandBreakpoint)result).Mode);
        }

        [Fact]
        public void StartReceiving_shall_receive_Command_Memory()
        {
            byte[] payload = new byte[] { (byte)MemoryTypeEnum.MT_FLASH_PAGE,
                            0x01, 0x00, 0x00, 0x00, // ByteCount 1
                            0x01, 0x02, 0x03, 0x04, // address 0x04030201
                            0xff};
            var frame = CreateBytesCommand(MasterCommandEnum.CMND_WRITE_MEMORY, payload);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<CommandMemory>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_WRITE_MEMORY, result.MessageId);
            Assert.Equal(payload[0], (byte)((CommandMemory)result).MemoryType);
            Assert.Equal(payload.AsSpan<byte>().Slice(1, 4).ToArray(), BitConverter.GetBytes(((CommandMemory)result).ByteCount));
            Assert.Equal(payload.AsSpan<byte>().Slice(5, 4).ToArray(), BitConverter.GetBytes(((CommandMemory)result).Address));
            Assert.Equal(payload.AsSpan<byte>().Slice(9, 1).ToArray(), ((CommandMemory)result).Data.ToArray());
            Assert.Equal(((CommandMemory)result).ByteCount, (UInt32)((CommandMemory)result).Data.Count);
        }

        [Fact]
        public void StartReceiving_shall_receive_Command_MultipleByte()
        {
            byte[] payload = new byte[] { 0x00,
                            0x01, 0x00, 0x00, 0x00, 
                            0x01, 0x02, 0x03, 0x04, 
                            0xff};
            var frame = CreateBytesCommand(MasterCommandEnum.CMND_SELFTEST, payload);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<CommandMultipleByte>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_SELFTEST, result.MessageId);
            Assert.Equal(payload, ((CommandMultipleByte)result).Data);
            Assert.Equal(payload.Length, (Int32)((CommandMultipleByte)result).Data.Count);
        }

        [Fact]
        public void StartReceiving_shall_receive_Command_NParameter()
        {
            byte[] payload = new byte[] { 0x01, (byte)ParameterEnum.PARAM_BAUD_RATE,
                            0x01, 0x00, 0x00, 0x00 // value
            };

            var frame = CreateBytesCommand(MasterCommandEnum.CMND_SET_N_PARAMETERS, payload);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<CommandNParameter>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_SET_N_PARAMETERS, result.MessageId);
            Assert.Equal(payload[0], ((CommandNParameter)result).NumberOfParameters);
            Assert.Equal(payload[0], ((CommandNParameter)result).Parameters.Count);
            var paramResult = ((CommandNParameter)result).Parameters[0];

            Assert.Equal(payload[1], (byte)paramResult.ParameterId);
            Assert.Equal(payload.AsSpan<byte>().Slice(2, 4).ToArray(), BitConverter.GetBytes(paramResult.ParameterData));
        }

        [Fact]
        public void StartReceiving_shall_receive_Command_Parameter()
        {
            byte[] payload = new byte[] { (byte)ParameterEnum.PARAM_BAUD_RATE,
                            0x01, 0x00, 0x00, 0x00 // value
            };

            var frame = CreateBytesCommand(MasterCommandEnum.CMND_SET_PARAMETER, payload);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<CommandParameter>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_SET_PARAMETER, result.MessageId);
            Assert.Equal(payload[0], (byte)((CommandParameter)result).ParameterId);
            Assert.Equal(payload.AsSpan<byte>().Slice(1, 4).ToArray(), ((CommandParameter)result).Data);
        }

        [Fact]
        public void StartReceiving_shall_receive_Command_ProgramCounter()
        {
            byte[] payload = new byte[] { 0x01, 0x02, 0x03, 0x04 // value
            };

            var frame = CreateBytesCommand(MasterCommandEnum.CMND_RUN_TO_ADDR, payload);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<CommandProgramCounter>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_RUN_TO_ADDR, result.MessageId);
            Assert.Equal(payload.AsSpan<byte>().Slice(0, 4).ToArray(), BitConverter.GetBytes(((CommandProgramCounter)result).ProgramCounter));
        }

        [Fact]
        public void StartReceiving_shall_receive_Command_SingleStep()
        {
            byte[] payload = new byte[] { (byte)ExceutionModeEnum.EXMODE_LOW_LEVEL,
            (byte)StepModeEnum.Into};

            var frame = CreateBytesCommand(MasterCommandEnum.CMND_SINGLE_STEP, payload);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<CommandSingleStep>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_SINGLE_STEP, result.MessageId);
            Assert.Equal(payload[0], (byte)((CommandSingleStep)result).ExecutionMode);
            Assert.Equal(payload[1], (byte)((CommandSingleStep)result).StepMode);
        }

        [Fact]
        public void StartReceiving_shall_receive_Command_PCMode()
        {
            byte[] payload = new byte[] { (byte)ExceutionModeEnum.EXMODE_LOW_LEVEL};

            var frame = CreateBytesCommand(MasterCommandEnum.CMND_FORCED_STOP, payload);
            var test = CreateFrameForTest(frame);

            IMasterCommand? result = this.TestReceiverMaster(test, out bool timerExpired);

            Assert.IsAssignableFrom<CommandPCMode>(result);
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(MasterCommandEnum.CMND_FORCED_STOP, result.MessageId);
            Assert.Equal(payload[0], (byte)((CommandPCMode)result).ExecutionMode);
        }

        [Fact]
        public void GetByte_shall_return_bytes_in_order()
        {
            byte[] buffer = new byte[] { 0x01, 0x02, 0x03 };
            var rxFrame = CreateFrameForTest(buffer);

            byte value;
            Assert.True(rxFrame.ReadByte(out value));
            Assert.Equal(0x01, value);
            Assert.True(rxFrame.ReadByte(out value));
            Assert.Equal(0x02, value);
            Assert.True(rxFrame.ReadByte(out value));
            Assert.Equal(0x03, value);
        }
        [Fact]
        public void GetBytes_shall_return_bytes_in_order()
        {
            byte[] buffer = new byte[] { 0x01, 0x02, 0x03 };
            var rxFrame = CreateFrameForTest(buffer);
            Assert.True(rxFrame.ReadBytes(out byte[]? values, (uint)buffer.Length));
            Assert.Equal(buffer, values);
        }
        [Fact]
        public async Task GetByteAsync_shall_return_bytes_in_order()
        {
            byte[] buffer = new byte[] { 0x01, 0x02, 0x03 };
            byte value = 0;

            var rxFrame = CreateFrameForTest(buffer);
            Assert.True(await rxFrame.ReadByteAsync(out value, CancellationToken.None));
            Assert.Equal(0x01, value);
            Assert.True(await rxFrame.ReadByteAsync(out value, CancellationToken.None));
            Assert.Equal(0x02, value);
            Assert.True(await rxFrame.ReadByteAsync(out value, CancellationToken.None));
            Assert.Equal(0x03, value);

        }
        [Fact]
        public async Task GetBytesAsync_shall_return_bytes_in_order()
        {
            byte[] buffer = new byte[] { 0x01, 0x02, 0x03 };
            var rxFrame = CreateFrameForTest(buffer);

            Assert.True(await rxFrame.ReadBytesAsync(out byte[]? values, (uint)buffer.Length, CancellationToken.None));

            Assert.Equal(buffer, values);
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
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(evtId, result.ResponseId);
            Assert.IsAssignableFrom(expectedType, result);
        }

        [Fact]
        public void StartReceiving_shall_receive_message_RSP_OK_and_wait_forever_until_timeout_occured()
        {
            var frame = Create1ByteResponse(SlaveResponseEnum.RSP_OK);
            var test = CreateFrameForTest(frame, 100);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired, true);

            Assert.True(timerExpired);
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
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

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
                (byte)JTAGICEmkII.Slave.BreakpointModeEnum.BKPT_MODE_PROGRAM};

            var frame = CreateBytesResponse(SlaveResponseEnum.RSP_GET_BREAK, payload);
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.RSP_GET_BREAK, result.ResponseId);

            Assert.IsAssignableFrom<ResponseBreakpoint>(result);
            ResponseBreakpoint responseBreakpoint = (ResponseBreakpoint)result;
            Assert.Equal(BreakpontTypeEnum.BKPT_PRG_MEMORY, responseBreakpoint.BreakpontType);
            Assert.Equal(JTAGICEmkII.Slave.BreakpointModeEnum.BKPT_MODE_PROGRAM, responseBreakpoint.BreakpointMode);
            Assert.Equal((UInt32)0x000001, responseBreakpoint.Address);
        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseMcuState_shall_time_out()
        {
            var payload = new byte[] {
                (byte)McuStateEnum.RUNNING};

            var frame = CreateBytesResponse(SlaveResponseEnum.RSP_GET_BREAK, payload);
            var test = CreateFrameForTest(frame, 100);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired, true);

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
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

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
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.RSP_PC, result.ResponseId);

            Assert.IsAssignableFrom<ResponseProgramCounter>(result);
            ResponseProgramCounter responseProgranCounter = (ResponseProgramCounter)result;
            Assert.Equal(expectedProgramCounter, responseProgranCounter.ProgramCounter);
        }

        [Fact]
        public void StartReceiving_shall_receive_ResponseSelfTest()
        {
            var payload = new byte[] {
                (byte)SelfTestReponseEnum.SELFTEST_OK,
                (byte)SelfTestReponseEnum.SELFTEST_SKIPPED,
                (byte)SelfTestReponseEnum.SELFTEST_FAILED,
                (byte)SelfTestReponseEnum.SELFTEST_OK,
                (byte)SelfTestReponseEnum.SELFTEST_SKIPPED,
                (byte)SelfTestReponseEnum.SELFTEST_FAILED,
                (byte)SelfTestReponseEnum.SELFTEST_SKIPPED,
                (byte)SelfTestReponseEnum.SELFTEST_FAILED,
            };

            var frame = CreateBytesResponse(SlaveResponseEnum.RSP_SELFTEST, payload);
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.RSP_SELFTEST, result.ResponseId);

            Assert.IsAssignableFrom<ResponseSelfTest>(result);
            ResponseSelfTest responseSelfTest = (ResponseSelfTest)result;


            var expectedResult = new SelfTestReponseEnum[] {
                SelfTestReponseEnum.SELFTEST_OK,
                SelfTestReponseEnum.SELFTEST_SKIPPED,
                SelfTestReponseEnum.SELFTEST_FAILED,
                SelfTestReponseEnum.SELFTEST_OK,
                SelfTestReponseEnum.SELFTEST_SKIPPED,
                SelfTestReponseEnum.SELFTEST_FAILED,
                SelfTestReponseEnum.SELFTEST_SKIPPED,
                SelfTestReponseEnum.SELFTEST_FAILED
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
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

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
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

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
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

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
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_FORCE_BREAK_TIMEOUT, typeof(ResponseEvent))]
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
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT_BAUD, typeof(ResponseEvent))]
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
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

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
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

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
            var test = CreateFrameForTest(frame);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired);

            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.EVT_RUN, result.ResponseId);

            Assert.IsAssignableFrom<ResponseEventRun>(result);
            ResponseEventRun responseEventRun = (ResponseEventRun)result;
            Assert.Equal(payload[0], responseEventRun.RunCause);

        }


        [Theory]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_FORCE_BREAK_TIMEOUT, typeof(ResponseEvent))]
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
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT_BAUD, typeof(ResponseEvent))]
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
        [InlineData(SlaveResponseEnum.RSP_PC, typeof(ResponseProgramCounter))]
        [InlineData(SlaveResponseEnum.RSP_SIGN_ON, typeof(ResponseSignOn))]
        [InlineData(SlaveResponseEnum.EVT_BREAK, typeof(ResponseEventBreak))]
        [InlineData(SlaveResponseEnum.EVT_RUN, typeof(ResponseEventRun))]
        [InlineData(SlaveResponseEnum.EVT_DEBUG, typeof(ResponseEventDebug))]
        public void ResponseFactory_CreateReponses_shall_create_the_right_object(SlaveResponseEnum testId, Type expectedType)
        {
            var result = ResponseFactory.CreateResponse(testId);
            Assert.IsAssignableFrom(expectedType, result);
        }

        [Theory]
        //[InlineData(-1, 0x0000, 0x0000, false)]
        //[InlineData(0x0000, 0x0001, 0x0001, false)]
        //[InlineData(0xFFFE, 0x0000, 0x0000, false)]
        //[InlineData(0xFFFD, 0xFFFE, 0xFFFE, false)]
        //[InlineData(0x0000, 0x0002, 0x0002, false)]
        //[InlineData(0x0005, 0x0004, 0x0005, true)]
        //[InlineData(0x0005, 0x0005, 0x0005, true)]
        [InlineData(0x0005, 0xFFFE, 0x0005, true)]
        public void StartReceiving_shall_manage_sequence_number(int previousSequence, UInt16 frameSequence, UInt16 expectedSequence, bool expectedTimerExpired)
        {
            var frame = Create1ByteResponse(SlaveResponseEnum.RSP_OK, frameSequence);
            int localTimeout;

            if (expectedTimerExpired)
                localTimeout = 1000;
            else
                localTimeout = -1;

            var test = CreateFrameForTest(frame, localTimeout);

            Assert.Equal(-1, test.PreviousSequenceNumber);
//            Assert.Equal(-1, test.PreviousSequenceNumber2.NumberValue);
            test.PreviousSequenceNumber = previousSequence;
            test.PreviousSequenceNumber2 = new SequenceNumber(previousSequence);

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired, expectedTimerExpired);

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
                //Assert.Equal(expectedSequence, test.PreviousSequenceNumber2.NumberValue);
            }
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
        public void StartReceiving_shall_manage_sequence_number2(int previousSequence, UInt16 frameSequence, UInt16 expectedSequence, bool expectedTimerExpired)
        {
            Logger.Debug($"Test StartReceiving_shall_manage_sequence_number2 with previousSequence: {previousSequence}, frameSequence: {frameSequence}, expectedSequence: {expectedSequence}, expectedTimerExpired: {expectedTimerExpired}");
            var frame = Create1ByteResponse(SlaveResponseEnum.RSP_OK, frameSequence);
            int localTimeout;

            if (expectedTimerExpired)
                localTimeout = 1000;
            else
                localTimeout = -1;

            var test = CreateFrameForTest(frame, localTimeout);

            Assert.True(test.PreviousSequenceNumber2.IsInitial);
            test.PreviousSequenceNumber2 = new SequenceNumber(previousSequence);
            test.PreviousSequenceNumber = previousSequence;

            ISlaveResponse? result = this.TestReceiverSlave(test, out bool timerExpired, expectedTimerExpired);

            Assert.Equal(expectedSequence, (UInt16)test.PreviousSequenceNumber2);
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
            }
        }

        protected virtual RxFrame CreateFrameForTest(byte[] buffer, int timeout = -1)
        {
            RxFrameMoq rxFrameMoq = new RxFrameMoq(buffer, timeout);
            RxFrame rxFrame = new RxFrame(rxFrameMoq);
            rxFrameMoq.Attach(rxFrame);

            if (timeout != -1)
                rxFrameMoq.WaitForTimeout = true;
            return rxFrame;
        }
    }
}
