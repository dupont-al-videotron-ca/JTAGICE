using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Threading
{

    /// <summary>
    /// Class wrapper to handle System.Threading.Timer in a single shot timer.
    /// </summary>
    public class SingleShotTimer : BaseTimer
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleShotTimer():
            base()
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MsTime">Time in milliseconds od the timer.</param>
        public SingleShotTimer(int MsTime):
            base(MsTime)
        {
        }

        protected override void Dispose(bool disposing) => base.Dispose(disposing);

        /// <summary>
        /// Starts  / Restarts the timer.
        /// </summary>
        public override void Start()
        {
            mStopped = false;
            mExpired = false;
            mTimer.Change(mMsTimeValue, System.Threading.Timeout.Infinite);
        }

        /// <summary>
        /// Starts  / Restarts the timer with a new time value.
        /// </summary>
        /// <param name="NewTime">New start value to use for subsequent call to Start(), in milliseconds</param>
        public override void Start(int NewTime)
        {
            OneShotTime = NewTime;
            Start();
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public override void Stop()
        {
            if (!mStopped)
            {
                mStopped = true;
                mExpired = false;
                mTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            }
        }

        /// <summary>
        /// Gets / Sets the one shot time expiration value.
        /// </summary>
        public int OneShotTime
        {
            get
            {
                return mMsTimeValue;
            }
            set
            {
                mMsTimeValue = value;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
