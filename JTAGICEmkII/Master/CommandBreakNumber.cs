using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal class CommandBreakNumber  : Command
     {
        #region Constructors 
        internal CommandBreakNumber(MasterCommandEnum messageId) : base(messageId)
        {
        }
        #endregion

        #region Fields 

        #endregion


        #region Properties 

        public byte BreakNumber { get; set; }

        public override int Size => base.Size + 1; // 1 byte for BreakNumber;

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override byte[] WriteToBytes()
        {
            var buffer = base.WriteToBytes();

            // Add BreakNumber bytes to the buffer
            buffer = buffer.Concat(new byte[] { (byte)BreakNumber }).ToArray();
            MessageLength = (uint)buffer.Length;
            return buffer;
        }

        public override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data == null || data.Length < Size)
                throw new ArgumentException("Data cannot be null or empty.", nameof(data));

            BreakNumber = data[base.Size];
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
