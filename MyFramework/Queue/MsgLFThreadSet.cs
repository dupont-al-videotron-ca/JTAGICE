using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using log4net;
using MyFramework.Threading;

namespace MyFramework.Queue
{
    /// <summary>
    /// This class implement the Leader / Followers design pattern. The leader thread use the MsgReactor
    /// To dispatch message to QueueHandler.
    /// </summary>
    public class MsgLFThreadSet
    {
        #region Members

        /// <summary>
        /// Number of incresing thread.
        /// </summary>
        private const int mIncreaseLfThread = 10;

        /// <summary>
        /// Reactor.
        /// </summary>
        private IMsgReactor mReactor;

        /// <summary>
        /// Current leader thread.
        /// </summary>
        private Thread? mLeader;

        /// <summary>
        /// Internal lock
        /// </summary>
        private Mutex mLock;

        /// <summary>
        /// Follower notification
        /// </summary>
        private AutoResetEvent mFollowerCondition;

        /// <summary>
        /// Number of actual worker thread (Leader + Followers)
        /// </summary>
        private int mNbWorkerLFThread;

        /// <summary>
        /// Number of followers (Waiting thread)
        /// </summary>
        private int mNbFollowersThread;

        /// <summary>
        /// Minimum of threads
        /// </summary>
        private int mMinThread;
        /// <summary>
        /// Maximum of threads
        /// </summary>
        private int mMaxThread;

        /// <summary>
        /// Thread number for trace log.
        /// </summary>
        private int mThreadIdNumber;

        /// <summary>
        /// mLogger
        /// </summary>
        private ILog mLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal MsgLFThreadSet() : this(new MsgReactor())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal MsgLFThreadSet(IMsgReactor UserReactor)
        {
            mLogger = LogManager.GetLogger(this.GetType());

            mReactor = UserReactor;
            mFollowerCondition = new AutoResetEvent(false);
            mLock = new Mutex();

            mMaxThread = 100;
            mMinThread = mIncreaseLfThread;
            mNbFollowersThread = 0;
            mThreadIdNumber = 1;

            IncreaseFollower();
        }

        #endregion

        #region Properties

        /// <summary>
        /// thread name
        /// </summary>
        private static String ThreadName
        {
            get
            {
                return "L/F-";
            }
        }

        /// <summary>
        /// Gets the number of allocated thread.
        /// </summary>
        public int AllocatedWorkerThread
        {
            get
            {
                using (LockMutex l = new LockMutex(mLock))
                {
                    return mNbWorkerLFThread;
                }
            }
        }

        /// <summary>
        /// Gets the number of available worker thread.
        /// </summary>
        public int AvailableWorkerThread
        {
            get
            {
                using (LockMutex l = new LockMutex(mLock))
                {
                    return mNbFollowersThread;
                }
            }
        }

        /// <summary>
        /// Gets the number of busy worker thread.
        /// </summary>
        public int BusyWorkerThread
        {
            get
            {
                using (LockMutex l = new LockMutex(mLock))
                {
                    return mNbWorkerLFThread - mNbFollowersThread;
                }
            }
        }

        /// <summary>
        /// Gets the number of maximum worker thread.
        /// </summary>
        public int MaxWorkerThread
        {
            get
            {
                using (LockMutex l = new LockMutex(mLock))
                {
                    return mMaxThread;
                }
            }
            set
            {
                using (LockMutex l = new LockMutex(mLock))
                {
                    mMaxThread = value;
                }
            }
        }

        /// <summary>
        /// Gets the number of minimum worker thread.
        /// </summary>
        public int MinWorkerThread
        {
            get
            {
                using (LockMutex l = new LockMutex(mLock))
                {
                    return mMinThread;
                }
            }
            set
            {
                using (LockMutex l = new LockMutex(mLock))
                {
                    mMinThread = value;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Join the the Leader follower main loop.
        /// </summary>
        /// <param name="state">not applicable.</param>
        private void Join(object? state)
        {
            Thread.CurrentThread.IsBackground = true;
            Thread.CurrentThread.Priority = ThreadPriority.Normal;

            using (LockMutex l = new LockMutex(mLock))
            {
                if (Thread.CurrentThread.Name == null)
                    Thread.CurrentThread.Name = MsgLFThreadSet.ThreadName + mThreadIdNumber++;

                if (mLogger.Logger.IsEnabledFor(log4net.Core.Level.All))
                    mLogger.DebugFormat("Joinning thread set {0}", Thread.CurrentThread.Name);

                while (true)
                {
                    try
                    {
                        while (mLeader != null)
                        {
                            if (mLogger.Logger.IsEnabledFor(log4net.Core.Level.All))
                                mLogger.DebugFormat("Follower {0}", Thread.CurrentThread.Name);

                            mNbFollowersThread++;
                            l.PopLocks();

                            mFollowerCondition.WaitOne();

                            l.PushLocks();
                            mNbFollowersThread--;
                        }

                        mLeader = Thread.CurrentThread;
                        
                        if (mLogger.Logger.IsEnabledFor(log4net.Core.Level.All))
                            mLogger.DebugFormat("Leader {1}", Thread.CurrentThread.Name, mLeader.Name);

                        l.PopLocks();

                        mReactor.HandleQueueEvent();
                    }
                    catch (ThreadAbortException)
                    {
                        l.PushLocks();
                        return;
                    }
                    finally
                    {
                        l.PushLocks();
                    }
                }
            }
        }

        /// <summary>
        /// Promote next available followers to leader.
        /// </summary>
        internal void PromoteNewLeader()
        {
            using (LockMutex l = new LockMutex(mLock))
            {
                // Only leader thread can call this method
                // Is Leader thread ?
                if (mLeader == Thread.CurrentThread)
                {
                    // Yes,
                    mLeader = null;
                    mFollowerCondition.Set();
                    mLogger.DebugFormat("PromoteNewLeader {0}", Thread.CurrentThread.Name);
                }
                else
                {
                    mLogger.ErrorFormat("Wrong call to PromoteNewLeader {0}", Thread.CurrentThread.Name);
                }
            }
        }

        /// <summary>
        /// Register a Queue handler to the reactor
        /// </summary>
        /// <param name="h">Message queue handler</param>
        internal void RegisterHandle(IMsgQueueHandle h)
        {
            mReactor.RegisterHandle(h);
            float a = ((float) mNbWorkerLFThread / (float) mReactor.Count) * 100.00f;

            // Is lest than 1%
            if (a < 25.0f)
                // Yes, increase
                IncreaseFollower();
        }

        /// <summary>
        /// Resume the Queue handler (put handler in the ready list)
        /// </summary>
        /// <param name="h">Message queue handler</param>
        internal void ResumeHandle(IMsgQueueHandle h)
        {
            mReactor.ResumeHandle(h);
        }

        /// <summary>
        /// Suspend the Queue handler (put handler in the suspended list)
        /// </summary>
        /// <param name="h">Message queue handler</param>
        internal void SuspendHandle(IMsgQueueHandle h)
        {
            mReactor.SuspendHandle(h);
        }

        /// <summary>
        /// Unregister the Queue handler (remove handler from the reactor)
        /// </summary>
        /// <param name="h">Message queue handler</param>
        internal void UnregisterHandle(IMsgQueueHandle h)
        {
            mReactor.UnregisterHandle(h);
        }

        /// <summary>
        /// Increase the number of thread.
        /// </summary>
        private void IncreaseFollower()
        {

            int WorkerThread;
            int IoThread;

            ThreadPool.GetAvailableThreads(out WorkerThread, out IoThread);

            using (LockMutex l = new LockMutex(mLock))
            {

                if (!(WorkerThread >= mNbWorkerLFThread + mIncreaseLfThread))
                    return;


                if (mMinThread > mNbWorkerLFThread || mNbWorkerLFThread < mMaxThread)
                {
                    for (int i = 0; i < mIncreaseLfThread; i++)
                    {
                        // Start Followers thread
                        ThreadPool.QueueUserWorkItem(new WaitCallback(Join));
                        mNbWorkerLFThread++;
                    }
                }
            }
        }
        #endregion


    }
}
