using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Moq;
using Xunit;

namespace Common.Test.Xunit
{
    /// <summary>
    /// Represents the base class of all XUnit test
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class XUnitTestBase : IDisposable
    {
        private readonly XUnitTestAppender _xUnitTestAppender;
        private readonly log4net.Layout.PatternLayout _patternLayout = new log4net.Layout.PatternLayout("%date %-5level %30.30logger, %message%newline");
        private bool _disposed;

        protected ILog Logger { get; private set; }

        protected bool VerifyLogForError { get; set; } = true;

        protected bool VerifyMockSetup { get; set; } = true;

        protected MockRepository MockRepository { get; } = new MockRepository(MockBehavior.Strict);

        protected bool VerifyLogForWarning { get; set; } = false;

        public IEnumerable<LoggingEvent> LoggedEvents => _xUnitTestAppender.LoggedEvents;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitTestBase"/> class.
        /// </summary>
        protected XUnitTestBase(bool appendToFile = false)
        {
            Logger = LogManager.GetLogger(this.GetType());
            Logger loggerImpl = (Logger)Logger.Logger ;
            loggerImpl.Level = log4net.Core.Level.All;

            Hierarchy logRepository = (Hierarchy)LogManager.GetRepository(Assembly.GetCallingAssembly());
            XmlConfigurator.ConfigureAndWatch(logRepository, new FileInfo("log4net.config"));
            RootLogger rootlogger = (RootLogger)logRepository.Root;
            FileAppender? fileAppender = (FileAppender?)rootlogger.GetAppender("FileAppender");
            if (fileAppender == null)
            {
                throw new InvalidOperationException("FileAppender is not configured in log4net.config");
            }

            // file appender
            //var appender = new FileAppender();
            //appender.Name = "FileAppender";
            //appender.File = "log.txt";
            //appender.AppendToFile = appendToFile;
            //appender.Layout = _patternLayout;
            //appender.ActivateOptions();

            // xunit appender
            XUnitTestAppender? appender = (XUnitTestAppender?)rootlogger.GetAppender("XUnitTestAppender");
            if(appender == null)
            {
                throw new InvalidOperationException("XUnitTestAppender is not configured in log4net.config");
            }

            _xUnitTestAppender = appender;

            //_xUnitTestAppender.Name = "XUnitTestAppender";
            //_xUnitTestAppender.Layout = _patternLayout;
            //_xUnitTestAppender.ActivateOptions();

            Logger.Info("----- Starting test: " + this.GetType().Name + " -----");

        }

        // Public implementation of IDisposable pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Finalizer as a safety net. Only needed if unmanaged resources are present,
        // but provided here because this is a base class that may be extended.
        ~XUnitTestBase()
        {
            Dispose(false);
        }

        // Core dispose logic. When disposing == true, release managed resources.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                CheckBeforeDispose();
                Logger.Info("----- Ending test: " + this.GetType().Name + " -----");
                _xUnitTestAppender.Close();

                Logger? rootLogger = (Logger)LogManager.GetLogger("root").Logger;
                rootLogger?.Repository?.Shutdown();
            }

            // free unmanaged resources here (none in this class)
            _disposed = true;
        }

        private void CheckBeforeDispose()
        {
            if (VerifyLogForError)
            {

                Assert.False(_xUnitTestAppender.IsLoggingEventsGt(log4net.Core.Level.Error));
            }

            if (VerifyLogForWarning)
            {
                Assert.False(_xUnitTestAppender.IsLoggingEventsEq(log4net.Core.Level.Warn));
            }

            if (VerifyMockSetup)
                MockRepository.VerifyAll();
        }
    }
}