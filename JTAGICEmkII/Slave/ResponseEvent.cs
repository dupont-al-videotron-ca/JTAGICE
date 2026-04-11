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
            if (messageId < SlaveResponseEnum.EventRangeMin || messageId > SlaveResponseEnum.EventRangeMax)
                throw new ArgumentException($"Invalid message ID for {nameof(ResponseEvent)}. Expected a value between {SlaveResponseEnum.EventRangeMin} and {SlaveResponseEnum.EventRangeMax}, got {messageId}.");
        }

        #endregion

        #region Properties 

        public override bool IsEvent
        {
            get
            {
                return (this.ResponseId >= SlaveResponseEnum.EventRangeMin && this.ResponseId <= SlaveResponseEnum.EventRangeMax);
            }
        }

        #endregion

    }
}
