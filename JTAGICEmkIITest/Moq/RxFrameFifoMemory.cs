using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using log4net.Repository.Hierarchy;
using MyFramework;

namespace JTAGICEmkIITest.Moq
{
    internal class RxFrameFifoMemory : IRxFrameAdaptorTest
    {
        public int RxTimeout
        {
            get;
            private set;
        }

        private const int _defaultWaitDelay = 100;
        private readonly FifoBuffer<byte> _fifoBuffer;
        private RxFrame _rxFrame = null!;

        public RxFrameFifoMemory(FifoBuffer<byte> fifoBuffer) : this(fifoBuffer, -1)
        {
        }

        public RxFrameFifoMemory(FifoBuffer<byte> fifoBuffer, int timeout) : base()
        {
            _fifoBuffer = fifoBuffer ?? throw new ArgumentNullException(nameof(fifoBuffer));
            RxTimeout = timeout;
        }

        public bool IsByteToRead => _fifoBuffer.IsEmpty;

        public bool WaitForTimeout { get; set; } = false;

        public void Attach(RxFrame rxFrame)
        {
            ArgumentNullException.ThrowIfNull(rxFrame);
            _rxFrame = rxFrame;
        }

        public bool ReadBytes(out byte[]? values, uint length, int timeout = -1)
        {
            return ReadBytesAsync(out values, length, _rxFrame.CancellationToken, timeout).GetAwaiter().GetResult();
        }

        public Task<bool> ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout = -1)
        {
            int localTimeout = -1;
            values = null;
            if (timeout != -1)
            {
                // force timeout to occur.
                localTimeout = (int)(timeout * length);
            }

            _rxFrame.Logger.Debug($"ReadBytesAsync called with length: {length}, localTimeout: {localTimeout}, cancellationRequested: {cancellationToken.IsCancellationRequested}.");

            while (true)
            {
//                _rxFrame.Logger.Debug($"Attempting to read {length} bytes from FIFO buffer with current count: {_fifoBuffer.Count} and IsByteToRead: {IsByteToRead}.");

                // Wait indefinitely until timeout or cancellation is requested
                try
                {
                    values = new byte[length];
                    bool result = _fifoBuffer.Out(out byte[] data, (int)length, localTimeout);

                    if (result)
                    {
                        Array.Copy(data, values, length);
                        return Task.FromResult(true);
                    }
                    else if (cancellationToken.IsCancellationRequested)
                    {
                        _rxFrame.Logger.Debug($"Cancellation requested while waiting for byte.");
                        break;
                    }
                    else
                    {
                        if (WaitForTimeout)
                        {
                            _rxFrame.TimeoutOccured = true;
                            WaitForTimeout = false;
                            break;
                        }
                        _rxFrame.Logger.Debug($"Read operation timed out after {localTimeout} milliseconds while waiting for {length} bytes.");
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

            return Task.FromResult(false);

        }

        bool IRxFrameAdaptor.ReadBytes(out byte[]? values, uint length, int timeout) => this.ReadBytes(out values, length, timeout);

        Task<bool> IRxFrameAdaptor.ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout) => this.ReadBytesAsync(out values, length, cancellationToken, timeout);

    }
}