using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal class ResponseEvent : Response
    {


        #region Constructors 

        internal ResponseEvent(SlaveResponseEnum messageId) : base(messageId)
        {
            if (messageId < (SlaveResponseEnum)EventRangeMin || messageId > (SlaveResponseEnum)EventRangeMax)
                throw new ArgumentException($"Invalid message ID for {nameof(ResponseEvent)}. Expected a value between {(SlaveResponseEnum)EventRangeMin} and {(SlaveResponseEnum)EventRangeMax}, got {messageId}.");
        }

        #endregion


        #region Fields 

        private const byte EventRangeMin = 0xE0;
        private const byte EventRangeMax = 0xFF;    


        #endregion


        #region Properties 

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
