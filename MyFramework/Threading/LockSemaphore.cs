namespace MyFramework.Threading
{
    public sealed class LockSemaphore : IDisposable
    {
        private Semaphore mSema;
        private bool disposedValue;

        public LockSemaphore(Semaphore m, int Timeout)
        {
            mSema = m;
            _WaitOne(Timeout);
        }

        public LockSemaphore(Semaphore m)
        {
            mSema = m;
            _WaitOne();
        }

        /// <summary>
        /// Lock underlaying mutex
        /// </summary>
        private void _WaitOne()
        {
            try
            {
                mSema.WaitOne();
            }
            // Mutex abandon by owner thread
            catch (AbandonedMutexException)
            {
                mSema.WaitOne();
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
                TimeoutOccured = !mSema.WaitOne(Timeout);
            }
            // Mutex abandon by owner thread
            catch (AbandonedMutexException)
            {
                TimeoutOccured = !mSema.WaitOne(Timeout);
            }

            if (TimeoutOccured)
                throw new System.TimeoutException();
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    mSema.Release();
                    mSema.Dispose();
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
