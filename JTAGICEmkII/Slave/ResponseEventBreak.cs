using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseEventBreak : ResponseEvent
    {

        #region Constructors 

        internal ResponseEventBreak(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        #endregion

        #region Properties 

        public UInt32 ProgramCounter { get; set; }
        
        public EventBreakCauseEnum BreakCause { get; set; }
        public override int Size => base.Size + 4 +1;

        #endregion

        #region Public Methods 
        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            buffer = buffer.Concat(BitConverter.GetBytes(ProgramCounter)).ToArray();
            buffer = buffer.Concat(new byte[] { (byte)BreakCause }).ToArray();

            MessageLength = (uint)buffer.Length;
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            ProgramCounter = BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(base.Size, 4));
            BreakCause = (EventBreakCauseEnum)data[base.Size + 4];
        }

        #endregion


    }
}
