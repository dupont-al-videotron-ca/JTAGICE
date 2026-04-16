using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JTAGICEmkII;
using JTAGICEmkII.Master;
using MyFramework;
using MyFramework.Threading;

namespace JTAGICEmkII.HostService
{
    public class CommandRequest<S, R> : IDisposable
        where S : class // send command type
        where R : class // receive response type
    {

        #region Constructors 
        public CommandRequest(S command, IActivityComElement activityElement) : this(command, activityElement, TimeSpan.FromSeconds(45))
        {
        }
        public CommandRequest(S command, IActivityComElement activityElement, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNull(activityElement);
            Command = command ?? throw new ArgumentNullException(nameof(command));
            this._timeout = timeout;
            Response = null!;
            _waitHandle = new ManualResetEvent(false);
            CommandElement = activityElement;
        }


        #endregion


        #region Fields 

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

        public void ReceivedResponse(R response)
        {
            ArgumentNullException.ThrowIfNull(response);
            _waitHandle.Set();
            Response = response;
        }

        public bool WaitForResponse()
        {
            RetryCount--;
            _timeoutOccured = _waitHandle.WaitOne(_timeout);
            return _timeoutOccured;
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
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CommandRequest()
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
