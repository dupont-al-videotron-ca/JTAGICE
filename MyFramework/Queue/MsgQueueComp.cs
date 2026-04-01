using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{
    /// <summary>
    /// Composite pattern class to send one message to multiple queue. Act as multicast message.
    /// </summary>
    public class MsgQueueComp : MsgQueue
    {
        #region Members

        /// <summary>
        /// Compisition list
        /// </summary>
        private MsgQueueList mCompQueue;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Q">First queue of the composition</param>
        public MsgQueueComp(MsgQueue Q)
        {
            mCompQueue = new MsgQueueList();
            AddQueue(Q);

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Queues">List of Queues</param>
        public MsgQueueComp(MsgQueue[] Queues)
        {
            mCompQueue = new MsgQueueList();
            AddRangeQueue(Queues);
        }

        #endregion

        #region Properties
        
        #endregion

        #region Methods

        /// <summary>
        /// Convert a MsgQueue object to a MsgQueueComp from exising Queue
        /// </summary>
        /// <param name="ExistingQ">Existing queue</param>
        /// <param name="ToAddQ">Queue to Add</param>
        /// <returns>return a MsgQueueComp</returns>
        public static MsgQueue ConvertToCompMsgQueue(MsgQueue ExistingQ, MsgQueue ToAddQ)
        {
            // Is existing queue exist ?
            if (ExistingQ == null)
                // No, add queue
                return new MsgQueueComp(ToAddQ);
            // Is conversion required ?
            else if (ExistingQ is MsgQueue)
            {
                // Yes,
                MsgQueueComp CompQ = new MsgQueueComp(ExistingQ);
                CompQ.AddQueue(ToAddQ);
                return CompQ;
            }
            else
            {
                // No, add queue to existing Composition (Multicast)
                ((MsgQueueComp)ExistingQ).AddQueue(ToAddQ);
                return ExistingQ;
            }
        }

        /// <summary>
        /// Add a MsgQueue from the composition
        /// </summary>
        /// <param name="CompQ">Message queue</param>
        /// <param name="Q">First queue of the composition</param>
        public static void AddQueue(MsgQueue CompQ, MsgQueue Q)
        {
            // Is Queue a composition ?
            if (CompQ is MsgQueueComp)
            {
                // Yes,
                ((MsgQueueComp)CompQ).AddQueue(Q);
            }
            // No, nothing to do
        }

        /// <summary>
        /// Add a MsgQueue to the composition
        /// </summary>
        /// <param name="Q">First queue of the composition</param>
        public void AddQueue(MsgQueue Q)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mCompQueue.Add(Q);
            }
        }

        /// <summary>
        /// Add a range of MsgQueue to the composition
        /// </summary>
        /// <param name="Queues">List of Queues</param>
        public void AddRangeQueue(MsgQueue[] Queues)
        {
            if (Queues.Length == 0)
                throw new System.ArgumentOutOfRangeException("Queues", "Is empty");

            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mCompQueue.AddRange(Queues);
            }
        }

        /// <summary>
        /// Remove a MsgQueue from the composition
        /// </summary>
        /// <param name="CompQ">Message queue</param>
        /// <param name="Q">First queue of the composition</param>
        public static void RemoveQueue(MsgQueue CompQ, MsgQueue Q)
        {
            // Is Queue a composition ?
            if (CompQ is MsgQueueComp)
            {
                // Yes,
                ((MsgQueueComp)CompQ).RemoveQueue(Q);
            }
            // No, nothing to do
        }

        /// <summary>
        /// Remove a MsgQueue from the composition
        /// </summary>
        /// <param name="Q">First queue of the composition</param>
        public void RemoveQueue(MsgQueue Q)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mCompQueue.Remove(Q);
            }
        }

        /// <summary>
        /// Add message identification to the filter. 
        /// </summary>
        /// <param name="FilterId">Message id to receive.</param>
        public override void AddFilter(MsgId FilterId)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (MsgQueue Q in mCompQueue)
                {
                    Q.AddFilter(FilterId);
                }
            }
        }

        /// <summary>
        /// Add messages identification to the filter.
        /// </summary>
        /// <param name="FiltersId">Message ids to receive.</param>
        public override void AddFilter(MsgId[] FiltersId)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (MsgQueue Q in mCompQueue)
                {
                    Q.AddFilter(FiltersId);
                }
            }
        }

        /// <summary>
        /// Remove messages identification from the filter.
        /// </summary>
        /// <param name="FiltersId">Message ids to remove.</param>
        public override void RemoveFilter(MsgId[] FiltersId)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (MsgQueue Q in mCompQueue)
                {
                    Q.RemoveFilter(FiltersId);
                }
            }
        }

        /// <summary>
        /// Remove message identification from the filter.
        /// </summary>
        /// <param name="FilterId">Message id to remove.</param>
        public override void RemoveFilter(MsgId FilterId)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (MsgQueue Q in mCompQueue)
                {
                    Q.RemoveFilter(FilterId);
                }
            }
        }

        /// <summary>
        /// Sends (Enqueue first in first out) a message. Notify and unblock receiver thread.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        public override void Send(Msg msg)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (MsgQueue Q in mCompQueue)
                {
                    Q.Send(msg);
                }
            }
        }

        /// <summary>
        /// Sends an urgent message (Enqueue last in forst out). Notify and unblock receiver thread.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        public override void SendUrgent(Msg msg)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (MsgQueue Q in mCompQueue)
                {
                    Q.SendUrgent(msg);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if ((disposing))
            {
                mCompQueue.Clear();
            }

            base.Dispose(disposing);
        }

        #endregion


    }
}
