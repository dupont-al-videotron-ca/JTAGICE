using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{

    /// <summary>
    /// Implement a Queue thread that calls the method define in IMsgHandler inteface
    /// </summary>
    public class MsgQueueThreadHandler: MsgQueueThread
    {
        #region Members

        private IMsgHandler mMsgHandler;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="h">Message hangler</param>
        public MsgQueueThreadHandler(IMsgHandler h)
        {
            mMsgHandler = h;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Call handlers
        /// </summary>
        /// <param name="Message"></param>
        protected override void AnalyzeReceiveMsg(Msg? Message)
        {
            mMsgHandler.HandleMsg(Queue, Message);
        }

        #endregion
    }
}
