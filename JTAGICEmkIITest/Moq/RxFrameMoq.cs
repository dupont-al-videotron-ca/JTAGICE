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
    internal class RxFrameMoq : IRxFrameAdaptor
    {
        private byte[] _buffer;
        private int _position;

        public int RxTimeout { get; private set; }

        private const int _defaultWaitDelay = 100;
        private int waitDelay = _defaultWaitDelay;
        private RxFrame _rxFrame = null!;

        public RxFrameMoq(byte[] buffer) : this(buffer, -1)
        {
        }

        public RxFrameMoq(byte[] buffer, int timeout) : base()
        {
            this._buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            _position = 0;
            RxTimeout = timeout;
        }

        public void Attach(RxFrame rxFrame)
        {
            ArgumentNullException.ThrowIfNull(rxFrame);
            _rxFrame = rxFrame;
        }

        public bool IsByteToRead => _position >= _buffer.Length;

        public bool WaitForTimeout { get; set; } = false;

        public bool ReadByte(out byte value, int timeout = -1)
        {
            return ReadByteAsync(out value, _rxFrame.CancellationToken, _rxFrame.Timeout).GetAwaiter().GetResult();
        }

        public Task<bool> ReadByteAsync(out byte value, CancellationToken cancellationToken, int timeout = -1)
        {
            byte[]? values = new byte[1];
            value = 0;
            bool result = ReadBytesAsync(out values, 1, cancellationToken, _rxFrame.Timeout).GetAwaiter().GetResult();
            if (result)
            {
                value = values![0];
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public bool ReadBytes(out byte[]? values, uint length, int timeout = -1)
        {
            return ReadBytesAsync(out values, length, _rxFrame.CancellationToken, _rxFrame.Timeout).GetAwaiter().GetResult();
        }

        public Task<bool> ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout = -1)
        {
            bool timeoutOccured = false;
            if (_rxFrame.Timeout != -1)
            {
                // force timeout to occured.
                timeout = (int)(_rxFrame.Timeout * length);
                waitDelay = timeout;
            }
            else
            {
                waitDelay = _defaultWaitDelay;
            }

            if (!IsByteToRead)
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
                        _rxFrame.Logger.Debug($"Before timeoutOccured: {timeoutOccured}, waitDelay: {waitDelay}, timeout: {timeout}, cancellationRequest: {cancellationToken.IsCancellationRequested}.");
                        Task.Delay(waitDelay, cancellationToken).Wait();
                        _rxFrame.Logger.Debug($"After timeoutOccured: {timeoutOccured}, waitDelay: {waitDelay}, timeout: {timeout}, cancellationRequest: {cancellationToken.IsCancellationRequested}.");
                        if (WaitForTimeout)
                        {
                            _rxFrame.TimeoutOccured = true;
                            WaitForTimeout = false;
                            break;
                        }

                        if (cancellationToken.IsCancellationRequested)
                        {
                            _rxFrame.Logger.Debug($"Cancellation requested while waiting for byte.");
                            break;
                        }
                    }
                    catch (OperationCanceledException ex)
                    {
                        if (!WaitForTimeout)
                        {
                            _rxFrame.Logger.Debug($"Cancellation requested while waiting for byte: {ex.Message}");
                            break;
                        }
                    }
                    catch (System.AggregateException ex)
                    {
                        if (ex.InnerExceptions.Any(e => e is TaskCanceledException))
                        {
                            _rxFrame.Logger.Debug($"Cancellation requested while waiting for byte: {ex.InnerExceptions.First(e => e is TaskCanceledException).Message}");
                            break;
                        }
                    }
                }

                values = null!;
                return Task.FromResult(false);

            }
        }

        bool IRxFrameAdaptor.ReadByte(out byte value, int timeout) => this.ReadByte(out value, timeout);

        Task<bool> IRxFrameAdaptor.ReadByteAsync(out byte value, CancellationToken cancellationToken, int timeout) => this.ReadByteAsync(out value, cancellationToken, timeout);

        bool IRxFrameAdaptor.ReadBytes(out byte[]? values, uint length, int timeout) => this.ReadBytes(out values, length, timeout);

        Task<bool> IRxFrameAdaptor.ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout) => this.ReadBytesAsync(out values, length, cancellationToken, timeout);
    }
}