using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{

    /// <summary>
    /// Interface for message handler used by the message reactor.
    /// </summary>
    public interface IMsgHandler
    {
        /// <summary>
        /// Handler of message
        /// </summary>
        /// <param name="source">Queue from where the message has been read by the message reactor.</param>
        /// <param name="m">The message.</param>
        /// <returns>Returns true when the message has been analysed.</returns>
        bool HandleMsg(MsgQueue source, Msg? m);
    }

    internal class IMsgHandlerCollection : List<IMsgHandler>
    {
        public IMsgHandlerCollection()
            : base()
        {
        }

        public IMsgHandlerCollection(IMsgHandlerCollection Src)
            : base(Src)
        {
        }
    }

  
}
