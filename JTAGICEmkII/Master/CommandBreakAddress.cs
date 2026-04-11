using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandBreakAddress: CommandBreakNumber
     {
        #region Constructors 
        internal CommandBreakAddress(MasterCommandEnum messageId) : base(messageId)
        {
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 
        public UInt32 Address { get; set; }

        public override int Size => base.Size + 4; // 4 bytes for Address;

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();

            // Add Address bytes to the buffer
            buffer = buffer.Concat(BitConverter.GetBytes(Address)).ToArray();
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data == null || data.Length < Size)
                throw new ArgumentException("Data cannot be null or empty.", nameof(data));

            Address = BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(base.Size, 4));

        }

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
