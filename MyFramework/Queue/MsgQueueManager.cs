using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyFramework.Queue
{
    internal class MsgQueueList : List<MsgQueue>
    {
    }

    internal class MsgQueueNameMap : Dictionary<string, MsgQueue>
    {

    }

    internal class MsgQueuePriorityMap : Dictionary<MsgQueuePriority, MsgQueueList>
    {
    }

    /// <summary>
    /// Facade class for message queue manager.
    /// Provides factory methods to create MsgQueue.
    /// Provides factory methods to create MsgQueueReactor.
    /// Provides methods to broadcast message to all queues.
    /// 
    /// </summary>
    public class MsgQueueManager 
    {
        // Singleton
        internal static Object gMsgManagerLock = new object();
        internal static MsgQueueManager gMsgManager = new MsgQueueManager();


        /// <summary>
        /// Log4Net mLogger.
        /// </summary>
        private log4net.ILog mLog;
        private MsgQueuePriorityMap mPriorityList;
        private MsgQueueNameMap mNamedList;
        private Mutex mLock;
        private MsgLFThreadSet mLeaderFollowerThreadSet;

        /// <summary>
        /// Constructor
        /// </summary>
        private MsgQueueManager()
        {
            mLog = log4net.LogManager.GetLogger(GetType());

            mPriorityList = new MsgQueuePriorityMap();
            mNamedList = new MsgQueueNameMap();
            mLock = new Mutex();
            mLeaderFollowerThreadSet = new MsgLFThreadSet();
        }

        /// <summary>
        /// Gets the singleton of the QueueManager
        /// </summary>
        /// <returns></returns>
        private static MsgQueueManager Instance
        {
            get
            {
                lock (gMsgManagerLock)
                {
                    return gMsgManager;
                }
            }
        }

        /// <summary>
        /// Gets the Leader / follower.
        /// </summary>
        public static MsgLFThreadSet LFThreadSet
        {
            get
            {
                return Instance.mLeaderFollowerThreadSet;
            }
        }

        /// <summary>
        /// Creates a message queue and registers the queue to receive broacasted messages.
        /// </summary>
        /// <param name="Name">Name of the queue</param>
        /// <param name="Priority">Priority of the queue</param>
        /// <param name="RegisterQueue">When true register the queue</param>
        /// <returns>MsgQueue</returns>
        private MsgQueue CreateQueue_Internal(string Name, MsgQueuePriority Priority, bool RegisterQueue)
        {

            MsgQueue MqObj;
            MqObj = new MsgQueue(Name, Priority);

            if (RegisterQueue)
                RegisterQueue_Internal(MqObj);

            mLog.DebugFormat("Queue Created {0}, {1}", new Object[] { Name, Priority });
            return MqObj;
        }

        /// <summary>
        /// Create and register a message queue.
        /// </summary>
        /// <param name="HandlerMsg">Message handler</param>
        /// <param name="Name">Name of the queue</param>
        /// <param name="Priority">Priority of the queue</param>
        /// <returns>MsgQueue</returns>
        private MsgQueueReactor CreateQueueReactor_Internal(IMsgHandler HandlerMsg, string Name, MsgQueuePriority Priority)
        {

            MsgQueueReactor MqObj;
            MqObj = new MsgQueueReactor(Name, Priority);
            RegisterQueue_Internal(MqObj);

            MqObj.RegisterHandler(HandlerMsg);

            mLog.DebugFormat("Queue Created {0}, {1}", new Object[] { Name, Priority });
            return MqObj;
        }


        private MsgQueue this[String Name]
        {
            get
            {
                using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
                {
                    return mNamedList[Name];
                }
            }
        }

        /// <summary>
        /// Register the Queue into the priority list and nanmed list.
        /// </summary>
        /// <param name="Queue">Message queue to registered</param>
        private void RegisterQueue_Internal(MsgQueue Queue)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                // Is queue has a name ?
                if (Queue.Name != String.Empty)
                    // Yes, add it to the named list.
                    mNamedList.Add(Queue.Name, Queue);

                // Is priority map exist ?
                if (!mPriorityList.ContainsKey(Queue.Priority))
                    // No, create it
                    mPriorityList.Add(Queue.Priority, new MsgQueueList());

                // Add queue to the priority list.
                mPriorityList[Queue.Priority].Add(Queue);
                mLog.DebugFormat("Queue registred {0}, {1}", new Object[] { Queue.Name, Queue.Priority });
            }
        }

        /// <summary>
        /// Unregister the Queue from the priority list and nanmed list.
        /// </summary>
        /// <param name="Queue">Message queue to registered</param>
        private void UnregisterQueue_Internal(MsgQueue Queue)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                // Is queue has a name ?
                if (Queue.Name != String.Empty)
                    // Yes, remove it to the named list.
                    mNamedList.Remove(Queue.Name);

                // remove queue to the priority list.
                if (mPriorityList.ContainsKey(Queue.Priority))
                {
                    mPriorityList[Queue.Priority].Remove(Queue);
                    mLog.DebugFormat("Queue unregistred {0}, {1}", new Object[] { Queue.Name, Queue.Priority });
                }
            }
        }

        /// <summary>
        /// Send message to a list of queues
        /// </summary>
        /// <param name="QueueList"></param>
        /// <param name="msg"></param>
        private void Send_Internal(MsgQueueList QueueList, Msg msg)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (MsgQueue Queue in QueueList)
                {
                    Queue.Send(msg);
                }
            }
        }

        /// <summary>
        /// Send message 
        /// </summary>
        /// <param name="msg">Message to send</param>
        private void Send_Internal(Msg msg)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (KeyValuePair<MsgQueuePriority, MsgQueueList> PrioList in mPriorityList)
                {
                    Send_Internal(PrioList.Value, msg);
                }
            }
        }

        /// <summary>
        /// Send message 
        /// </summary>
        /// <param name="msg">Message to send</param>
        private void Send_Internal(String Name, Msg msg)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                // Is a queue has been registered ?
                if (mNamedList.ContainsKey(Name))
                {
                    // Yes, send message to the queue
                    mNamedList[Name].Send(msg);
                }
                // else, no, nothing to do
            }
        }

        /// <summary>
        /// Send message 
        /// </summary>
        /// <param name="msg">Message to send</param>
        private void Send_Internal(MsgQueuePriority Prio, Msg msg)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                // Is a queue priority has been registered ?
                if (mPriorityList.ContainsKey(Prio))
                {
                    // Yes, send message to all queue
                    Send_Internal(mPriorityList[Prio], msg);
                }
                // else, no, nothing to do
            }
        }

        /// <summary>
        /// Send message to the head of the queues
        /// </summary>
        /// <param name="msg">Message to send</param>
        private void SendUrgent_Internal(MsgQueueList QueueList, Msg msg)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (MsgQueue Queue in QueueList)
                {
                    Queue.SendUrgent(msg);
                }
            }
        }

        /// <summary>
        /// Send message to the head of the queue
        /// </summary>
        /// <param name="msg">Message to send</param>
        private void SendUrgent_Internal(Msg msg)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                foreach (KeyValuePair<MsgQueuePriority, MsgQueueList> PrioList in mPriorityList)
                {
                    SendUrgent_Internal(PrioList.Value, msg);
                }
            }
        }

        /// <summary>
        /// Send message to the head of the queue
        /// </summary>
        /// <param name="msg">Message to send</param>
        private void SendUrgent_Internal(String Name, Msg msg)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                // Is a queue has been registered ?
                if (mNamedList.ContainsKey(Name))
                {
                    // Yes, send message to the queue
                    mNamedList[Name].SendUrgent(msg);
                }
                // else, no, nothing to do
            }
        }

        /// <summary>
        /// Send message to the head of the queue
        /// </summary>
        /// <param name="msg">Message to send</param>
        private void SendUrgent_Internal(MsgQueuePriority Prio, Msg msg)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                // Is a queue priority has been registered ?
                if (mPriorityList.ContainsKey(Prio))
                {
                    // Yes, send message to all queue
                    SendUrgent_Internal(mPriorityList[Prio], msg);
                }
                // else, no, nothing to do
            }
        }

        /// <summary>
        /// Register the Queue into the priority list and named list.
        /// </summary>
        /// <param name="Queue">Message queue to registered</param>
        public static void RegisterQueue(MsgQueue Queue)
        {
            Instance.RegisterQueue_Internal(Queue);
        }

        /// <summary>
        /// Unrgister the Queue into the priority list and named list.
        /// </summary>
        /// <param name="Queue">Message queue to registered</param>
        public static void UnregisterQueue(MsgQueue Queue)
        {
            Instance.UnregisterQueue_Internal(Queue);
        }

        /// <summary>
        /// Creates a message queue and registers the queue to receive all sent messages.
        /// </summary>
        /// <returns>MsgQueue</returns>
        public static MsgQueue CreateQueue()
        {
            return Instance.CreateQueue_Internal(String.Empty, MsgQueuePriority.QueuePriorityNormal, true);
        }
        /// <summary>
        /// Create a named message queue and registers the queue to receive all sent messages.
        /// </summary>
        /// <param name="Name">Name of the mesage Queue</param>
        /// <returns>MsgQueue</returns>
        public static MsgQueue CreateQueue(string Name)
        {
            return Instance.CreateQueue_Internal(Name, MsgQueuePriority.QueuePriorityNormal, true);
        }

        /// <summary>
        /// Create a prioterise message queue and registers the queue to receive all sent messages.
        /// </summary>
        /// <param name="Prio">Priority of the mesage Queue</param>
        /// <returns>MsgQueue</returns>
        public static MsgQueue CreateQueue(MsgQueuePriority Prio)
        {
            return Instance.CreateQueue_Internal(String.Empty, Prio, true);
        }

        /// <summary>
        /// Create a named and prioterise message queue and registers the queue to receive all sent messages.
        /// </summary>
        /// <param name="Name">Name of the mesage Queue</param>
        /// <param name="Prio">Priority of the mesage Queue</param>
        /// <returns>MsgQueue</returns>
        public static MsgQueue CreateQueue(string Name, MsgQueuePriority Prio)
        {
            return Instance.CreateQueue_Internal(Name, Prio, true);
        }

        /// <summary>
        /// Create a named and prioterise message defer queue.
        /// </summary>
        /// <param name="Name">Name of the mesage defer Queue</param>
        /// <returns>MsgQueue</returns>
        public static MsgQueue CreateDeferQueue(string Name)
        {
            return Instance.CreateQueue_Internal(Name, MsgQueuePriority.QueuePriorityNormal, false);
        }

        /// <summary>
        /// Create a message queue reactor that have a notifier mechanism for received message and registers the queue to receive all sent messages.
        /// </summary>
        /// <param name="HandlerMsg">The message handler.</param>
        /// <returns>MsgQueue</returns>
        public static MsgQueueReactor CreateReactorQueue(IMsgHandler HandlerMsg)
        {
            return Instance.CreateQueueReactor_Internal(HandlerMsg, String.Empty, MsgQueuePriority.QueuePriorityNormal);
        }
        /// <summary>
        /// Create a message queue with a reactor has a notifier mechanism for received message and registers the queue to receive all sent messages.
        /// </summary>
        /// <param name="HandlerMsg">The message handler.</param>
        /// <param name="Name">Name of the mesage Queue</param>
        /// <returns>MsgQueue</returns>
        public static MsgQueueReactor CreateReactorQueue(IMsgHandler HandlerMsg, string Name)
        {
            return Instance.CreateQueueReactor_Internal(HandlerMsg, Name, MsgQueuePriority.QueuePriorityNormal);
        }

        /// <summary>
        /// Create a message queue with a reactor has a notifier mechanism for received message and registers the queue to receive all sent messages.
        /// </summary>
        /// <param name="HandlerMsg">The message handler.</param>
        /// <param name="Prio">Priority of the mesage Queue</param>
        /// <returns>MsgQueue</returns>
        public static MsgQueueReactor CreateReactorQueue(IMsgHandler HandlerMsg, MsgQueuePriority Prio)
        {
            return Instance.CreateQueueReactor_Internal(HandlerMsg, String.Empty, Prio);
        }

        /// <summary>
        /// Create a message queue with a reactor has a notifier mechanism for received message and registers the queue to receive all sent messages.
        /// </summary>
        /// <param name="HandlerMsg">The message handler.</param>
        /// <param name="Name">Name of the mesage Queue</param>
        /// <param name="Prio">Priority of the mesage Queue</param>
        /// <returns>MsgQueue</returns>
        public static MsgQueueReactor CreateReactorQueue(IMsgHandler HandlerMsg, string Name, MsgQueuePriority Prio)
        {
            return Instance.CreateQueueReactor_Internal(HandlerMsg, Name, Prio);
        }

        /// <summary>
        /// Sends (Broadcast) the message to all registered message queue.
        /// </summary>
        /// <param name="msg">Mesasge ti send.</param>
        public static void Broadcast(Msg msg)
        {
            Instance.Send_Internal(msg);
        }

        /// <summary>
        /// Sends (Broadcast) the message to all registered message queue with the desired priority.
        /// </summary>
        /// <param name="Prio"></param>
        /// <param name="msg">Mesasge ti send.</param>
        public static void Broadcast(MsgQueuePriority Prio, Msg msg)
        {
            Instance.Send_Internal(Prio, msg);
        }

        /// <summary>
        /// Sends the message to the named message queue.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="msg">Mesasge ti send.</param>
        public static void Broadcast(String Name, Msg msg)
        {
            Instance.Send_Internal(Name, msg);
        }
        /// <summary>
        /// Sends (Broadcast) the urgent message to all registered message queue.
        /// </summary>
        /// <param name="msg">Mesasge ti send.</param>
        public static void BroadcastUrgent(Msg msg)
        {
            Instance.SendUrgent_Internal(msg);
        }

        /// <summary>
        /// Sends (Broadcast) the urgent message to all registered message queue with the desired priority.
        /// </summary>
        /// <param name="Prio"></param>
        /// <param name="msg">Mesasge ti send.</param>
        public static void BroadcastUrgent(MsgQueuePriority Prio, Msg msg)
        {
            Instance.SendUrgent_Internal(Prio, msg);
        }

        /// <summary>
        /// Sends the urgent message to the named message queue.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="msg">Mesasge ti send.</param>
        public static void BroadcastUrgent(String Name, Msg msg)
        {
            Instance.SendUrgent_Internal(Name, msg);
        }

        /// <summary>
        /// Gets the message queue
        /// </summary>
        /// <param name="Name">Name of the Queue</param>
        /// <returns></returns>
        public static MsgQueue Queue(String Name)
        {
            return Instance[Name];
        }

    }
}
