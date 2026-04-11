using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.Master
{
    internal class CommandMemory : CommandMultipleByte
    {
        #region Constructors 
        internal CommandMemory(MasterCommandEnum messageId) : base(messageId, false)
        {
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public MemoryTypeEnum MemoryType{ get; set; }
        public UInt32 ByteCount { get; set; }
        public UInt32 Address { get; set; }

        public override int Size => base.Size + 1 + 4 + 4; // MemoryType (1 byte) + ByteCount (4 bytes) + Address (4 bytes) + Data (ByteCount bytes)

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();
            // Add MemoryType, ByteCount and Address bytes to the buffer
            buffer = buffer.Concat(new byte[] { (byte)MemoryType }).ToArray();
            MessageLength += 1;
            buffer = buffer.Concat(BitConverter.GetBytes(ByteCount)).ToArray();
            MessageLength += 4;
            buffer = buffer.Concat(BitConverter.GetBytes(Address)).ToArray();
            MessageLength += 4;
            buffer = buffer.Concat(WriteDataToBytes()).ToArray();
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < Size)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            MemoryType = (MemoryTypeEnum)data[base.Size];
            ByteCount = BitConverter.ToUInt32(data,  base.Size + 1);
            Address = BitConverter.ToUInt32(data, base.Size + 5);
            this.ReadDataToBytes(data, base.Size + 9);
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
