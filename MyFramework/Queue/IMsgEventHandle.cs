using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{
    public interface IMsgQueueHandle
    {
        /// <summary>
        /// Returns true when the queue has message.
        /// </summary>
        bool QueueHasMsg
        {
            get;
        }

        /// <summary>
        /// Returns the Queue
        /// </summary>
        MsgQueue Queue
        {
            get;
        }

        /// <summary>
        /// Handle the messages of the Queue. 
        /// </summary>
        void HandleQueue();


    }

    public class IMsgQueueHandleList : List<IMsgQueueHandle>
    {
        public bool Contains(MsgQueue Q)
        {
            foreach (IMsgQueueHandle Iter in this)
            {
                if (Iter.Queue == Q)
                    return true;
            }
            return false;
        }

    }

    public class IMsgQueueHandleMap : Dictionary<MsgQueue, IMsgQueueHandle>
    {
    }
}
