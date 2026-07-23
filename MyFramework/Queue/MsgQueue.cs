using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MyFramework.Exceptions;
using log4net;

namespace MyFramework.Queue
{
    public enum MsgQueuePriority
    {
        QueuePriorityLow = 0,
        QueuePriorityNormal,
        QueuePriorityHigh,
        QueuePriorityUrgent,
    }


    /// <summary>
    /// Class to manage a queue of message. 
    /// Message is queued only if it's Id is found in the Filters list or the Filters list is empty.
    /// </summary>
    public class MsgQueue: IDisposable
    {
        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public MsgQueue() : this("", MsgQueuePriority.QueuePriorityNormal)
        {
        }   

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="QueueName">Name of the Queue.</param>
        internal MsgQueue(string QueueName): this(QueueName, MsgQueuePriority.QueuePriorityNormal)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="QueueName">Name of the Queue.</param>
        /// <param name="Priority">Queue's priority.</param>
        internal MsgQueue(string QueueName, MsgQueuePriority Priority)
        {
            mQueue = new MsgLinkedList();
            mLock = new Mutex();
            mPriority = MsgQueuePriority.QueuePriorityNormal;
            mName = string.Empty;
            mFilters = new MsgIdDictionary();
            mWaitMsg = new EventWaitHandle(false, EventResetMode.AutoReset);
            mLog = log4net.LogManager.GetLogger(this.GetType());
            mName = QueueName;
            mPriority = Priority;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Priority">Queue's priority.</param>
        internal MsgQueue(MsgQueuePriority Priority):this("", Priority)
        {
        }

        #endregion

        #region Members

        protected bool mDisposed;
        /// <summary>
        /// Message queue
        /// </summary>
        private MsgLinkedList mQueue;

        /// <summary>
        /// Queue's name
        /// </summary>
        private String mName;

        /// <summary>
        /// Queue Priority
        /// </summary>
        private MsgQueuePriority mPriority;

        /// <summary>
        /// Multi-thread synchronisation.
        /// </summary>
        protected Mutex mLock;

        /// <summary>
        /// Message filters
        /// </summary>
        private MsgIdDictionary mFilters;

        /// <summary>
        /// Wait on event
        /// </summary>
        private EventWaitHandle mWaitMsg;

        /// <summary>
        /// Log4Net mLogger.
        /// </summary>
        private log4net.ILog mLog;
        private bool disposedValue;

        #endregion

        #region Properties

        /// <summary>
        /// Gets / Sets the message queue's name.
        /// </summary>
        public String Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        /// <summary>
        /// Gets / Sets Queue's log.
        /// </summary>
        public log4net.ILog QueueLog
        {
            get
            {
                return mLog;
            }
            set
            {
                mLog = value;
            }
        }

        /// <summary>
        /// Gets / Sets the message queue's name.
        /// </summary>
        public MsgQueuePriority Priority
        {
            get
            {
                return mPriority;
            }
            set
            {
                mPriority = value;
            }
        }

        /// <summary>
        /// Return true when the queue is empty.
        /// </summary>
        public virtual bool IsEmtpy
        {
            get
            {
                using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
                {
                    return mQueue.Count == 0;
                }
            }
        }

        public int NbMsg
        {
            get
            {
                using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
                {
                    return mQueue.Count;
                }
            }
        }
        #endregion

        #region Events
        public delegate void MsgReceiveHandler(object sender, EventArgs e);

        /// <summary>
        /// Occurs when a message is available in the message queue.
        /// </summary>
        public event MsgReceiveHandler? Received;

        public delegate void MsgSentHandler(object sender, EventArgs e);

        /// <summary>
        /// Occurs when a message is added to the message queue.
        /// </summary>
        public event MsgSentHandler? Sent;

        /// <summary>
        /// Generate Receive event.
        /// </summary>
        protected virtual void OnReceived()
        {
            if (Received != null)
                Received(this, EventArgs.Empty);
        }

        /// <summary>
        /// Generate Sent event.
        /// </summary>
        protected virtual void OnSent()
        {
            if (Sent != null)
                Sent(this, EventArgs.Empty);
        }

        #endregion

        #region Private methods

        #endregion

        #region Public methods

        /// <summary>
        /// Add message identification to the filter. 
        /// </summary>
        /// <param name="FilterId">Message id to receive.</param>
        public virtual void AddFilter(MsgId FilterId)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                // Is filters exist ?
                if (!mFilters.ContainsKey(FilterId.Id))
                {
                    // No, add it
                    mLog.DebugFormat("Add filter[{0}] to {1}.", FilterId, Name );
                    mFilters.Add(FilterId.Id, FilterId);
                }
            }
        }

        /// <summary>
        /// Add messages identification to the filter.
        /// </summary>
        /// <param name="FiltersId">Message ids to receive.</param>
        public virtual void AddFilter(MsgId[] FiltersId)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (MsgId FilterId in FiltersId)
                {
                    // Is filters exist ?
                    if (!mFilters.ContainsKey(FilterId.Id))
                    {
                        // No, add it
                        mLog.DebugFormat("Add filter[{0}] to {1}.", FilterId, Name );
                        mFilters.Add(FilterId.Id, FilterId);
                    }
                }
            }
        }

        /// <summary>
        /// Remove messages identification from the filter.
        /// </summary>
        /// <param name="FiltersId">Message ids to remove.</param>
        public virtual void RemoveFilter(MsgId[] FiltersId)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (MsgId FilterId in FiltersId)
                {
                    mLog.DebugFormat("Remove filter[{0}] to {1}.", FilterId, Name);
                    mFilters.Remove(FilterId.Id);
                }
            }
        }
        /// <summary>
        /// Remove message identification from the filter.
        /// </summary>
        /// <param name="FilterId">Message id to remove.</param>
        public virtual void RemoveFilter(MsgId FilterId)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mLog.DebugFormat("Remove filter[{0}] to {1}.", FilterId, Name );
                mFilters.Remove(FilterId.Id);
            }
        }

        /// <summary>
        /// Returns true when the message is part of the filtered message.
        /// </summary>
        /// <param name="msg">Message to verify</param>
        /// <returns>bool</returns>
        protected bool Accept(Msg msg)
        {
            // Is filter list empry ?
            if (mFilters.Count != 0)
                // No, message must be in filter list to be accepted.
                return mFilters.ContainsKey(msg.Id.Id);
            else
                // Yes, accept message.
                return true;
        }

        /// <summary>
        /// Send the message to Queue
        /// </summary>
        /// <param name="Q">The destination queue, may be null.</param>
        /// <param name="msg">The message to send.</param>
        public static void Send(MsgQueue Q, Msg msg)
        {
            if (Q != null)
                Q.Send(msg);
        }

        /// <summary>
        /// Sends (Enqueue first in first out) a message. Notify and unblock receiver thread.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        public virtual void Send(Msg msg)
        {
            bool MsgAccepted = false;
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                MsgAccepted = Accept(msg);
                if (MsgAccepted)
                {
                    if (mLog.Logger.IsEnabledFor(log4net.Core.Level.All))
                        mLog.DebugFormat("Sending message[{0}] to {1}.", msg.Id, Name);

                    mQueue.AddLast(msg);
                    mWaitMsg.Set();
                }
            }

            if (MsgAccepted)
            {
                OnSent();
                OnReceived();
            }
        }


        /// <summary>
        /// Send the message to Queue at the head of it.
        /// </summary>
        /// <param name="Q">The destination queue, may be null.</param>
        /// <param name="msg">The message to send.</param>
        public static void SendUrgent(MsgQueue Q, Msg msg)
        {
            if (Q != null)
                Q.SendUrgent(msg);
        }
        
        /// <summary>
        /// Sends an urgent message (Enqueue last in forst out). Notify and unblock receiver thread.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        public virtual void SendUrgent(Msg msg)
        {
            bool MsgAccepted = false;
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                MsgAccepted = Accept(msg);
                if (MsgAccepted)
                {
                    if (mLog.Logger.IsEnabledFor(log4net.Core.Level.All))
                        mLog.DebugFormat("Sending urgent message[{0}] to {1}.", msg.Id, Name);

                    mQueue.AddFirst(msg);
                    mWaitMsg.Set();
                }
            }

            if (MsgAccepted)
            {
                OnSent();
                OnReceived();
            }
        }

        /// <summary>
        /// Retreive the next available message from the message queue. If no message is available
        /// block calling thread until a message is available.
        /// </summary>
        /// <returns>Msg, the message</returns>
        /// <exception cref="QueueException"> Thrown on timeout or error.</exception>
        public virtual Msg? Receive()
        {
            return Receive(Timeout.Infinite);
        }


        /// <summary>
        /// Retreive the next available message from the message queue. If no message is available
        /// waits Timeout milliseconds for the message.
        /// </summary>
        /// <param name="Timeout">Waiting time for a message. Timeout.Infinite = wait for ever</param>
        /// <returns>Msg, the message</returns>
        /// <exception cref="QueueException"> Thrown on timeout or error.</exception>
        public virtual Msg? Receive(int Timeout)
        {
            return _Receive(Timeout);
        }

        /// <summary>
        /// Retreive the next available message from the message queue. If no message is available
        /// waits Timeout milliseconds for the message.
        /// </summary>
        /// <param name="Timeout">Waiting time for a message. Timeout.Infinite = wait for ever</param>
        /// <returns>Msg, the message</returns>
        /// <exception cref="QueueException"> Thrown on timeout or error.</exception>
        protected Msg? _Receive(int Timeout)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                Msg? Message;

                // Is message available ?
                if ((Message = _LocalRetreive()) != null)
                {
                    // Yes,
                    return Message;
                }
                // else continue,
            }

            try
            {
                // Is timeout expired ?
                if (!mWaitMsg.WaitOne(Timeout, false))
                {
                    // Yes,
                    throw new QueueTimeoutException();
                }
                else
                {
                    using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
                    {
                        return _LocalRetreive();
                    }
                }
            }
            catch (AbandonedMutexException)
            {
                throw new QueueTimeoutException();
            }
        }

        /// <summary>
        /// Retreive the next available message from the message queue. If no message is available,
        /// null is return.
        /// </summary>
        /// <returns>Msg, the message otherwise null</returns>
        protected Msg? _Receive()
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                return _LocalRetreive();
            }
        }

        /// <summary>
        /// Retreive the next available message from the message queue. 
        /// </summary>
        /// <returns>Msg, the message, otherwise false</returns>
        private Msg? _LocalRetreive()
        {
            // is a message is available?
            if (mQueue.Any())
            {
                // Yes, return message
                Msg Message = mQueue.First();
                if (mLog.Logger.IsEnabledFor(log4net.Core.Level.All))
                    mLog.DebugFormat("Receiving message[{0}] by {1}", Message.Id, Name);

                mWaitMsg.Reset();
                mQueue.RemoveFirst();
                return Message;
            }
            else
            {
                // No, Queue empty
                return null;
            }
        }

        public override string ToString()
        {
            return String.Format("Count = {0}", mQueue.Count);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    mFilters.Clear();
                    mQueue.Clear();
                    mWaitMsg.Close();
                    mLock.Close();
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
