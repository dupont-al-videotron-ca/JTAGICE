using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace MyFramework
{
    public class FifoBuffer<T>: IEnumerable<T>,
        ICollection<T>,
        IReadOnlyCollection<T>
    {

        #region Constructors 

        public FifoBuffer()
        {
            _buffer = new Queue<T>();
        }

        public FifoBuffer(T[] items)
        {
            _buffer = new Queue<T>();
            In(items);
        }

        public FifoBuffer(T item)
        {
            _buffer = new Queue<T>();
            In(item);
        }

        #endregion


        #region Fields 

        private Queue<T> _buffer;

        #endregion


        #region Properties 

        public bool CanRead => true;

        public bool CanSeek => false;

        public bool CanWrite => true;

        public int Capacity => _buffer.Capacity;

        public long Length => _buffer.Count;

        public bool IsEmpty => Count == 0;

        public int Count => _buffer.Count;

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        #endregion


        #region Public Methods 

        public int In(T item)
        {
            _buffer.Enqueue(item);
            return 1;
        }
        public int In(T[] items)
        {
            foreach (var item in items)
                this.In(item);

            return items.Length;
        }

        public T[] ToArray()
        {
            return _buffer.ToArray();
        }
        public T Out()
        {
            if (!IsEmpty)
            {
                T reval = _buffer.Dequeue();
                return reval;
            }
            else
            {
                throw new InvalidOperationException("Buffer is empty");
            }
        }

        public T[] Out(int length)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Buffer is empty");
            }
            else if (length > _buffer.Count)
            {
                throw new InvalidOperationException("Not enough elements in buffer");
            }
            else
            {
                T[] reval = new T[length];
                for (int i = 0; i < length; i++)
                {
                    reval[i] = Out();
                }

                return reval;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _buffer.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public void Add(T item) => throw new NotImplementedException("Use In method instead.");

        public void Clear() => _buffer.Clear()  ;

        public bool Contains(T item) => _buffer.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _buffer.CopyTo(array, arrayIndex);

        public bool Remove(T item) => throw new NotImplementedException("Use Out method instead.");

        #endregion
    }
}
