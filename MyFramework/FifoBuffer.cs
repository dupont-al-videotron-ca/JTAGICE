using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFramework
{
    public class FifoBuffer<T> : IEnumerable<T>,
        ICollection<T>,
        IReadOnlyCollection<T>,
        IDisposable
    {

        #region Constructors 

        public FifoBuffer()
        {
            _buffer = new Queue<T>();
            _dataAvailableEvent = new AutoResetEvent(false);
        }

        public FifoBuffer(T[] items) : this()
        {
            In(items);
        }

        public FifoBuffer(T item) : this()
        {
            In(item);
        }

        #endregion


        #region Fields 

        private AutoResetEvent _dataAvailableEvent;
        private Queue<T> _buffer;
        private object lockObj = new object();
        private bool disposedValue;

        #endregion


        #region Properties 

        public bool CanRead => true;

        public bool CanSeek => false;

        public bool CanWrite => true;

        public int Capacity
        {
            get
            {
                lock (lockObj)
                {
                    return _buffer.Capacity;
                }
            }
        }


        public long Length
        {
            get
            {
                lock (lockObj)
                {
                    return _buffer.Count;
                }
            }
        }

        public bool IsEmpty => Count == 0;

        public int Count
        {
            get
            {
                lock (lockObj)
                {
                    return _buffer.Count;
                }
            }
        }

        public bool IsSynchronized => true;

        public object SyncRoot => true;

        public bool IsReadOnly => false;

        #endregion


        #region Public Methods 

        public int In(T item)
        {
            lock (lockObj)
            {
                _buffer.Enqueue(item);
            }
            _dataAvailableEvent.Set();
            return 1;
        }
        public int In(T[] items)
        {
            lock (lockObj)
            {
                foreach (var item in items)
                    this.In(item);

            }
            _dataAvailableEvent.Set();
            return items.Length;
        }

        public T[] ToArray()
        {
            lock (lockObj)
            {
                return _buffer.ToArray();
            }
        }
        public bool Out(out T outValue, int timeout = -1)
        {
            if(timeout != -1)
            {
                if (!_dataAvailableEvent.WaitOne(timeout))
                {
                    outValue = default!;
                    return false;
                }
            }

            if (!IsEmpty)
            {
                lock (lockObj)
                {
                    outValue = _buffer.Dequeue();
                    return true;
                }
            }
            else
            {
                throw new InvalidOperationException("Buffer is empty");
            }
        }

        public bool Out(out T[] outValue, int length, int timeout = -1)
        {
            outValue = null!;

            if (timeout == -1)
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("Buffer is empty");
                }
                else if (length > _buffer.Count)
                {
                    throw new InvalidOperationException("Not enough elements in buffer");
                }
            }
            else
            {
                while (_buffer.Count < length)
                {
                    if (!_dataAvailableEvent.WaitOne(timeout))
                    {
                        return false;
                    }
                }
            }

            outValue = new T[length];
            lock (lockObj)
            {
                for (int i = 0; i < length; i++)
                {
                    outValue[i] = _buffer.Dequeue();
                }

                return true;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            lock (lockObj)
            {
                return _buffer.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public void Add(T item) => throw new NotImplementedException("Use In method instead.");

        public void Clear()
        {
            lock (lockObj)
            {
                _buffer.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (lockObj)
            {
                return _buffer.Contains(item);
            }
        }


        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (lockObj)
            {
                _buffer.CopyTo(array, arrayIndex);
            }
        }   

        public bool Remove(T item) => throw new NotImplementedException("Use Out method instead.");

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dataAvailableEvent.Dispose();
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
