using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Repository.Hierarchy;
using log4net;
using MyFramework;

namespace JTAGICEmkII
{
    internal abstract class TxFrame : ITxFrame
    {

        #region Constructors 
        public TxFrame()
        {
            sequenceNumber = 0;
            Logger = LogManager.GetLogger(this.GetType());
        }

        #endregion


        #region Fields 
        public static readonly int FrameSizeOverhead = 10;

        private const byte ESC = 27;
        private const byte TOKEN = 14;
        private UInt16 sequenceNumber;
        private const UInt16 SequenceNumberWrap = 0xFFFF;
        private const UInt16 SequenceNumberEvent = 0xFFFF;

        #endregion


        #region Properties 

        protected ILog Logger { get; private set; } 
        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        internal int BuildAndSendTxFrameCommand(Command command)
        {
            var frame = BuildTxFrame(command);
            return this.SendBytes(frame.ToArray());
        }

        internal async Task<int> BuildAndSendTxFrameCommandAsync(Command command, CancellationToken cancellationToken)
        {
            List<byte> frame = BuildTxFrame(command);

            var bytesSent = await this.SendBytesAsync(frame.ToArray(), cancellationToken);
            return bytesSent;
        }


        #endregion


        #region Protected Methods 

        protected internal abstract int SendBytes(byte[] values);

        protected internal abstract Task<int> SendBytesAsync(byte[] values, CancellationToken cancellationToken);

        #endregion

        #region Private Methods 

        private void IncrementNextSequenceNumber()
        {
            sequenceNumber = (UInt16)((sequenceNumber + 1) % SequenceNumberWrap);
        }

        private List<Byte> BuildTxFrame(Command command)
        {
            List<byte> message = new List<byte>();
            // Start of frame
            message.Add(ESC);
            // Sequence number
            message.Add((byte)(sequenceNumber & 0xFF));
            message.Add((byte)((sequenceNumber >> 8) & 0xFF));
            // frame size
            var messageSize = (uint)command.MessageLength;
            message.Add((byte)(messageSize & 0xFF));
            message.Add((byte)((messageSize >> 8) & 0xFF));
            message.Add((byte)((messageSize >> 16) & 0xFF));
            message.Add((byte)((messageSize >> 24) & 0xFF));
            // token
            message.Add(TOKEN);
            var payload = command.WriteToBytes();
            message.AddRange(payload);

            ushort crc = Crc16.ComputeCrc(message.ToArray());

            // crc
            message.Add((byte)(crc & 0xFF)); // payload length LSB
            message.Add((byte)((crc >> 8) & 0xFF)); // payload length Msb

            Logger.Debug($"Tx Frame: commandId: {command.MessageId}, sequenceNumber:{sequenceNumber}, crc: 0x{crc:x4}, messageSize: {command.MessageLength}.");
            IncrementNextSequenceNumber();

            return message;
        }

        #endregion

        #region Private Classes / Enum 

        #endregion


    }
}
