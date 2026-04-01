using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MyFramework.Threading
{
    /// <summary>
    /// Implementation of a Stop watch
    /// </summary>
    public class StopWatch
    {
        #region Members

        private Stopwatch mWinStopWatch;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public StopWatch()
        {
            mWinStopWatch = new Stopwatch();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="StartNow">When true start the stop watch immediatly</param>
        public StopWatch(bool StartNow)
        {
            mWinStopWatch = new Stopwatch();
            if (StartNow)
                Start();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the time span.
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get
            {
                return mWinStopWatch.Elapsed;
            }
        }

        /// <summary>
        /// Gets the time span in milliseconds.
        /// </summary>
        public double MsTime
        {
            get
            {
                return mWinStopWatch.ElapsedMilliseconds;
            }
        }

        #endregion

        #region Methods

        public void Reset()
        {
            mWinStopWatch.Reset();
        }

        public void Start()
        {
            mWinStopWatch.Start();
        }

        /// <summary>
        /// Start the stop watch
        /// </summary>
        /// <returns></returns>
        public TimeSpan Stop()
        {
            mWinStopWatch.Stop();

            return ElapsedTime;
        }

        /// <summary>
        /// Save a lap time
        /// </summary>
        /// <returns></returns>
        public TimeSpan Lap()
        {
            return mWinStopWatch.Elapsed;
        }

        public override string ToString()
        {
            return ElapsedTime.TotalMilliseconds.ToString("F0") + " ms";
        }
        #endregion


    }
}
