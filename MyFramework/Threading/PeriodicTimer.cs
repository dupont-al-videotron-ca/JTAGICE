using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFramework.Threading
{
    /// <summary>
    /// Class wrapper to handle System.Threading.Timer in a peridoc timer.
    /// </summary>
    public class PeriodicTimer : BaseTimer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PeriodicTimer()
            : base()
        {

        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MsTime">Time in milliseconds od the timer.</param>
        public PeriodicTimer(int MsTime)
            : base(MsTime)
        {

        }

        /// <summary>
        /// Starts  / Restarts the timer.
        /// </summary>
        public override void Start()
        {
            mStopped = false;
            mTimer.Change(0, mMsTimeValue);
        }

        /// <summary>
        /// Starts  / Restarts the timer with a new time value.
        /// </summary>
        /// <param name="NewTime">New start value to use for subsequent call to Start(), in milliseconds</param>
        public override void Start(int NewTime)
        {
            mStopped = false;
            PeriodicTime = NewTime;
            Start();
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public override void Stop()
        {
            mStopped = true;
            mTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        }

        /// <summary>
        /// Gets / Sets the periodic time expiration value.
        /// </summary>
        public int PeriodicTime
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
