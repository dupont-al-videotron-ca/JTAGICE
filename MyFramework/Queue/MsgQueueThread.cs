#pragma warning disable CS1591, CS8618
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyFramework.Queue
{
    /// <summary>
    /// Class providing a thread to extract message from the MsgQueue
    /// </summary>
    public abstract class MsgQueueThread : IDisposable
    {

        #region Members

        /// <summary>
        /// Internal thread
        /// </summary>
        private Task? mTask;
        private bool mRunning;

        protected MsgQueue mQueue;
        private CancellationTokenSource source;
        private CancellationToken token;
        private bool disposedValue;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public MsgQueueThread() :this("", MsgQueuePriority.QueuePriorityNormal)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="QueueName">Name of the Queue.</param>
        public MsgQueueThread(string QueueName) : this(QueueName, MsgQueuePriority.QueuePriorityNormal)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="QueueName">Name of the Queue.</param>
        /// <param name="Priority">Queue's priority.</param>
        public MsgQueueThread(string QueueName, MsgQueuePriority Priority)
        {
            mQueue = new MsgQueue(QueueName, Priority);
            // Define the cancellation token.
            source = new CancellationTokenSource();
            token = source.Token;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Priority">Queue's priority.</param>
        public MsgQueueThread(MsgQueuePriority Priority): this("", Priority)
        {
        }

        #endregion



        #region Private methods

        private void LocalMain()
        {
            Main();
        }

        #endregion

        #region Protected methods
        /// <summary>
        /// AnalyzeReceiveMsg. To be overrided.
        /// </summary>
        protected abstract void AnalyzeReceiveMsg(Msg? Message);


        protected virtual void Main()
        {
            Thread.CurrentThread.Name = Queue.Name;
            while (mRunning)
            {
                try
                {
                    Msg? RxMsg = mQueue.Receive();
                    if (RxMsg != null)
                    {
                        AnalyzeReceiveMsg(RxMsg);
                    }
                }
                catch (TaskCanceledException ex)
                {
                    mQueue.QueueLog.Debug("Task canceled.", ex);
                    mRunning = false;
                }
                catch (System.Exception Ex)
                {
                    mQueue.QueueLog.Error("runtime error: {0}", Ex);
                }
            }
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Open the Queue thread to dispatch message.
        /// </summary>
        public virtual void Open()
        {
            if (mTask == null)
            {
                mTask = Task.Run(() => LocalMain(), this.token);
            }
        }
        /// <summary>
        /// Close the receiving thread
        /// </summary>
        public virtual bool Close()
        {
            if (mTask != null && !mTask.IsCompleted)
            {
                token.ThrowIfCancellationRequested();
                mRunning = false;
                source.Cancel();
                if (!mTask.Wait(1000))
                {
                    return false;
                }
            }

            mTask = null;

            return true;
        }

        /// <summary>
        /// Gets the underlaying message queue.
        /// </summary>
        public MsgQueue Queue
        {
            get
            {
                return mQueue;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    MsgQueueManager.UnregisterQueue(mQueue);

                    Close();

                    mQueue.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MsgQueueThread()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
