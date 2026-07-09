using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JTAGICEmkII;
using JTAGICEmkII.Slave;
using log4net;
using MyFramework;
using MyFramework.Threading;

namespace JTAGICEmkII.HostService
{

    public class CommandRequestBase<S, R> : ICommandRequest, ICommandRequest<S, R>, IDisposable
        where S : class // send command type
        where R : class // receive response type
    {

        #region Constructors 
        public CommandRequestBase(S command, IActivityComElement activityElement) : this(command, activityElement, TimeSpan.FromSeconds(45))
        {
        }
        public CommandRequestBase(S command, IActivityComElement activityElement, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNull(activityElement);
            Command = command ?? throw new ArgumentNullException(nameof(command));
            this._timeout = timeout;
            Response = null!;
            _waitHandle = new ManualResetEvent(false);
            CommandElement = activityElement;
            Logger = LogManager.GetLogger(this.GetType());
        }


        #endregion


        #region Fields 

        private ILog Logger;
        private ManualResetEvent _waitHandle;

        private bool disposedValue;
        private TimeSpan _timeout;
        private bool _timeoutOccured;


        #endregion


        #region Properties 
        public S Command { get; private set; }
        public R? Response { get; private set; }

        public int RetryCount { get; set; } = 3;

        public bool IsRequestTimeout
        {
            get
            {
                return _timeoutOccured && RetryCount <= 0;
            }
        }

        internal IActivityComElement CommandElement { get; private set; }

        #endregion

        #region Public Methods 

        public bool WaitForResponse()
        {
            if (_waitHandle.WaitOne(_timeout))
            {
                return true;
            }
            else
            {
                RetryCount--;
                _timeoutOccured = true;
                return false;
            }
        }

        internal void ReceivedResponse(R response)
        {
            ArgumentNullException.ThrowIfNull(response);
            Response = response;
            _timeoutOccured = false;
            _waitHandle.Set();
        }

        #endregion


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _waitHandle.Close();
                    _waitHandle.Dispose();
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
