using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseEventDebug : ResponseEvent
    {

        #region Constructors 

        internal ResponseEventDebug(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        #endregion

        #region Properties 

        public byte EventId { get; set; }

        public CommStateEnum CommState { get; set; }

        public CommErrorEnum CommError { get; set; }
        public override int Size => base.Size + 3;

        #endregion


        #region Public Methods 
        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            buffer = buffer.Concat(new byte[] { EventId }).ToArray();
            buffer = buffer.Concat(new byte[] { (byte)CommState }).ToArray();
            buffer = buffer.Concat(new byte[] { (byte)CommError }).ToArray();

            MessageLength = (uint)buffer.Length;
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            EventId = data[base.Size];
            CommState = (CommStateEnum)data[base.Size + 1];
            CommError = (CommErrorEnum)data[base.Size + 2];
        }

        #endregion
    }
}
