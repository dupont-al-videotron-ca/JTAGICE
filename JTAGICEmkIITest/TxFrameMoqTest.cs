using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using JTAGICEmkIITest.Moq;
using MyFramework;
using MyFramework.Queue;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JTAGICEmkIITest
{
    public class TxFrameMoqTest : FrameTest
    {

        public TxFrameMoqTest() : base()
        {
        }


        [Fact]
        public void SendBytes_ShouldAddBytesToBuffer()
        {
            byte[] values = new byte[] { 0x01, 0x02, 0x03 };

            // Arrange
            var test = CreateFrameForTest();

            // Act
            var result = test.SendBytes(values);

            // Assert
            Assert.Equal(values.Length, result);
            Assert.Equal(values, ((ITxFrameAdaptor)test.Adaptor).Buffer);
        }



        [Fact]
        public async Task SendBytesAsync_ShouldAddBytesToBuffer()
        {
            byte[] values = new byte[] { 0x01, 0x02, 0x03 };

            // Arrange
            var test = CreateFrameForTest();

            // Act
            var result = await test.SendBytesAsync(values, CancellationToken.None);

            // Assert
            Assert.Equal(values.Length, result);
            Assert.Equal(values, ((ITxFrameAdaptor)test.Adaptor).Buffer);
        }

        [Fact]
        public void TxFrame_shall_send_Responce()
        {
            // Arrange
            var test = CreateFrameForTest();
            Response response = (Response)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK)!;
            var expectedResponse = (Response)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_OK)!;
            expectedResponse.MessageLength = 1;

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);

            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
        }

        [Fact]
        public void TxFrame_shall_send_ResponseBreakpoint()
        {
            // Arrange
            var test = CreateFrameForTest();
            var response = (ResponseBreakpoint)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_GET_BREAK)!;
            var expectedResponse = (ResponseBreakpoint)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_GET_BREAK)!;
            expectedResponse.MessageLength = 7;

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());
            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
            Assert.Equal(expectedResponse.BreakpontType, response.BreakpontType);
            Assert.Equal(expectedResponse.BreakpointMode, response.BreakpointMode);
            Assert.Equal(expectedResponse.Address, response.Address);
            Assert.Equal(expectedResponse.IsEvent, response.IsEvent);
        }

        [Fact]
        public void TxFrame_shall_send_ResponseEmulatorMode()
        {
            // Arrange
            var test = CreateFrameForTest();
            var response = (ResponseEmulatorMode)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_ILLEGAL_EMULATOR_MODE)!;
            var expectedResponse = (ResponseEmulatorMode)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_ILLEGAL_EMULATOR_MODE)!;
            expectedResponse.MessageLength = 2;

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());
            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
            Assert.Equal(expectedResponse.EmulatorMode, response.EmulatorMode);
            Assert.Equal(expectedResponse.IsEvent, response.IsEvent);
        }

        [Fact]
        public void TxFrame_shall_send_ResponseEvent()
        {
            // Arrange
            var test = CreateFrameForTest();
            var response = (ResponseEvent)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.EVT_TARGET_POWER_ON)!;
            var expectedResponse = (ResponseEvent)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.EVT_TARGET_POWER_ON)!;
            expectedResponse.MessageLength = 1;

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());
            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
            Assert.True(response.IsEvent);
            Assert.Equal(expectedResponse.IsEvent, response.IsEvent);
        }

        [Fact]
        public void TxFrame_shall_send_ResponseEventBreak()
        {
            // Arrange
            var test = CreateFrameForTest();
            var response = (ResponseEventBreak)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.EVT_BREAK)!;
            var expectedResponse = (ResponseEventBreak)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.EVT_BREAK)!;
            expectedResponse.MessageLength = (uint)expectedResponse.Size;

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());
            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
            Assert.Equal(expectedResponse.ProgramCounter, response.ProgramCounter);
            Assert.Equal(expectedResponse.BreakCause, response.BreakCause);
            Assert.True(response.IsEvent);
            Assert.Equal(expectedResponse.IsEvent, response.IsEvent);
        }

        [Fact]
        public void TxFrame_shall_send_ResponseEventDebug()
        {
            // Arrange
            var test = CreateFrameForTest();
            var response = (ResponseEventDebug)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.EVT_DEBUG)!;
            var expectedResponse = (ResponseEventDebug)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.EVT_DEBUG)!;
            expectedResponse.MessageLength = (uint)expectedResponse.Size;

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());
            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
            Assert.Equal(expectedResponse.CommState, response.CommState);
            Assert.Equal(expectedResponse.CommError, response.CommError);
            Assert.Equal(expectedResponse.EventId, response.EventId);
            Assert.True(response.IsEvent);
            Assert.Equal(expectedResponse.IsEvent, response.IsEvent);
        }

        [Fact]
        public void TxFrame_shall_send_ResponseEventRun()
        {
            // Arrange
            var test = CreateFrameForTest();
            var response = (ResponseEventRun)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.EVT_RUN)!;
            var expectedResponse = (ResponseEventRun)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.EVT_RUN)!;
            expectedResponse.MessageLength = (uint)expectedResponse.Size;

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());
            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
            Assert.Equal(expectedResponse.RunCause, response.RunCause);
            Assert.True(response.IsEvent);
            Assert.Equal(expectedResponse.IsEvent, response.IsEvent);
        }

        [Fact]
        public void TxFrame_shall_send_ResponseMcuState()
        {
            // Arrange
            var test = CreateFrameForTest();
            var response = (ResponseMcuState)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_ILLEGAL_MCU_STATE)!;
            var expectedResponse = (ResponseMcuState)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_ILLEGAL_MCU_STATE)!;
            expectedResponse.MessageLength = (uint)expectedResponse.Size;

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());
            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
            Assert.Equal(expectedResponse.State, response.State);
            Assert.False(response.IsEvent);
            Assert.Equal(expectedResponse.IsEvent, response.IsEvent);
        }

        [Fact]
        public void TxFrame_shall_send_ResponseMultipleByte()
        {
            byte[] memory = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };

            // Arrange
            var test = CreateFrameForTest();
            var response = (ResponseMultipleByte)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_MEMORY)!;
            response.Data.AddRange(memory);

            var expectedResponse = (ResponseMultipleByte)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_MEMORY)!;
            expectedResponse.Data.AddRange(memory);
            expectedResponse.MessageLength = (uint)(memory.Length + 1);

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());
            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
            Assert.Equal(memory, response.Data.ToArray());
            Assert.False(response.IsEvent);
            Assert.Equal(expectedResponse.IsEvent, response.IsEvent);
        }

        [Fact]
        public void TxFrame_shall_send_ResponseProgranCounter()
        {

            // Arrange
            var test = CreateFrameForTest();
            var response = (ResponseProgramCounter)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_PC)!;

            var expectedResponse = (ResponseProgramCounter)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_PC)!;
            expectedResponse.MessageLength = (uint)expectedResponse.Size;

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());
            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
            Assert.Equal(expectedResponse.ProgramCounter, response.ProgramCounter);
            Assert.False(response.IsEvent);
            Assert.Equal(expectedResponse.IsEvent, response.IsEvent);
        }

        [Fact]
        public void TxFrame_shall_send_ResponseSelfTest()
        {
            byte[] payload = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

            // Arrange
            var test = CreateFrameForTest();
            var response = (ResponseSelfTest)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_SELFTEST)!;
            response.Data.AddRange(payload);
            var expectedResponse = (ResponseSelfTest)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_SELFTEST)!;
            expectedResponse.MessageLength = (uint)(expectedResponse.Size);

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());
            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
            Assert.Equal(payload, response.Data.ToArray());
            Assert.False(response.IsEvent);
            Assert.Equal(expectedResponse.IsEvent, response.IsEvent);

            for (int i = 0; i < payload.Length; i++)
            {
                Assert.Equal(expectedResponse.GetSelfTestResult(i), response.GetSelfTestResult(i));
            }
            
            Assert.Equal(expectedResponse.SelfTestResults, response.SelfTestResults);

        }

        [Fact]
        public void TxFrame_shall_send_ResponseSignOn()
        {
            // Arrange
            var test = CreateFrameForTest();
            var response = (ResponseSignOn)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_SIGN_ON)!;
            response.SlaveMcuBootLoaderVersion = 0x01;
            response.SlaveMcuFirmwareVersionMajor = 0x02;
            response.SlaveMcuFirmwareVersionMinor = 0x01;
            response.SlaveMcuHwVersion = 0x03;
            response.MasterMcuFirmwareVersionMajor = 0x10;
            response.MasterMcuFirmwareVersionMinor = 0x02;
            response.MasterMcuHwVersion = 0x20;
            response.MasterMcuBootLoaderVersion = 0x40;
            response.CommunicationProtocolVersion = 0x12;
            response.SerialNumber = new byte[ResponseSignOn.SerialNumberSize] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
            response.DeviceId = new byte[] {(byte)'a', (byte)'b', (byte)'c', 0x00};

            var expectedResponse = (ResponseSignOn)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_SIGN_ON)!;
            expectedResponse.MessageLength = (uint)(expectedResponse.Size+3);
            expectedResponse.SlaveMcuBootLoaderVersion = 0x01;
            expectedResponse.SlaveMcuFirmwareVersionMajor = 0x02;
            expectedResponse.SlaveMcuFirmwareVersionMinor = 0x01;
            expectedResponse.SlaveMcuHwVersion = 0x03;
            expectedResponse.MasterMcuFirmwareVersionMajor = 0x10;
            expectedResponse.MasterMcuFirmwareVersionMinor = 0x02;
            expectedResponse.MasterMcuHwVersion = 0x20;
            expectedResponse.MasterMcuBootLoaderVersion = 0x40;
            expectedResponse.CommunicationProtocolVersion = 0x12;
            expectedResponse.SerialNumber = new byte[ResponseSignOn.SerialNumberSize] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
            expectedResponse.DeviceId = new byte[] { (byte)'a', (byte)'b', (byte)'c', 0x00 };

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(response);

            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            byte[] buffer = test.Adaptor.Buffer;
            ValidateCRC(buffer);
            Assert.Equal(buffer.Length, result);

            expectedResponse.ReadFromBytes(buffer.AsSpan(8, buffer.Length - 10).ToArray());
            Assert.Equal(expectedResponse.MessageLength, response.MessageLength);
            Assert.Equal(expectedResponse.Size, response.Size);
            Assert.Equal(expectedResponse.ResponseId, response.ResponseId);
            Assert.Equal(expectedResponse.SlaveMcuBootLoaderVersion, response.SlaveMcuBootLoaderVersion);
            Assert.Equal(expectedResponse.SlaveMcuFirmwareVersionMajor, response.SlaveMcuFirmwareVersionMajor);
            Assert.Equal(expectedResponse.SlaveMcuFirmwareVersionMinor, response.SlaveMcuFirmwareVersionMinor);
            Assert.Equal(expectedResponse.SlaveMcuHwVersion, response.SlaveMcuHwVersion);
            Assert.Equal(expectedResponse.MasterMcuFirmwareVersionMajor, response.MasterMcuFirmwareVersionMajor);
            Assert.Equal(expectedResponse.MasterMcuFirmwareVersionMinor, response.MasterMcuFirmwareVersionMinor);
            Assert.Equal(expectedResponse.MasterMcuHwVersion, response.MasterMcuHwVersion);
            Assert.Equal(expectedResponse.MasterMcuBootLoaderVersion, response.MasterMcuBootLoaderVersion);
            Assert.Equal(expectedResponse.CommunicationProtocolVersion, response.CommunicationProtocolVersion);
            Assert.Equal(expectedResponse.SerialNumber, response.SerialNumber);
            Assert.Equal(expectedResponse.DeviceId, response.DeviceId);
            Assert.Equal("654321", response.SerialNumberString);
            Assert.Equal("abc", response.DeviceIdString);
            Assert.False(response.IsEvent);
            Assert.Equal(expectedResponse.IsEvent, response.IsEvent);

        }

        [Fact]
        public void TxFrame_shall_BuildAndSendFrameCommand()
        {
            List<byte> expectedFrame = new List<byte>();
            // Start of frame
            expectedFrame.Add(27);
            // Sequence number
            expectedFrame.Add((byte)(0 & 0xFF));
            expectedFrame.Add((byte)((0 >> 8) & 0xFF));
            // frame size
            var messageSize = (uint)1;
            expectedFrame.Add((byte)(messageSize & 0xFF));
            expectedFrame.Add((byte)((messageSize >> 8) & 0xFF));
            expectedFrame.Add((byte)((messageSize >> 16) & 0xFF));
            expectedFrame.Add((byte)((messageSize >> 24) & 0xFF));
            // token
            expectedFrame.Add(14);

            expectedFrame.Add((byte)MasterCommandEnum.CMND_SIGN_OFF);

            ushort crc = Crc16.ComputeCrc(expectedFrame.ToArray());

            // crc
            expectedFrame.Add((byte)(crc & 0xFF)); // payload length LSB
            expectedFrame.Add((byte)((crc >> 8) & 0xFF)); // payload length Msb

            // Arrange
            var test = CreateFrameForTest();
            Command command = new Command(MasterCommandEnum.CMND_SIGN_OFF);

            // Act & Assert
            int result = test.BuildAndSendFrameCommand(command);

            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(expectedFrame.ToArray(), buffer);

        }

        [Fact]
        public void TxFrame_shall_BuildAndSendFrameResponse()
        {
            List<byte> expectedFrame = new List<byte>();
            // Start of frame
            expectedFrame.Add(27);
            // Sequence number
            expectedFrame.Add((byte)(0 & 0xFF));
            expectedFrame.Add((byte)((0 >> 8) & 0xFF));
            // frame size
            var messageSize = (uint)1;
            expectedFrame.Add((byte)(messageSize & 0xFF));
            expectedFrame.Add((byte)((messageSize >> 8) & 0xFF));
            expectedFrame.Add((byte)((messageSize >> 16) & 0xFF));
            expectedFrame.Add((byte)((messageSize >> 24) & 0xFF));
            // token
            expectedFrame.Add(14);

            expectedFrame.Add((byte)SlaveResponseEnum.RSP_ILLEGAL_COMMAND);

            ushort crc = Crc16.ComputeCrc(expectedFrame.ToArray());

            // crc
            expectedFrame.Add((byte)(crc & 0xFF)); // payload length LSB
            expectedFrame.Add((byte)((crc >> 8) & 0xFF)); // payload length Msb

            // Arrange
            var test = CreateFrameForTest();
            Response command = (Response)JTAGICEmkII.Slave.ResponseFactory.CreateResponse(SlaveResponseEnum.RSP_ILLEGAL_COMMAND)!;

            // Act & Assert
            int result = test.BuildAndSendFrameResponse(command);

            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);
            Assert.Equal(expectedFrame.ToArray(), buffer);
        }

        [Theory]
        [InlineData(MasterCommandEnum.CMND_SIGN_OFF, typeof(Command))]
        [InlineData(MasterCommandEnum.CMND_GET_SIGN_ON, typeof(Command))]
        [InlineData(MasterCommandEnum.CMND_READ_PC, typeof(Command))]
        [InlineData(MasterCommandEnum.CMND_GO, typeof(Command))]
        [InlineData(MasterCommandEnum.CMND_GET_SYNC, typeof(Command))]
        [InlineData(MasterCommandEnum.CMND_CHIP_ERASE, typeof(Command))]
        [InlineData(MasterCommandEnum.CMND_ENTER_PROGMODE, typeof(Command))]
        [InlineData(MasterCommandEnum.CMND_LEAVE_PROGMODE, typeof(Command))]
        [InlineData(MasterCommandEnum.CMND_CLEAR_EVENTS, typeof(Command))]
        [InlineData(MasterCommandEnum.CMND_RESTORE_TARGET, typeof(Command))]

        [InlineData(MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR, typeof(CommandMultipleByte))]
        [InlineData(MasterCommandEnum.CMND_SELFTEST, typeof(CommandMultipleByte))]
        [InlineData(MasterCommandEnum.CMND_SPI_CMD, typeof(CommandMultipleByte))]

        [InlineData(MasterCommandEnum.CMND_SET_PARAMETER, typeof(CommandParameter))]
        [InlineData(MasterCommandEnum.CMND_GET_PARAMETER, typeof(CommandParameter))]

        [InlineData(MasterCommandEnum.CMND_WRITE_MEMORY, typeof(CommandMemory))]
        [InlineData(MasterCommandEnum.CMND_READ_MEMORY, typeof(CommandMemory))]

        [InlineData(MasterCommandEnum.CMND_WRITE_PC, typeof(CommandProgramCounter))]
        [InlineData(MasterCommandEnum.CMND_RUN_TO_ADDR, typeof(CommandProgramCounter))]

        [InlineData(MasterCommandEnum.CMND_SINGLE_STEP, typeof(CommandSingleStep))]
        [InlineData(MasterCommandEnum.CMND_FORCED_STOP, typeof(CommandPCMode))]
        [InlineData(MasterCommandEnum.CMND_RESET, typeof(CommandPCMode))]


        [InlineData(MasterCommandEnum.CMND_ERASEPAGE_SPM, typeof(CommandAddress))]
        [InlineData(MasterCommandEnum.CMND_GET_BREAK, typeof(CommandBreakNumber))]
        [InlineData(MasterCommandEnum.CMND_SET_BREAK, typeof(CommandBreakpoint))]
        [InlineData(MasterCommandEnum.CMND_CLR_BREAK, typeof(CommandBreakAddress))]
        [InlineData(MasterCommandEnum.CMND_SET_N_PARAMETERS, typeof(CommandNParameter))]

        public void CommandFactory_shall_create_the_right_frame(MasterCommandEnum msgId, Type expectedType)
        {
            // Arrange

            // Act & Assert
            IMasterCommand? result = CommandFactory.CreateCommand(msgId);

            Assert.NotNull(result);
            Assert.IsAssignableFrom(expectedType, result);
            Assert.Equal(msgId, result.MessageId);
        }

        [Theory]
        [InlineData(MasterCommandEnum.CMND_SIGN_OFF)]
        [InlineData(MasterCommandEnum.CMND_GET_SIGN_ON)]
        [InlineData(MasterCommandEnum.CMND_READ_PC)]
        [InlineData(MasterCommandEnum.CMND_GO)]
        [InlineData(MasterCommandEnum.CMND_GET_SYNC)]
        [InlineData(MasterCommandEnum.CMND_CHIP_ERASE)]
        [InlineData(MasterCommandEnum.CMND_ENTER_PROGMODE)]
        [InlineData(MasterCommandEnum.CMND_LEAVE_PROGMODE)]
        [InlineData(MasterCommandEnum.CMND_CLEAR_EVENTS)]
        [InlineData(MasterCommandEnum.CMND_RESTORE_TARGET)]
        public void TxFrame_shall_send_Command(MasterCommandEnum msgId)
        {
            // Arrange
            var test = CreateFrameForTest();
            Command command = new Command(msgId);

            // Act & Assert
            int result = test.BuildAndSendFrameCommand(command);

            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, (int)command.MessageLength);
            Assert.Equal(msgId, command.MessageId);

        }

        [Theory]
        [InlineData(MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR)]
        [InlineData(MasterCommandEnum.CMND_SELFTEST)]
        [InlineData(MasterCommandEnum.CMND_SPI_CMD)]
        public void TxFrame_shall_send_CommandParameter(MasterCommandEnum msgId)
        {
            // Arrange
            var bytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            var test = CreateFrameForTest();
            CommandParameter command = new CommandParameter(msgId);
            command.ParameterId = ParameterEnum.PARAM_BAUD_RATE;
            command.Data.AddRange(bytes);

            // Act & Assert
            var result = test.BuildAndSendFrameCommand(command);

            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, (int)command.MessageLength);
            Assert.Equal(msgId, command.MessageId);

        }

        [Theory]
        [InlineData(MasterCommandEnum.CMND_SET_PARAMETER)]
        [InlineData(MasterCommandEnum.CMND_GET_PARAMETER)]
        public void TxFrame_shall_send_CommandMultipleByte(MasterCommandEnum msgId)
        {
            // Arrange
            var bytes = new byte[] { 0x01, 0x02, 0x03 };

            var test = CreateFrameForTest();
            CommandMultipleByte command = new CommandMultipleByte(msgId);
            command.Data.AddRange(bytes);

            // Act & Assert
            var result = test.BuildAndSendFrameCommand(command);

            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, (int)command.MessageLength);
            Assert.Equal(buffer, ((ITxFrameAdaptor)test.Adaptor).Buffer);
            Assert.Equal(msgId, command.MessageId);

        }

        [Fact]
        public void TxFrame_shall_send_CommandMemory_Write()
        {
            // Arrange
            var bytes = new byte[] { 0x01, 0x02, 0x03 };

            var test = CreateFrameForTest();
            CommandMemory command = new CommandMemory(MasterCommandEnum.CMND_WRITE_MEMORY);
            command.MemoryType = MemoryTypeEnum.MT_SRAM;
            command.Address = 0x12345678;
            command.ByteCount = 0x03;
            command.Data.AddRange(bytes);

            // Act & Assert
            var result = test.BuildAndSendFrameCommand(command);


            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, (int)command.MessageLength);
            Assert.Equal(MasterCommandEnum.CMND_WRITE_MEMORY, command.MessageId);

        }

        [Fact]
        public void TxFrame_shall_send_CommandMemory_Read()
        {
            // Arrange
            var bytes = new byte[] { };

            var test = CreateFrameForTest();
            CommandMemory command = new CommandMemory(MasterCommandEnum.CMND_READ_MEMORY);
            command.MemoryType = MemoryTypeEnum.MT_SRAM;
            command.Address = 0x12345678;
            command.ByteCount = 0x03;
            command.Data.AddRange(bytes);

            // Act & Assert
            var result = test.BuildAndSendFrameCommand(command);


            byte[] buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, (int)command.MessageLength);
            Assert.Equal(MasterCommandEnum.CMND_READ_MEMORY, command.MessageId);

        }

        [Theory]
        [InlineData(MasterCommandEnum.CMND_WRITE_PC)]
        [InlineData(MasterCommandEnum.CMND_RUN_TO_ADDR)]
        public void TxFrame_shall_send_CommandProgranCounter(MasterCommandEnum msgId)
        {
            // Arrange

            var test = CreateFrameForTest();
            CommandProgramCounter command = new CommandProgramCounter(msgId);
            command.ProgramCounter = 0x12345678;

            // Act & Assert
            var result = test.BuildAndSendFrameCommand(command);

            var buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, (int)command.MessageLength);
            Assert.Equal(msgId, command.MessageId);

        }

        [Fact]
        public void TxFrame_shall_send_CommandSingleStep()
        {
            // Arrange
            MasterCommandEnum msgId = MasterCommandEnum.CMND_SINGLE_STEP;
            var test = CreateFrameForTest();
            CommandSingleStep command = new CommandSingleStep(msgId);
            command.StepMode = StepModeEnum.Into;

            // Act & Assert
            var result = test.BuildAndSendFrameCommand(command);

            var buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, (int)command.MessageLength);
            Assert.Equal(msgId, command.MessageId);

        }

        [Theory]
        [InlineData(MasterCommandEnum.CMND_FORCED_STOP)]
        [InlineData(MasterCommandEnum.CMND_RESET)]
        public void TxFrame_shall_send_CommandPCMode(MasterCommandEnum msgId)
        {
            // Arrange
            var test = CreateFrameForTest();
            CommandPCMode command = new CommandPCMode(msgId);
            command.ExecutionMode = ExceutionModeEnum.EXMODE_HIGH_LEVEL;

            // Act & Assert
            var result = test.BuildAndSendFrameCommand(command);

            var buffer = ((ITxFrameAdaptor)test.Adaptor).Buffer;
            ValidateCRC(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, (int)command.MessageLength);
            Assert.Equal(msgId, command.MessageId);

        }

        protected virtual TxFrame CreateFrameForTest()
        {
            TxFrameMoq txFrameMoq = new TxFrameMoq();
            TxFrame txFrame = new TxFrame(txFrameMoq);
            return txFrame;
        }

    }
}
