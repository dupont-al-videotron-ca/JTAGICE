using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{
    /// <summary>
    /// Factory class to instanciate message for queue message.
    /// </summary>
    public class MsgFactory
    {
        /// <summary>
        /// Create a massage to be used in queue message.
        /// </summary>
        /// <param name="id">Message identifier</param>
        /// <returns>Msg, message</returns>
        public static Msg CreateMsg(MsgId id)
        {
            return new Msg(id);
        }

        /// <summary>
        /// Create a massage to be used in queue message.
        /// </summary>
        /// <param name="id">Message identifier</param>
        /// <returns>Msg, message</returns>
        public static Msg CreateMsg(int id)
        {
            return new Msg(new MsgId(id));
        }

        /// <summary>
        /// Create a massage to be used in queue message.
        /// </summary>
        /// <param name="id">Message identifier</param>
        /// <param name="e">.Net EventArgs object.</param>
        /// <returns>Msg, message</returns>
        public static Msg CreateMsg(MsgId id, EventArgs e)
        {
            return new MsgEventArgs(id, e);
        }

        /// <summary>
        /// Create a massage to be used in queue message.
        /// </summary>
        /// <param name="id">Message identifier</param>
        /// <param name="e">.Net EventArgs object.</param>
        /// <returns>Msg, message</returns>
        public static Msg CreateMsg(int id, EventArgs e)
        {
            return new MsgEventArgs(new MsgId(id), e);
        }
    }
}
