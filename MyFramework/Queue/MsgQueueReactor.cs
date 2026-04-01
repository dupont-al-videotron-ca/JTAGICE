using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyFramework.Queue
{
    /// <summary>
    /// Class used by an application to Send / receive message using the IMsgHandler interface callback.
    /// </summary>
    public class MsgQueueReactor : MsgQueue
    {
        private MsgDispatcher mDispatcher;
        private bool mOpenned;

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        internal MsgQueueReactor()
            : base()
        {
            mDispatcher = new MsgDispatcher(this, MsgQueueManager.LFThreadSet);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="QueueName">Name of the Queue.</param>
        internal MsgQueueReactor(string QueueName)
            : base(QueueName)
        {
            mDispatcher = new MsgDispatcher(this, MsgQueueManager.LFThreadSet);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="QueueName">Name of the Queue.</param>
        /// <param name="Priority">Queue's priority.</param>
        internal MsgQueueReactor(string QueueName, MsgQueuePriority Priority)
            : base(QueueName, Priority)
        {
            mDispatcher = new MsgDispatcher(this, MsgQueueManager.LFThreadSet);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Priority">Queue's priority.</param>
        internal MsgQueueReactor(MsgQueuePriority Priority)
            : base(Priority)
        {
            mDispatcher = new MsgDispatcher(this, MsgQueueManager.LFThreadSet);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Return true when the queue is empty.
        /// </summary>
        public override bool IsEmtpy
        {
            get
            {
                using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
                {
                    if (mOpenned)
                        return base.IsEmtpy;
                    else
                        return true;
                }
            }
        }

        /// <summary>
        /// Return truwe when the queue is opened
        /// </summary>
        public virtual bool IsOpen
        {
            get
            {
                using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
                {
                    return mOpenned;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Open the Queue reactor to start receiving message
        /// </summary>
        public virtual void Open()
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mOpenned = true;
            }
        }

        /// <summary>
        /// CLose the Queue reactor to stop receiving message
        /// </summary>
        public virtual void Close()
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mOpenned = false;
            }
        }

        /// <summary>
        /// Dispose Queue
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mDispatcher.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Invalid operation.
        /// </summary>
        /// <returns></returns>
        public override Msg Receive()
        {
            throw new InvalidOperationException("Cannot call Receive. Register a message handler instead.");
        }

        /// <summary>
        /// Invalid operation.
        /// </summary>
        /// <returns></returns>
        public override Msg Receive(int Timeout)
        {
            throw new InvalidOperationException("Cannot call Receive. Register a message handler instead.");
        }

        /// <summary>
        /// Return the next available message.
        /// </summary>
        /// <returns>Msg available, othrewise null</returns>
        internal Msg? DispathReceive()
        {
            // Is message available ?
            return _Receive();
        }

        /// <summary>
        /// Register message Handler with its filter.
        /// </summary>
        /// <param name="Handler">Message handler.</param>
        /// <param name="Filter">Message filter.</param>
        public void RegisterHandler(IMsgHandler Handler, MsgId Filter)
        {
            mDispatcher.Add(Handler);
            AddFilter(Filter);
        }

        /// <summary>
        /// Register message Handler with its filters.
        /// </summary>
        /// <param name="Handler">Message handler.</param>
        /// <param name="Filters">Message filters.</param>
        public void RegisterHandler(IMsgHandler Handler, MsgId[] Filters)
        {
            mDispatcher.Add(Handler);
            AddFilter(Filters);
        }


        /// <summary>
        /// Register message Handler without message filters.
        /// </summary>
        /// <param name="Handler">Message handler.</param>
        public void RegisterHandler(IMsgHandler Handler)
        {
            mDispatcher.Add(Handler);
        }

        /// <summary>
        /// unregister message Handler.
        /// </summary>
        /// <param name="Handler">Message handler.</param>
        public void UnregisterHandler(IMsgHandler Handler)
        {
            mDispatcher.Remove(Handler);
        }
        #endregion

    }
}
