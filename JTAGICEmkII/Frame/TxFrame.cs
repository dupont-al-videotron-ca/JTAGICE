using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Repository.Hierarchy;
using log4net;
using MyFramework;
using JTAGICEmkII.Master;
using JTAGICEmkII.Frame;

namespace JTAGICEmkII
{
    public class TxFrame : ITxFrame, IDisposable
    {

        #region Constructors 
        public TxFrame(ITxFrameAdaptor txComAdaptor)
        {
            sequenceNumber = new SequenceNumber(0);
            Logger = LogManager.GetLogger(this.GetType());
            this.txFrameAdaptor = txComAdaptor ?? throw new ArgumentNullException(nameof(txComAdaptor));
        }

        #endregion


        #region Fields 
        public static readonly int FrameSizeOverhead = 10;

        private const byte ESC = 27;
        private const byte TOKEN = 14;
        private SequenceNumber sequenceNumber;
        private const UInt16 SequenceNumberEvent = 0xFFFF;

        #endregion


        #region Properties 

        public ITxFrameAdaptor Adaptor => txFrameAdaptor;

        protected ILog Logger { get; private set; }

        private ITxFrameAdaptor txFrameAdaptor;
        private bool disposedValue;
        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public int BuildAndSendFrameCommand(Command command)
        {
            ArgumentNullException.ThrowIfNull(command);
            var frame = BuildTxFrame(command);
            return this.SendBytes(frame.ToArray());
        }

        public int BuildAndSendFrameResponse(Slave.Response response, bool isEvent = false)
        {
            ArgumentNullException.ThrowIfNull(response);
            var frame = BuildRxFrame(response, isEvent);
            return this.SendBytes(frame.ToArray());
        }

        public async Task<int> BuildAndSendFrameCommandAsync(Command command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);
            List<byte> frame = BuildTxFrame(command);

            var bytesSent = await this.SendBytesAsync(frame.ToArray(), cancellationToken);
            return bytesSent;
        }
        public async Task<int> BuildAndSendFrameResponseAsync(Slave.Response response, CancellationToken cancellationToken, bool isEvent = false)
        {
            ArgumentNullException.ThrowIfNull(response);
            List<byte> frame = BuildRxFrame(response, isEvent);

            var bytesSent = await this.SendBytesAsync(frame.ToArray(), cancellationToken);
            return bytesSent;
        }


        #endregion


        #region Protected Methods 

        protected internal int SendBytes(byte[] values)
            => txFrameAdaptor.SendBytes(values);

        protected internal Task<int> SendBytesAsync(byte[] values, CancellationToken cancellationToken)
            => txFrameAdaptor.SendBytesAsync(values, cancellationToken);

        #endregion

        #region Private Methods 

        private List<Byte> BuildTxFrame(Command command)
        {
            var payload = command.WriteToBytes();
            List<byte> message = new List<byte>();
            // Start of frame
            message.Add(ESC);
            // Sequence number
            message.AddRange(sequenceNumber.GetUInt16LittleEndian());
            // frame size
            var messageSize = (uint)command.MessageLength;
            message.Add((byte)(messageSize & 0xFF));
            message.Add((byte)((messageSize >> 8) & 0xFF));
            message.Add((byte)((messageSize >> 16) & 0xFF));
            message.Add((byte)((messageSize >> 24) & 0xFF));
            // _token
            message.Add(TOKEN);
            message.AddRange(payload);

            ushort crc = Crc16.ComputeCrc(message.ToArray());

            // crc
            message.Add((byte)(crc & 0xFF)); // payload length LSB
            message.Add((byte)((crc >> 8) & 0xFF)); // payload length Msb

            Logger.Debug($"Tx Frame: commandId: {command.MessageId}, sequenceNumber:{sequenceNumber.NumberValue}, crc: 0x{crc:X4}, messageSize: {command.MessageLength}.");
            sequenceNumber++;

            return message;
        }

        private List<Byte> BuildRxFrame(Slave.Response response, bool isEvent)
        {
            var payload = response.WriteToBytes();
            List<byte> message = new List<byte>();
            // Start of frame
            message.Add(ESC);
            // Sequence number
            if (!isEvent)
            {
                message.AddRange(sequenceNumber.GetUInt16LittleEndian());
            }
            else
            {
                // For events, use the special event sequence number
                message.Add((byte)(SequenceNumberEvent & 0xFF));
                message.Add((byte)((SequenceNumberEvent >> 8) & 0xFF));
            }

            // frame size
            var messageSize = (uint)response.MessageLength;
            message.Add((byte)(messageSize & 0xFF));
            message.Add((byte)((messageSize >> 8) & 0xFF));
            message.Add((byte)((messageSize >> 16) & 0xFF));
            message.Add((byte)((messageSize >> 24) & 0xFF));
            // _token
            message.Add(TOKEN);
            message.AddRange(payload);

            ushort crc = Crc16.ComputeCrc(message.ToArray());

            // crc
            message.Add((byte)(crc & 0xFF)); // payload length LSB
            message.Add((byte)((crc >> 8) & 0xFF)); // payload length Msb

            Logger.Debug($"Rx Frame: responseId: {response.ResponseId}, sequenceNumber:{sequenceNumber.NumberValue}, crc: 0x{crc:x4}, messageSize: {response.MessageLength}.");
            sequenceNumber++;

            return message;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed _state (managed objects)
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Classes / Enum 

        #endregion


    }
}
