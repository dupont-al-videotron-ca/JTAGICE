using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using Newtonsoft.Json.Linq;

namespace JTAGICEmkIITest.Moq
{
    internal class RxFrameMoq : RxFrame
    {
        private byte[] _buffer;
        private int _position;
        private int waitDelay = 1;

        internal bool WaitForEver { get; set; }

        public RxFrameMoq(byte[] buffer) : this(buffer, -1)
        {
        }

        public RxFrameMoq(byte[] buffer, int timeout) : base()
        {
            this._buffer = buffer;
            _position = 0;
            this.Timeout = timeout;
        }

        public bool IsEndOfFrame => _position >= _buffer.Length;

        protected internal override bool ReadByte(out byte value, int timeout = -1)
        {
            return ReadByteAsync(out value, this.CancellationToken, timeout).GetAwaiter().GetResult();
        }

        protected internal override Task<bool> ReadByteAsync(out byte value, CancellationToken cancellationToken, int timeout = -1)
        {
            bool timeoutOccured = false;
            value = 0;
            while (WaitForEver && !timeoutOccured && IsEndOfFrame && !cancellationToken.IsCancellationRequested)
            {
                // Wait indefinitely until a byte is available or cancellation is requested
                try
                {
                    timeoutOccured = Task.Delay(waitDelay, cancellationToken).Wait(timeout, cancellationToken);

                    if (timeoutOccured)
                    {
                        this.TimeoutOccured = true;
                        break;
                    }

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

            if (IsEndOfFrame || cancellationToken.IsCancellationRequested || timeoutOccured)
                return Task.FromResult(false);

            value = _buffer[_position++];
            return Task.FromResult(true);
        }


        protected internal override bool ReadBytes(out byte[]? values, uint length, int timeout = -1)
        {
            return ReadBytesAsync(out values, length, this.CancellationToken).GetAwaiter().GetResult();
        }

        protected internal override Task<bool> ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout = -1)
        {
            bool timeoutOccured = false;
            while (WaitForEver && !timeoutOccured && IsEndOfFrame && !cancellationToken.IsCancellationRequested)
            {
                // Wait indefinitely until a byte is available or cancellation is requested
                try
                {
                    timeoutOccured = Task.Delay(waitDelay, cancellationToken).Wait(timeout, cancellationToken);
                    if (timeoutOccured)
                    {
                        TimeoutOccured = true;
                        break;
                    }

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

            if (IsEndOfFrame || timeoutOccured ||
                cancellationToken.IsCancellationRequested ||
                length + _position > _buffer.Length)
            {
                values = null!;
                return Task.FromResult(false);
            }
            else
            {
                values = new byte[length];
                Array.Copy(_buffer, _position, values, 0, length);
                _position += (int)length;
                return Task.FromResult(true);
            }
        }
    }
}