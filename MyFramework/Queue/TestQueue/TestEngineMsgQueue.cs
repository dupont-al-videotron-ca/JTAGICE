using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using log4net;
using MyFramework.Queue;

namespace TestQueue
{
    public interface ITestEngine
    {
        void StartTest();
        void StopTest();
        int MsgIn
        {
            get;
        }

        int MsgOut
        {
            get;
        }

    }

    public class TestEngineMsgQueue : ITestEngine
    {
        protected log4net.ILog mLogger;
        protected System.ComponentModel.BackgroundWorker mSenderBackgroundWorker;
        protected System.ComponentModel.BackgroundWorker mReceiverBackgroundWorker;
        protected MsgQueue mTestQueue;
        protected String mName;
        protected int mMsgIn;
        protected int mMsgOut;
        protected System.Threading.ManualResetEvent mSenderDone;
        protected System.Threading.ManualResetEvent mReceiverDone;

        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ParentForm"></param>
        internal TestEngineMsgQueue(String Name)
        {
            mLogger = log4net.LogManager.GetLogger(this.GetType());
            mSenderBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            mReceiverBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            mName = Name;
            mSenderDone = new System.Threading.ManualResetEvent(false);
            mReceiverDone = new System.Threading.ManualResetEvent(false);
            mTestQueue = MsgQueueManager.CreateQueue(mName);

        }

        public int MsgIn
        {
            get
            {
                return mMsgIn;
            }
        }

        public int MsgOut
        {
            get
            {
                return mMsgOut;
            }
        }

        public virtual void StartTest()
        {
            
            mSenderBackgroundWorker.DoWork += new DoWorkEventHandler(SenderBackgroundWorker_DoWork);
            mSenderBackgroundWorker.WorkerSupportsCancellation = true;
            mSenderBackgroundWorker.WorkerReportsProgress = true;


            mReceiverBackgroundWorker.DoWork += new DoWorkEventHandler(ReceiverBackgroundWorker_DoWork);
            mReceiverBackgroundWorker.WorkerSupportsCancellation = true;
            mReceiverBackgroundWorker.WorkerReportsProgress = true;

            
            
            mSenderBackgroundWorker.RunWorkerAsync();
            mReceiverBackgroundWorker.RunWorkerAsync();
        }

        public virtual void StopTest()
        {
            if (mSenderBackgroundWorker.IsBusy)
            {
                mSenderBackgroundWorker.CancelAsync();
                mSenderDone.WaitOne();
            }
            if (mReceiverBackgroundWorker.IsBusy)
            {
                mReceiverBackgroundWorker.CancelAsync();
                mReceiverDone.WaitOne();
            }

            if (mTestQueue != null)
                mTestQueue.Dispose();

        }

        private void SenderBackgroundWorker_DoWork(object? sender, DoWorkEventArgs? e)
        {
            Random Rnd = new Random();

            while (!mSenderBackgroundWorker.CancellationPending)
            {
                Msg m;
                
                if (Rnd.Next(100) <= 25)
                {
                    m = new Msg(mMsgIn++);
                    //mLogger.DebugFormat("Send {0}", m.ToString());
                    mTestQueue.Send(m);
                    mSenderBackgroundWorker.ReportProgress(100-(mMsgOut * 100) / mMsgIn);
                }

                if (Rnd.Next(100) <= 25)
                {
                    m = new Msg(mMsgIn++);
                    //mLogger.DebugFormat("SendUrgent {0}", m.ToString());
                    mTestQueue.SendUrgent(m);
                    mSenderBackgroundWorker.ReportProgress(100-(mMsgOut * 100)/ mMsgIn);
                }

                if (Rnd.Next(100) <= 25)
                {
                    System.Threading.Thread.Sleep(Rnd.Next(100));
                }
            }
            mSenderDone.Set();
        }

        private void ReceiverBackgroundWorker_DoWork(object? sender, DoWorkEventArgs? e)
        {
            Random Rnd = new Random();

            while (!mSenderBackgroundWorker.CancellationPending)
            {
                try
                {
                    Msg? m = mTestQueue.Receive(100);
                    //mLogger.DebugFormat("Received {0}", m.ToString());
                    mMsgOut++;

                    mReceiverBackgroundWorker.ReportProgress(100-(mMsgOut * 100) / mMsgIn);
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(Rnd.Next(250, 1000));
                }
            }
            mReceiverDone.Set();
        }
    }
}
