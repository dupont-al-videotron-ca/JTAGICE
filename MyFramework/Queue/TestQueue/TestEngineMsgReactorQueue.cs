#pragma warning disable CS1591, CS8618
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using log4net;
using MyFramework.Queue;

namespace TestQueue
{
    public class TestEngineMsgReactorQueue : TestEngineMsgQueue
    {
        internal class TestMsgQueueHandler : IMsgHandler
        {
            TestEngineMsgReactorQueue mEngine;
            int mId;
            Random mRdn;
            ILog mLogger = LogManager.GetLogger(typeof(TestMsgQueueHandler));
            public TestMsgQueueHandler(TestEngineMsgReactorQueue Engine, int Id)
            {
                mEngine = Engine;
                mRdn = new Random();
                mId = Id;
            }

            public bool HandleMsg(MsgQueue source, Msg? m)
            {
                bool Ret = mRdn.Next(0, 1) == 1;
                if (Ret)
                    mEngine.mMsgOut += NbHandler - mId;
                else
                    mEngine.mMsgOut++;

                mLogger.Debug($"HandleMsg_{mId} {m}");

                System.Threading.Thread.Sleep(mRdn.Next(1, 100));
//                System.Threading.Thread.Sleep(100);
//                return mRdn.Next(0, 1) == 1;
                return Ret;
            }
        }


        internal class TestMsgQueueHandlerList : List<TestMsgQueueHandler>
        {
        }

        internal class TestQ2
        {
            private MsgQueueReactor mQreactor;
            TestMsgQueueHandlerList mHandlerList;
            TestEngineMsgReactorQueue mEngine;
            MsgChainOfResponsability mChain;
            String mName;
            int mId;

            public TestQ2(String Name, TestEngineMsgReactorQueue Engine, int Id)
            {
                mName = Name;
                mId = Id;

                mHandlerList = new TestMsgQueueHandlerList();
                mEngine = Engine;
            }

            public MsgQueue Queue
            {
                get
                {
                    return mQreactor;
                }

            }

            public void StartTest()
            {
                MsgId[] Filters = new MsgId[LastMsgId];
                for (int j = 0; j < LastMsgId; j++)
                {
                    Filters[j] = new MsgId(FirstMsgId + j);
                }
                
                int i = 0;
                TestMsgQueueHandler Handler = new TestMsgQueueHandler(mEngine, i++);
                mHandlerList.Add(Handler);

                mChain = new MsgChainOfResponsability(Handler);
                mQreactor = MsgQueueManager.CreateReactorQueue(mChain , mName + mId.ToString());
                mQreactor.Open();

                for (; i < NbHandler; i++)
                {
                    Handler = new TestMsgQueueHandler(mEngine, i);
                    mChain.AddHandler(Handler);
                }
            }

            public void StopTest()
            {
                foreach (TestMsgQueueHandler Item in mHandlerList)
                {
                    mQreactor.UnregisterHandler(Item);
                }

                mQreactor.Close();
                mHandlerList.Clear();
            }
        }

        internal class TestQ2List : List<TestQ2>
        {
        }

        internal class QSenderList : List<MsgQueue>
        {
        }
        const int NbHandler = 10;
        const int FirstMsgId = 1;
        const int LastMsgId = 100 * NbHandler;
        const int NbQueue = 10;

        TestQ2List mQ2List;
        QSenderList mQSenderList;
        int mCurrentId;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ParentForm"></param>
        internal TestEngineMsgReactorQueue(String Name)
            : base(Name)
        {
            mQ2List = new TestQ2List();
            mQSenderList = new QSenderList();
            mCurrentId = FirstMsgId;
        }

        public override void StartTest()
        {

            for (int i = 0; i < NbQueue; i++)
            {
                TestQ2 Q2 = new TestQ2(mName, this, i);
                mQ2List.Add(Q2);
                mQSenderList.Add(Q2.Queue);
                Q2.StartTest();
            }

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

            mQSenderList.Clear();

            foreach (TestQ2 Item in mQ2List)
            {
                Item.StopTest();
            }

            mQ2List.Clear();

        }

        private void SenderBackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            Random Rnd = new Random();
            System.Threading.Thread.CurrentThread.Name = "Sender_Thread";
            System.Threading.Thread.Sleep(1000);
            while (!mSenderBackgroundWorker.CancellationPending)
            {
                foreach (MsgQueue TestQueue in mQSenderList)
                {
#if true
                    Msg m;

                    if (Rnd.Next(100) <= 25)
                    {
                        m = new Msg(mCurrentId++);
                        if (mCurrentId >= LastMsgId)
                            mCurrentId = FirstMsgId;

                        mMsgIn += NbHandler;

                        //mLogger.DebugFormat("Send {0}", m.ToString());
                        TestQueue.Send(m);
                        mSenderBackgroundWorker.ReportProgress(100 - (mMsgOut * 100) / mMsgIn);
                    }

                    if (Rnd.Next(100) <= 25)
                    {
                        m = new Msg(mCurrentId++);
                        if (mCurrentId >= LastMsgId)
                            mCurrentId = FirstMsgId;

                        mMsgIn += NbHandler;
                        //mLogger.DebugFormat("SendUrgent {0}", m.ToString());
                        TestQueue.SendUrgent(m);
                        mSenderBackgroundWorker.ReportProgress(100 - (mMsgOut * 100) / mMsgIn);
                    }

                    if (Rnd.Next(100) <= 50)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
#else
                        System.Threading.Thread.Sleep(100);

#endif
                }
            }

            mSenderDone.Set();
        }

    }
}
