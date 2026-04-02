using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseEventDebug : ResponseEvent
    {

        #region Constructors 

        internal ResponseEventDebug(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public byte EventId { get; set; }

        public CommStateEnum CommState { get; set; }

        public CommErrorEnum CommError { get; set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        internal override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < 3)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            EventId = data[1];
            CommState = (CommStateEnum)data[2];
            CommError = (CommErrorEnum)data[3];
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
