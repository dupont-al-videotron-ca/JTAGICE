using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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


        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
