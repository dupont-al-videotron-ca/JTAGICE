using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using MyFramework;
using Xunit;

namespace JTAGICEmkIITest
{
    public abstract class FrameTest : XUnitTestBase
    {
        private const byte ESC = 27;
        private const byte TOKEN = 14;
        private const byte SequenceNumberLsb = 0x01;
        private const byte SequenceNumberMsb = 0x00;

        public byte[] Create1ByteResponse(SlaveResponseEnum responseId)
        {
            List<byte> message = new List<byte>();
            // Start of message
            message.Add(ESC);
            // Sequence number
            message.Add(SequenceNumberLsb);
            message.Add(SequenceNumberMsb);
            // message size
            message.Add(0x01);
            message.Add(0x00);
            message.Add(0x00);
            message.Add(0x00);
            // token
            message.Add(TOKEN);
            // payload
            message.Add((byte)responseId);

            ushort crc = Crc16.ComputeCrc(message.ToArray());

            // crc
            message.Add((byte)(crc & 0xFF)); // payload length LSB
            message.Add((byte)((crc >> 8) & 0xFF)); // payload length Msb

            return message.ToArray();

        }
    }

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
            Assert.Equal(buffer, rxFrame.GetBytes((uint) buffer.Length));
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

        [Fact]
        public void StartReceiving_shall_receive_message_RSP_OK()
        {
            var frame = Create1ByteResponse(SlaveResponseEnum.RSP_OK);
            var test = new Moq.RxFrameMoq(frame);
            bool timerExpired = false;
            ISlaveResponse? result = null;

            test.RxTimerExpired += (s, e) => timerExpired = true;
            test.MessageReceived += (s, e) =>
                {
                    result = e.Response;
                };

            test.StartReceiving();
            while (!timerExpired && result == null)
            {
                // Wait for the timer to expire or the message to be received
                Thread.Sleep(1000);
            }
            test.StopReceiving();
            Assert.False(timerExpired);
            Assert.NotNull(result);
            Assert.Equal(SlaveResponseEnum.RSP_OK, result.ResponseId);

        }
    }
}
