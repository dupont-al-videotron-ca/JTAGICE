using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class Response : ISlaveResponse
    {

        #region Constructors 
        internal Response(SlaveResponseEnum messageId)
        {
            ResponseId = messageId;
        }

        #endregion


        #region Fields 

        protected const int MessageIdOffset = 0;

        #endregion


        #region Properties 
        public SlaveResponseEnum ResponseId { get; set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        internal virtual void ReadFromBytes(byte[] data)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("Data cannot be null or empty.", nameof(data));

            // The first byte is the response ID
            ResponseId = (SlaveResponseEnum)data[MessageIdOffset];
        }

        void ISlaveResponse.ReadFromBytes(byte[] data) => this.ReadFromBytes(data);


        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
