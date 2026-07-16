using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyFramework.Threading
{
#if NETCF

    /// <summary>
    /// Class to manage interprocess / interthread synchronization (System.Threading.Mutex).
    /// 
    /// </summary>
    /// <remarks>Usage in a method:             
    ///     using (Robotronique.Threading.LockMutex l = new Robotronique.Threading.LockMutex(YourMutex))
    ///     {
    ///         // Your code
    ///     }
    /// </remarks>
    public sealed class LockMutex : IDisposable
    {
        private Mutex mMutex;
        private int mPopCount;

        /// <summary>
        /// Constructor, gets mutex
        /// </summary>
        /// <param name="m">The Mutex.</param>
        /// <param name="Timeout">Timeout to use</param>
        public LockMutex(Mutex m, int Timeout)
        {
            mMutex = m;
            _WaitOne(Timeout);
        }

        /// <summary>
        /// Constructor, gets mutex
        /// </summary>
        /// <param name="m">The Mutex.</param>
        public LockMutex(Mutex m)
        {
            mMutex = m;
            _WaitOne();
        }

        /// <summary>
        /// Dispose, release mutex
        /// </summary>
        public void Dispose()
        {
            mMutex.ReleaseMutex();
        }

        /// <summary>
        /// Unlock all recursive lock. Call PushLocks to restore recursive locks.
        /// </summary>
        /// <remarks>Use this method before calling any callback method. It will prevent dead lock with the mMutex.</remarks>
        public void PopLocks()
        {
            try
            {
                // Do the necessary actions before releasing the Mutex
                mPopCount = 0;
                
                while (true)
                {
                    mPopCount++;
                    mMutex.ReleaseMutex();
                }
            }
            catch (ApplicationException)
            {
                // End of Pop
                return;
            }
            catch (System.Exception)
            {
                // End of Pop
                return;
            }
        }

        /// <summary>
        /// Lock all recursive lock previously lock bu PopLocks.
        /// </summary>
        /// <remarks>Use this method after the call to any callback method.</remarks>
        public void PushLocks()
        {
            do
            {
                _WaitOne();
                mPopCount--;
                
                if (mPopCount <= 0)
                    throw new Robotronique.Exception.RbInternalException("mPopCount is negative");

            } while (mPopCount != 1);

        }


        /// <summary>
        /// Lock underlaying mutex
        /// </summary>
        private void _WaitOne()
        {
           mMutex.WaitOne();
        }

        /// <summary>
        /// Lock underlaying mutex
        /// </summary>
        /// <param name="Timeout">Time to wait</param>
        private void _WaitOne(int Timeout)
        {
            bool TimeoutOccured = false;
            TimeoutOccured = !mMutex.WaitOne(Timeout, false);

            if (TimeoutOccured)
            {
                throw new System.TimeoutException();
            }
        }
    }

#else

    /// <summary>
    /// Class to manage interprocess / interthread synchronization (System.Threading.Mutex).
    /// 
    /// </summary>
    /// <remarks>Usage in a method:             
    ///     using (MyFramework.Threading.LockMutex l = new MyFramework.Threading.LockMutex(YourMutex))
    ///     {
    ///         // Your code
    ///     }
    /// </remarks>
    public sealed class LockMutex : IDisposable
    {
        private Mutex mMutex;
        private int mPopCount;
        private bool disposedValue;

        /// <summary>
        /// Constructor, gets mutex
        /// </summary>
        /// <param name="m">The Mutex.</param>
        /// <param name="Timeout">Timeout to use</param>
        public LockMutex(Mutex m, int Timeout)
        {
            mMutex = m;
            _WaitOne(Timeout);
        }

        /// <summary>
        /// Constructor, gets mutex
        /// </summary>
        /// <param name="m">The Mutex.</param>
        public LockMutex(Mutex m)
        {
            mMutex = m;
            _WaitOne();
        }

        /// <summary>
        /// Dispose, release mutex
        /// </summary>
        public void Dispose2()
        {
            mMutex.ReleaseMutex();
        }

        /// <summary>
        /// Unlock all recursive lock. Call PushLocks to restore recursive locks.
        /// </summary>
        /// <remarks>Use this method before calling any callback method. It will prevent dead lock with the mMutex.</remarks>
        public void PopLocks()
        {
            try
            {
                // Do the necessary actions before releasing the Mutex
                mPopCount = 0;

                while (true)
                {
                    mPopCount++;
                    mMutex.ReleaseMutex();
                }
            }
            catch (ApplicationException)
            {
                // End of Pop
                return;
            }
            catch (System.Exception)
            {
                // End of Pop
                return;
            }
        }

        /// <summary>
        /// Lock all recursive lock previously lock bu PopLocks.
        /// </summary>
        /// <remarks>Use this method after the call to any callback method.</remarks>
        public void PushLocks()
        {
            do
            {
                _WaitOne();
                mPopCount--;

                if (mPopCount <= 0)
                    throw new MyFramework.Exceptions.MyInternalException("mPopCount is negative");

            } while (mPopCount != 1);

        }


        /// <summary>
        /// Lock underlaying mutex
        /// </summary>
        private void _WaitOne()
        {
            try
            {
                mMutex.WaitOne();
            }
            // Mutex abandon by owner thread
            catch (AbandonedMutexException)
            {
                mMutex.WaitOne();
            }
        }

        /// <summary>
        /// Lock underlaying mutex
        /// </summary>
        /// <param name="Timeout">Time to wait</param>
        private void _WaitOne(int Timeout)
        {
            bool TimeoutOccured = false;
            try
            {
                TimeoutOccured = !mMutex.WaitOne(Timeout);

            }
            // Mutex abandon by owner thread
            catch (AbandonedMutexException)
            {
                TimeoutOccured = !mMutex.WaitOne(Timeout);
            }

            if (TimeoutOccured)
            {
                throw new System.TimeoutException();
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    mMutex.ReleaseMutex();
                    mMutex.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                disposedValue = true;
            }
        }

        // // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LockMutex()
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
    }

#endif
    
    

}
