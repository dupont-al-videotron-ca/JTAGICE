#pragma warning disable CS1591, CS8603
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{
    /// <summary>
    /// Class to send timer expiration message.
    /// </summary>
    public class MsgTimer : Msg
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MessageId">Message identifier.</param>
        /// <param name="Timer">The timer that expired.</param>
        public MsgTimer(MsgId MessageId, Threading.BaseTimer Timer)
            :base(MessageId, Timer)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MessageId">Message identifier.</param>
        /// <param name="Timer">The timer that expired.</param>
        public MsgTimer(int MessageId, Threading.BaseTimer Timer)
            :base(new MsgId(MessageId), Timer)
        {
        }

        /// <summary>
        /// Gets timer associated to this message.
        /// </summary>
        public Threading.BaseTimer Timer
        {
            get
            {
                return base.Data as Threading.BaseTimer;
            }
        }
    }
}
