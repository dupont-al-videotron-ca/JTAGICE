using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Core;

namespace Common.Test.Xunit
{
    /// <summary>
    /// Represent log appender for XUnit test in memory
    /// </summary>
    /// <seealso cref="log4net.Appender.AppenderSkeleton" />
    internal class XUnitTestAppender : log4net.Appender.AppenderSkeleton
    {
        private List<LoggingEvent> _loggingEvents;

        /// <summary>
        /// The logged events
        /// </summary>
        public IEnumerable<LoggingEvent> LoggedEvents;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitTestAppender"/> class.
        /// </summary>
        public XUnitTestAppender()
        {
            _loggingEvents = new List<LoggingEvent>();
            LoggedEvents = _loggingEvents;
        }

        /// <summary>
        /// Appends the specified logging event.
        /// </summary>
        /// <param name="loggingEvent">The logging event.</param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            _loggingEvents.Add(loggingEvent);
        }

        public bool IsLoggingEventsGt(Level level)
        {
            return _loggingEvents.Any(l => l.Level >= level);
        }

        public bool IsLoggingEventsEq(Level level)
        {
            return _loggingEvents.Any(l => l.Level == level);
        }

        public IEnumerable<LoggingEvent> GetLoggingEvents(Level level)
        {
            return _loggingEvents.Where(l => l.Level >= level);
        }
    }
}
