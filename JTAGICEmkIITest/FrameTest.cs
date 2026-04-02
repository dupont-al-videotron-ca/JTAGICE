using Common.Test.Xunit;
using JTAGICEmkII;
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

        internal byte[] Create1ByteResponse(SlaveResponseEnum responseId, UInt16 sequenceNumber = 0x0001)
        {
            List<byte> message = new List<byte>();
            // Start of message
            message.Add(ESC);
            // Sequence number
            message.Add((byte)(sequenceNumber & 0xFF));
            message.Add((byte)((sequenceNumber >> 8) & 0xFF));
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
            Logger.Info($"Created frame: responseId: {responseId}, crc: 0x{crc:x4}");
            return message.ToArray();

        }

        internal byte[] CreateBytesResponse(SlaveResponseEnum responseId, byte[] payload)
        {
            List<byte> message = new List<byte>();
            // Start of message
            message.Add(ESC);
            // Sequence number
            message.Add(SequenceNumberLsb);
            message.Add(SequenceNumberMsb);
            // message size
            var messageSize = (uint)payload.Length + 1;
            message.Add((byte)(messageSize & 0xFF));
            message.Add((byte)((messageSize >> 8) & 0xFF));
            message.Add((byte)((messageSize >> 16) & 0xFF));
            message.Add((byte)((messageSize >> 24) & 0xFF));
            // token
            message.Add(TOKEN);
            // payload
            message.Add((byte)responseId);
            message.AddRange(payload);

            ushort crc = Crc16.ComputeCrc(message.ToArray());

            // crc
            message.Add((byte)(crc & 0xFF)); // payload length LSB
            message.Add((byte)((crc >> 8) & 0xFF)); // payload length Msb

            Logger.Info($"Created frame: responseId: {responseId}, crc: 0x{crc:x4}, messageSize: {messageSize}.");

            return message.ToArray();

        }

        internal byte[] CreateEventResponse(SlaveResponseEnum responseId, byte[] payload)
        {
            List<byte> message = new List<byte>();
            // Start of message
            message.Add(ESC);
            // Sequence number for event is always 0xFFFF
            message.Add(0xff);
            message.Add(0xff);
            // message size
            var messageSize = (uint)payload.Length + 1;
            message.Add((byte)(messageSize & 0xFF));
            message.Add((byte)((messageSize >> 8) & 0xFF));
            message.Add((byte)((messageSize >> 16) & 0xFF));
            message.Add((byte)((messageSize >> 24) & 0xFF));
            // token
            message.Add(TOKEN);
            // payload
            message.Add((byte)responseId);
            message.AddRange(payload);

            ushort crc = Crc16.ComputeCrc(message.ToArray());

            // crc
            message.Add((byte)(crc & 0xFF)); // payload length LSB
            message.Add((byte)((crc >> 8) & 0xFF)); // payload length Msb

            Logger.Info($"Created frame: responseId: {responseId}, crc: 0x{crc:x4}, messageSize: {messageSize}.");

            return message.ToArray();

        }

        internal ISlaveResponse TestReceiver(Moq.RxFrameMoq test, out bool timerExpired, int sleepTimeout = 1)
        {
            bool localTimerExpired = false;
            ISlaveResponse? result = null;

            test.RxTimerExpired += (s, e) =>
            {
                Logger.Info($"Receive timeout.");
                localTimerExpired = true;
            };

            test.MessageReceived += (s, e) =>
            {
                Logger.Info($"Message received: responseId: {e.Response.ResponseId}.");
                result = e.Response;
            };

            test.StartReceiving();
            while (!localTimerExpired && result == null)
            {
                // Wait for the timer to expire or the message to be received
                Thread.Sleep(sleepTimeout);
            }
            test.StopReceiving();

            timerExpired = localTimerExpired;
            Logger.Info($"Received completed.");
            return result!;

        }

        internal void ValidateTxBuffer(byte[] buffer)
        {
            var crc = Crc16.ComputeCrc(buffer.Take(buffer.Length - 2).ToArray());
            Assert.Equal((byte)(crc & 0xFF), buffer[buffer.Length - 2]);
            Assert.Equal((byte)((crc >> 8) & 0xFF), buffer[buffer.Length - 1]);
        }

    }
}
