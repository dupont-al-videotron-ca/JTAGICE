using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using log4net;

namespace MyFramework.Queue
{

    /// <summary>
    /// Defines an class for registering, removing, and dispatching message Handlers. 
    /// Ultimately, the Synchronous Message Demultiplexer is responsible for waiting until 
    /// new messages occur. When it detects new messages, it informs the Initiation Dispatcher 
    /// to call back application-specific message handlers. 
    /// 
    /// </summary>
    internal class MsgDispatcher : IMsgQueueHandle, IDisposable
    {

        #region Members

        private ILog mLogger;
        private MsgQueueReactor mDispatchedQueue;
        private MsgLFThreadSet mLFThreadSet;
        private IMsgHandlerCollection mHandlers;
        private Mutex mLock;
        private bool disposedValue;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Queue">Queue to manage</param>
        internal MsgDispatcher(MsgQueueReactor Queue, MsgLFThreadSet LFThreadSet)
        {
            mLogger = LogManager.GetLogger(this.GetType());

            mDispatchedQueue = Queue;
            mLFThreadSet = LFThreadSet;

            mHandlers = new IMsgHandlerCollection();
            mLock = new Mutex();
            mLFThreadSet.RegisterHandle(this);
        }

        #endregion


        #region Property

        #endregion

        #region Methods

        /// <summary>
        /// Add Handler to the dispatcher without message filters.
        /// </summary>
        /// <param name="Handler">Message handler.</param>
        internal void Remove(IMsgHandler Handler)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mHandlers.Remove(Handler);
            }
        }

        /// <summary>
        /// Add new dispatch handler.
        /// </summary>
        /// <param name="Entry">Message handle entry point</param>
        internal void Add(IMsgHandler Entry)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mHandlers.Add(Entry);
            }
        }

        public override string ToString()
        {
            return mDispatchedQueue.Name;
        }
        #endregion


        #region IMsgQueueHandle Members

        /// <summary>
        /// Returns true when the queue has message.
        /// </summary>
        public bool QueueHasMsg
        {
            get
            {
                return !mDispatchedQueue.IsEmtpy;
            }
        }

        /// <summary>
        /// Returns the Queue
        /// </summary>
        public MsgQueue Queue
        {
            get
            {
                return mDispatchedQueue;
            }
        }

        /// <summary>
        /// Handle the messages of the Queue. 
        /// </summary>
        /// <returns></returns>
        public void HandleQueue()
        {
            mLFThreadSet.SuspendHandle(this);
            mLFThreadSet.PromoteNewLeader();

            try
            {
                mLogger.Debug($"Q {Queue.Name}, MbMsg {Queue.NbMsg}");

                Msg? TheMsg = mDispatchedQueue.DispathReceive();

                // while a message is available
                while (TheMsg != null)
                {
                    // Do, 
                    try
                    {
                        using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
                        {
                            // Dispatch message to all handlers
                            foreach (IMsgHandler Item in mHandlers)
                            {
                                // Yes, call the handler.
                                Item.HandleMsg(mDispatchedQueue, TheMsg);
                            }
                        }
                    }
                    catch (System.Exception Ex)
                    {
                        mLogger.Error($"{Thread.CurrentThread.Name} has generate an unhandle exception:", Ex);
                    }
                    finally
                    {
                        // Next message
                        TheMsg = mDispatchedQueue.DispathReceive();
                    }
                }
            }
            catch (System.Exception Ex)
            {
                mLogger.Error($"HandleQueue has generate an unhandle exception: ", Ex);
            }
            finally
            {
                mLFThreadSet.ResumeHandle(this);
            }
        }


        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    mLFThreadSet.UnregisterHandle(this);
                    mLock.Close();
                    mHandlers.Clear();
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

        #endregion
    }


    internal class MsgDispatcherList : List<MsgDispatcher>
    {
    }
}
