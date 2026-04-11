using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.Master
{
    public class Command : IMasterCommand
    {

        #region Constructors 
        internal Command(MasterCommandEnum messageId)
        {
            MessageId = messageId;
            MessageLength = 0; // Default length is 1 byte for the message ID
        }

        #endregion


        #region Fields 

        protected const int MessageIdOffset = 0;

        #endregion


        #region Properties 
        public MasterCommandEnum MessageId { get; protected set; }

        public virtual int Size => 1; // Total size includes the message ID byte only.


        // This property will be serialize/deserialized when building frame.
        public uint MessageLength { get; set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public virtual byte[] WriteToBytes()
        {
            byte[] buffer = new byte[1];
            buffer[0] = (byte)MessageId; // First byte is the message ID
            MessageLength = (uint)buffer.Length;
            return buffer;

        }

        public virtual void ReadFromBytes(byte[] data)
        {

            if (data == null || data.Length < Size)
                throw new ArgumentException("Data cannot be null or empty.", nameof(data));

            // The first byte is the response ID
            MessageId = (MasterCommandEnum)data[0];
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
