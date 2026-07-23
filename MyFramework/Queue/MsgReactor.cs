using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using log4net;
using MyFramework.Threading;


namespace MyFramework.Queue
{
    /// <summary>
    /// The Reactor design pattern handles service requests that are
    /// delivered concurrently to an application by one or more
    /// clients. Each service in an application may consist of
    /// serveral methods and is represented by a separate message handler
    /// that is responsible for dispatching service-specific requests.
    /// 
    /// Dispatching of message handlers is performed by an initiation dispatcher, 
    /// which manages the registered message handlers. 
    /// Demultiplexing of service requests is performed by a synchronous reactor.
    /// 
    /// </summary>
    internal class MsgReactor : IDisposable, IMsgReactor
    {

        #region Members
        /// <summary>
        /// Internal concurency lock
        /// </summary>
        private Mutex mLock;

        /// <summary>
        /// List of waiting handlers 
        /// </summary>
        private IMsgQueueHandleList mHandlers;

        /// <summary>
        /// List of busy handlers 
        /// </summary>
        private IMsgQueueHandleList mSuspendedHandlers;

        /// <summary>
        /// Event to wakeup the reactor
        /// </summary>
        private AutoResetEvent mWakeup;

        protected ILog mLogger;
        private bool disposedValue;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal MsgReactor()
        {
            mLock = new Mutex();
            mHandlers = new IMsgQueueHandleList();
            mSuspendedHandlers = new IMsgQueueHandleList();
            mWakeup = new AutoResetEvent(false);
            mLogger = LogManager.GetLogger(this.GetType());
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Called by the sender method when a message has been sent.
        /// </summary>
        /// <param name="sender">Queue sencer</param>
        /// <param name="e">Event</param>
        void OnQueueReceive(object sender, EventArgs e)
        {
            using (LockMutex l = new LockMutex(mLock))
            {
                // Is queue already processing message ?
                if (!mSuspendedHandlers.Contains((MsgQueue) sender))
                {
                    // No, wake up L/F
                    mWakeup.Set();
                }
            }
        }


        #region IMsgReactor Members

        /// <summary>
        /// Gets the number of handlers
        /// </summary>
        public int Count
        {
            get
            {
                using (LockMutex l = new LockMutex(mLock))
                {
                    return mHandlers.Count + mSuspendedHandlers.Count;
                }
            }
        }

        /// <summary>
        /// Demultiplex and dispatch the message queue.
        /// </summary>
        /// <returns></returns>
        public void HandleQueueEvent()
        {
            // loop until a message is handled
            while (true)
            {
                // Get a new copy of available MsgQueueHandlers
                IMsgQueueHandle[] Handlers;
                using (LockMutex l = new LockMutex(mLock))
                {
                    Handlers = mHandlers.ToArray();
                }


                foreach (IMsgQueueHandle IterHandler in Handlers)
                {
                    // Is message avaialle in this queue ?
                    if (IterHandler.QueueHasMsg)
                    {
                        // Yes,

                        // Process All message of this Queue.
                        IterHandler.HandleQueue();
                        return;
                    }
                }

                // No queue has a message , Wait

                mLogger.Debug("mWakeup.WaitOne");

                mWakeup.WaitOne();
            }
        }

        /// <summary>
        /// Register a Queue handler to the reactor
        /// </summary>
        /// <param name="h">Message queue handler</param>
        public void RegisterHandle(IMsgQueueHandle h)
        {
            using (LockMutex l = new LockMutex(mLock))
            {
                if (!mHandlers.Contains(h))
                {
                    mHandlers.Add(h);
                    h.Queue.Received += new MsgQueue.MsgReceiveHandler(OnQueueReceive);

                    if (!h.Queue.IsEmtpy)
                        mWakeup.Set();
                }

            }
        }

        /// <summary>
        /// Resume the Queue handler (put handler in the ready list)
        /// </summary>
        /// <param name="h">Message queue handler</param>
        public void ResumeHandle(IMsgQueueHandle h)
        {
            using (LockMutex l = new LockMutex(mLock))
            {
                int Index = mSuspendedHandlers.IndexOf(h);
                mHandlers.Add(mSuspendedHandlers[Index]);
                mSuspendedHandlers.RemoveAt(Index);
            }
        }

        /// <summary>
        /// Suspend the Queue handler (put handler in the suspended list)
        /// </summary>
        /// <param name="h">Message queue handler</param>
        public void SuspendHandle(IMsgQueueHandle h)
        {
            using (LockMutex l = new LockMutex(mLock))
            {
                int Index = mHandlers.IndexOf(h);
                mSuspendedHandlers.Add(mHandlers[Index]);
                mHandlers.RemoveAt(Index);
            }
        }

        /// <summary>
        /// Unregister the Queue handler (remove handler from the reactor)
        /// </summary>
        /// <param name="h">Message queue handler</param>
        public void UnregisterHandle(IMsgQueueHandle h)
        {
            using (LockMutex l = new LockMutex(mLock))
            {
                int Index = mHandlers.IndexOf(h);
                if (Index != -1)
                {
                    mHandlers.RemoveAt(Index);
                    mSuspendedHandlers.Remove(h);
                    h.Queue.Received -= OnQueueReceive;
                }
            }
        }

        #endregion

        public override string ToString()
        {
            using (LockMutex l = new LockMutex(mLock))
            {
                return string.Format("H:{0}, S:{1}", mHandlers.Count, mSuspendedHandlers.Count);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    mLock.Close();
                    mWakeup.Close();
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

}
