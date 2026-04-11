using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandAddress : Command
    {
        internal CommandAddress(MasterCommandEnum messageId) : base(messageId)
        {
        }

        public UInt32 Address { get; set; }

        public override int Size => base.Size + 4; // 1 byte for MessageId and 4 bytes for Address

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

    }
}
