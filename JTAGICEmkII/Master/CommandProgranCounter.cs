using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.Master
{
    internal class CommandProgranCounter : Command
    {
        #region Constructors 
        internal CommandProgranCounter(MasterCommandEnum messageId) : base(messageId)
        {
        }

        #endregion



        #region Properties 
        override public int Size => base.Size + 4; // Base size + 4 bytes for ProgrammeCounter

        public UInt32 ProgrammeCounter { get; set; }

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();

            // Add ProgrammeCounter bytes to the buffer
            buffer = buffer.Concat(BitConverter.GetBytes(ProgrammeCounter)).ToArray();
            MessageLength += 4; // Increment message length by 4 bytes for the ProgrammeCounter
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            ProgrammeCounter = BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(base.Size, 4));
        }
        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 



        #endregion


    }
}
