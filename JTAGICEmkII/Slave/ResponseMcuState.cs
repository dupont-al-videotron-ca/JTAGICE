using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseMcuState : Response
    {

        #region Constructors 
        internal ResponseMcuState(SlaveResponseEnum messageId) : base(messageId)
        {
        }

        internal override void ReadFromBytes(byte[] data)
        {
            base.ReadFromBytes(data);
            if (data.Length < 2)
                throw new ArgumentOutOfRangeException($"Data length is insufficient for response {this.GetType().Name}.");

            State = (McuStateEnum)data[1];

        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public McuStateEnum State { get; set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
