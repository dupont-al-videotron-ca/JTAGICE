using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII;
using JTAGICEmkII.Master;
using JTAGICEmkIITest.Moq;
using MyFramework;
using Xunit;

namespace JTAGICEmkIITest
{
    public class TxFrameTest : FrameTest
    {

        public TxFrameTest() : base()
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
            Assert.Equal(values, ((TxFrameMoq)test.ComAdaptor).Buffer);
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
            Assert.Equal(values, ((TxFrameMoq)test.ComAdaptor).Buffer);
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

        [InlineData(MasterCommandEnum.CMND_WRITE_PC, typeof(CommandProgranCounter))]
        [InlineData(MasterCommandEnum.CMND_RUN_TO_ADDR, typeof(CommandProgranCounter))]

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
            Assert.Equal(1, ((Command)result).MessageLength);
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
            int result = test.BuildAndSendTxFrameCommand(command);

            byte[] buffer = ((TxFrameMoq)test.ComAdaptor).Buffer;
            ValidateTxBuffer(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, command.MessageLength);
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
            var result = test.BuildAndSendTxFrameCommand(command);

            byte[] buffer = ((TxFrameMoq)test.ComAdaptor).Buffer;
            ValidateTxBuffer(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, command.MessageLength);
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
            var result = test.BuildAndSendTxFrameCommand(command);

            byte[] buffer = ((TxFrameMoq)test.ComAdaptor).Buffer;
            ValidateTxBuffer(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, command.MessageLength);
            Assert.Equal(buffer, ((TxFrameMoq)test.ComAdaptor).Buffer);
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
            var result = test.BuildAndSendTxFrameCommand(command);


            byte[] buffer = ((TxFrameMoq)test.ComAdaptor).Buffer;
            ValidateTxBuffer(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, command.MessageLength);
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
            var result = test.BuildAndSendTxFrameCommand(command);


            byte[] buffer = ((TxFrameMoq)test.ComAdaptor).Buffer;
            ValidateTxBuffer(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, command.MessageLength);
            Assert.Equal(MasterCommandEnum.CMND_READ_MEMORY, command.MessageId);

        }

        [Theory]
        [InlineData(MasterCommandEnum.CMND_WRITE_PC)]
        [InlineData(MasterCommandEnum.CMND_RUN_TO_ADDR)]
        public void TxFrame_shall_send_CommandProgranCounter(MasterCommandEnum msgId)
        {
            // Arrange

            var test = CreateFrameForTest();
            CommandProgranCounter command = new CommandProgranCounter(msgId);
            command.ProgrammeCounter = 0x12345678;

            // Act & Assert
            var result = test.BuildAndSendTxFrameCommand(command);

            var buffer = ((TxFrameMoq)test.ComAdaptor).Buffer;
            ValidateTxBuffer(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, command.MessageLength);
            Assert.Equal(msgId, command.MessageId);

        }

        [Fact]
        public void TxFrame_shall_send_CommandSingleStep()
        {
            // Arrange
            MasterCommandEnum msgId = MasterCommandEnum.CMND_SINGLE_STEP;
            var test = CreateFrameForTest();
            CommandSingleStep command = new CommandSingleStep(msgId);
            command.StepMode = StepModeEnum.STEP_INTO;

            // Act & Assert
            var result = test.BuildAndSendTxFrameCommand(command);

            var buffer = ((TxFrameMoq)test.ComAdaptor).Buffer;
            ValidateTxBuffer(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, command.MessageLength);
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
            var result = test.BuildAndSendTxFrameCommand(command);

            var buffer = ((TxFrameMoq)test.ComAdaptor).Buffer;
            ValidateTxBuffer(buffer);

            Assert.Equal(buffer.Length, result);
            Assert.Equal(buffer.Length - TxFrame.FrameSizeOverhead, command.MessageLength);
            Assert.Equal(msgId, command.MessageId);

        }

        private TxFrame CreateFrameForTest()
        {
            TxFrameMoq TxFrameMoq = new TxFrameMoq();
            TxFrame TxFrame = new TxFrame(TxFrameMoq);
            return TxFrame;
        }

    }
}
