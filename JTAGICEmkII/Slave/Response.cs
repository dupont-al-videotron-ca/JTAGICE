using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    public class Response : ISlaveResponse
    {

        #region Constructors 
        internal Response(SlaveResponseEnum messageId)
        {
            ResponseId = messageId;
            MessageLength = 0;
        }

        #endregion


        #region Fields 

        protected const int MessageIdOffset = 0;

        #endregion


        #region Properties 
        public SlaveResponseEnum ResponseId { get; protected set; }

        // This property will be serialize/deserialized when building frame.
        public uint MessageLength { get; set; }

        public virtual bool IsEvent
        {
            get
            {
                return ResponseId >= SlaveResponseEnum.EventRangeMin && ResponseId <= SlaveResponseEnum.EventRangeMax;
            }
        }

        public virtual int Size => 1;

        #endregion

        #region Public Methods 

        public virtual byte[] WriteToBytes()
        {
            byte[] buffer = new byte[1];
            buffer[MessageIdOffset] = (byte)ResponseId;
            MessageLength = (uint)buffer.Length; // Default length is 1 byte for the message ID.
            return buffer;
        }

        public virtual void ReadFromBytes(byte[] data)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("Data cannot be null or empty.", nameof(data));

            // The first byte is the response ID
            ResponseId = (SlaveResponseEnum)data[MessageIdOffset];
        }

        void ISlaveResponse.ReadFromBytes(byte[] data) => this.ReadFromBytes(data);

        #endregion

    }
}
