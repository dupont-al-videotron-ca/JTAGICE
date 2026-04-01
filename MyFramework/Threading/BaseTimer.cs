using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Threading
{
    /// <summary>
    /// Base class for MyFramework's timers.
    /// </summary>
    public abstract class BaseTimer : IDisposable
    {
        protected bool mStopped;
        protected bool mExpired;

        /// <summary>
        /// The timer.
        /// </summary>
        protected System.Threading.Timer mTimer;

        /// <summary>
        /// Time in milliseconds.
        /// </summary>
        protected int mMsTimeValue;
        private bool disposedValue;

        /// <summary>
        /// Occurs when a the timer is active and expired.
        /// </summary>
        public event EventHandler? TimerExpired;

        protected BaseTimer()
        {
            mMsTimeValue = 0;
            mStopped = true;
            mTimer = new System.Threading.Timer(new System.Threading.TimerCallback(TimerExpirationCallBack));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MsTime">Time in milliseconds od the timer.</param>
        protected BaseTimer(int MsTime) : this()
        {
            mMsTimeValue = MsTime;
        }

        /// <summary>
        /// Return true when the timer is in stop state.
        /// </summary>
        public bool IsStopped
        {
            get
            {
                return mStopped;
            }
        }

        /// <summary>
        /// Return true when the timer has been expired been a Start and Stop.
        /// </summary>
        public bool IsExpired
        {
            get
            {
                return mExpired;
            }
        }


        /// <summary>
        /// Call event handler when timer expired.
        /// </summary>
        /// <param name="State"></param>
        private void TimerExpirationCallBack(Object? State)
        {
            mExpired = true;
            if (TimerExpired != null)
                TimerExpired(this, EventArgs.Empty);
        }

        /// <summary>
        /// Starts  / Restarts the timer.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Starts  / Restarts the timer with a new time value.
        /// </summary>
        /// <param name="NewTime">New start value to use for subsequent call to Start(), in milliseconds</param>
        public abstract void Start(int NewTime);

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public abstract void Stop();

        public override string ToString()
        {
            StringBuilder StrB = new StringBuilder();

            StrB.Append(base.ToString());
            StrB.Append(",");

            StrB.Append(mTimer.ToString());
            StrB.Append(",");

            StrB.Append(mMsTimeValue.ToString());
            StrB.Append(",");

            StrB.Append(mStopped.ToString());
            StrB.Append(",");

            StrB.Append(mExpired.ToString());

            return StrB.ToString();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop();
                    mTimer.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                disposedValue = true;
            }
        }

        // //  override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BaseTimer()
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
}
