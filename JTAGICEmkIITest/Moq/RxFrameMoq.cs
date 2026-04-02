using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;

namespace JTAGICEmkIITest.Moq
{
    internal class RxFrameMoq : RxFrame
    {
        private byte[] _buffer;
        private int _position;
        private int waitDelay = 1;

        internal bool WaitForEver { get; set; }

        public RxFrameMoq(byte[] buffer) : this(buffer, 1000 * 60 * 60)
        {
        }

        public RxFrameMoq(byte[] buffer, int timeout) : base()
        {
            this._buffer = buffer;
            _position = 0;
            this.Timeout = timeout;
        }

        public bool IsEndOfFrame => _position >= _buffer.Length;

        protected internal override byte GetByte()
        {
            return GetByteAsync(this.Token).GetAwaiter().GetResult();
        }

        protected internal override Task<byte> GetByteAsync(CancellationToken cancellationToken)
        {
            while (WaitForEver && IsEndOfFrame && !cancellationToken.IsCancellationRequested)
            {
                // Wait indefinitely until a byte is available or cancellation is requested
                try
                {
                    Task.Delay(waitDelay, cancellationToken).Wait(cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Logger.Debug($"Cancellation requested while waiting for byte.");
                    }
                }
                catch (OperationCanceledException ex)
                {
                    // Handle cancellation if needed
                    Logger.Debug($"OperationCanceledException {ex.Message} while waiting for byte.");
                    break;
                }
            }

            if (IsEndOfFrame || cancellationToken.IsCancellationRequested)
                return Task.FromResult((byte)0xFF);

            return Task.FromResult(_buffer[_position++]);
        }


        protected internal override byte[] GetBytes(uint length)
        {
            return GetBytesAsync(length, this.Token).GetAwaiter().GetResult();
        }

        protected internal override Task<byte[]> GetBytesAsync(uint length, CancellationToken cancellationToken)
        {
            while (WaitForEver && IsEndOfFrame && !cancellationToken.IsCancellationRequested)
            {
                // Wait indefinitely until a byte is available or cancellation is requested
                try
                {
                    Task.Delay(waitDelay, cancellationToken).Wait(cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Logger.Debug($"Cancellation requested while waiting for byte.");
                    }
                }
                catch (OperationCanceledException ex)
                {
                    // Handle cancellation if needed
                    Logger.Debug($"OperationCanceledException {ex.Message} while waiting for byte.");
                    break;
                }
            }

            byte[] result = new byte[length];
            if (IsEndOfFrame ||
                cancellationToken.IsCancellationRequested ||
                length + _position > _buffer.Length)
            {
                return Task.FromResult(result);
            }
            else
            {
                Array.Copy(_buffer, _position, result, 0, length);
                _position += (int)length;
                return Task.FromResult(result);
            }
        }
    }
}