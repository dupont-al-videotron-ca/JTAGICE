using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
        private int waitDelay = 100;

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

        public bool WaitForTimeout { get; set; } = false;

        protected internal override bool ReadByte(out byte value, int timeout = -1)
        {
            return ReadByteAsync(out value, this.CancellationToken, this.Timeout).GetAwaiter().GetResult();
        }

        protected internal override Task<bool> ReadByteAsync(out byte value, CancellationToken cancellationToken, int timeout = -1)
        {
            byte[]? values = new byte[1];
            value = 0;
            bool result = ReadBytesAsync(out values, 1, cancellationToken, timeout).GetAwaiter().GetResult();
            if (result)
            {
                value = values![0];
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }


        protected internal override bool ReadBytes(out byte[]? values, uint length, int timeout = -1)
        {
            return ReadBytesAsync(out values, length, this.CancellationToken, timeout).GetAwaiter().GetResult();
        }

        protected internal override Task<bool> ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout = -1)
        {
            bool timeoutOccured = false;
            if (this.Timeout != -1)
            {
                // force timeout to occured.
                timeout = (int)(this.Timeout * length);
                waitDelay = timeout * 2;
            }

            if (!IsEndOfFrame)
            {
                values = new byte[length];
                Array.Copy(_buffer, _position, values, 0, length);
                _position += (int)length;
                return Task.FromResult(true);
            }
            else
            {
                while (true)
                {
                    // Wait indefinitely until timeout or cancellation is requested
                    try
                    {
                        Logger.Debug($"Before timeoutOccured: {timeoutOccured}, waitDelay: {waitDelay}, timeout: {timeout}, cancellationRequest: {cancellationToken.IsCancellationRequested}.");
                        Task.Delay(waitDelay, cancellationToken).Wait();
                        Logger.Debug($"After timeoutOccured: {timeoutOccured}, waitDelay: {waitDelay}, timeout: {timeout}, cancellationRequest: {cancellationToken.IsCancellationRequested}.");
                        if (WaitForTimeout)
                        {
                            WaitForTimeout = false;
                            TimeoutOccured = true;
                            break;
                        }

                        if (cancellationToken.IsCancellationRequested)
                        {
                            Logger.Debug($"Cancellation requested while waiting for byte.");
                            break;
                        }
                    }
                    catch (OperationCanceledException ex)
                    {
                        if (!WaitForTimeout)
                        {
                            Logger.Debug($"Cancellation requested while waiting for byte: {ex.Message}");
                            break;
                        }
                    }
                    catch (System.AggregateException ex)
                    {
                        if(ex.InnerExceptions.Any(e => e is TaskCanceledException))
                        {
                            Logger.Debug($"Cancellation requested while waiting for byte: {ex.InnerExceptions.First(e => e is TaskCanceledException).Message}");
                            break;
                        }
                    }
                }

                values = null!;
                return Task.FromResult(false);

            }
        }
    }
}