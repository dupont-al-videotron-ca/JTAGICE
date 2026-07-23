using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Queue
{
    /// <summary>
    /// Class implemeting the Chain of resposability pattern for the Message handler used by the 
    /// message reactor.
    /// </summary>
    public class MsgChainOfResponsability : IMsgHandler, IDisposable
    {
        private IMsgHandlerCollection mSuccessors;
        private System.Threading.Mutex mLock;
        private bool disposedValue;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="First">The first successor.</param>
        public MsgChainOfResponsability(IMsgHandler First)
        {
            mSuccessors = new IMsgHandlerCollection();
            mLock = new System.Threading.Mutex();
            AddHandler(First);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Handlers">List of successor handlers.</param>
        public MsgChainOfResponsability(IMsgHandler[] Handlers)
        {
            mSuccessors = new IMsgHandlerCollection();
            AddRangeHandlers(Handlers);
            mLock = new System.Threading.Mutex();
        }

        /// <summary>
        /// Disposed object
        /// </summary>
        public void Dispose2()
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mSuccessors.Clear();
            }
        }

        /// <summary>
        /// Add a message handler.
        /// </summary>
        /// <param name="Handler">Message handler.</param>
        public void AddHandler(IMsgHandler Handler)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mSuccessors.Add(Handler);
            }
        }

        /// <summary>
        /// Add a range of message handler.
        /// </summary>
        /// <param name="Handler">Message handler.</param>
        public void AddRangeHandlers(IMsgHandler[] Handlers)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mSuccessors.AddRange(Handlers);
            }
        }

        /// <summary>
        /// Remove a message handler.
        /// </summary>
        /// <param name="Handler">Message handler to remove.</param>
        public void RemoveHandler(IMsgHandler Handler)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mSuccessors.Remove(Handler);
            }
        }

        /// <summary>
        /// Remove all message handlers except the Handler.
        /// </summary>
        /// <param name="Handler">Message handler keep.</param>
        public void Clear(IMsgHandler Handler)
        {
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                mSuccessors.Clear();
                mSuccessors.Add(Handler);
            }
        }

        /// <summary>
        /// Handler of message.
        /// </summary>
        /// <param name="source">Queue from where the message has been read bu the message reactor.</param>
        /// <param name="m">The message.</param>
        /// <returns>Returns true when the message has been analysed.</returns>
        public bool HandleMsg(MsgQueue source, Msg? m)
        {
            // copy list of handlers
            IMsgHandlerCollection Successors = new IMsgHandlerCollection();
            using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
            {
                Successors.AddRange(mSuccessors.ToArray());
            }

            // call handler until a handler comsume the message.
            foreach (IMsgHandler handler in Successors)
            {
                // Is message has been compsumed by the handler ?
                if (handler.HandleMsg(source, m))
                {
                    // Yes,
                    return true;
                }
                // else, no continue
            }
            
            // Message not for use!
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    using (MyFramework.Threading.LockMutex ml = new MyFramework.Threading.LockMutex(mLock))
                    {
                        mSuccessors.Clear();
                    }
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
    }


}
