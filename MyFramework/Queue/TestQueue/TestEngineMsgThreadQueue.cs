using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using log4net;
using MyFramework.Queue;

namespace TestQueue
{
    public class TestEngineMsgThreadQueue: TestEngineMsgQueue 
    {
        internal class TestMsgQueueThread : MsgQueueThread
        {
            TestEngineMsgThreadQueue mEngine;
            Random mRdn;
            public TestMsgQueueThread(TestEngineMsgThreadQueue Engine)
            {
                mEngine = Engine;
                mRdn = new Random();
            }

            protected override void AnalyzeReceiveMsg(Msg? Message)
            {
                mEngine.mMsgOut++;
                mEngine.mSenderBackgroundWorker.ReportProgress(100 - (mEngine.mMsgOut * 100) / mEngine.mMsgIn);
                //System.Threading.Thread.Sleep(mRdn.Next(10, 500));
            }
        }

        private TestMsgQueueThread mQueueThread;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ParentForm"></param>
        internal TestEngineMsgThreadQueue(String Name)
            : base(Name)
        {
            mQueueThread = new TestMsgQueueThread(this);
        }

        public override void StartTest()
        {

            mTestQueue = mQueueThread.Queue;
            
            mSenderBackgroundWorker.DoWork += new DoWorkEventHandler(SenderBackgroundWorker_DoWork);
            mSenderBackgroundWorker.WorkerSupportsCancellation = true;
            mSenderBackgroundWorker.WorkerReportsProgress = true;

            mSenderBackgroundWorker.RunWorkerAsync();
        }

        public override void StopTest()
        {
            if (mSenderBackgroundWorker.IsBusy)
            {
                mSenderBackgroundWorker.CancelAsync();
                mSenderDone.WaitOne();
            }

            if (mQueueThread != null)
                mQueueThread.Dispose();

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
                    mLogger.Debug($"Send {m.ToString()}");
                    mTestQueue.Send(m);
                    mSenderBackgroundWorker.ReportProgress(100-(mMsgOut * 100) / mMsgIn);
                }

                if (Rnd.Next(100) <= 25)
                {
                    m = new Msg(mMsgIn++);
                    mLogger.Debug($"SendUrgent {m.ToString()}");
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

        internal void ReceiverBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Random Rnd = new Random();

            while (!mSenderBackgroundWorker.CancellationPending)
            {
                try
                {
                    Msg? m = mTestQueue.Receive(100);
                    mLogger.Debug($"Received {m}");
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
