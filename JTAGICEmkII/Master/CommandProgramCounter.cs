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
    internal class CommandProgramCounter : Command
    {
        #region Constructors 
        internal CommandProgramCounter(MasterCommandEnum messageId) : base(messageId)
        {
        }

        #endregion



        #region Properties 
        override public int Size => base.Size + 4; // Base size + 4 bytes for ProgramCounter

        public UInt32 ProgramCounter { get; set; }

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();

            // Add ProgramCounter bytes to the buffer
            buffer = buffer.Concat(BitConverter.GetBytes(ProgramCounter)).ToArray();
            MessageLength += 4; // Increment message length by 4 bytes for the ProgramCounter
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            ProgramCounter = BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(base.Size, 4));
        }
        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 



        #endregion


    }
}
