#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8618, CS8603, CS8604, CA1416
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;

namespace UsbDeviceBase
{
    public sealed class BulkInPipeImpl : PipeInBase
    {

        #region Constructors 
        public BulkInPipeImpl(Windows.Devices.Usb.UsbEndpointDescriptor descriptor, Windows.Devices.Usb.UsbBulkInPipe pipe) : base(descriptor)
        {
            logger = log4net.LogManager.GetLogger(this.GetType());
            this.InPipe = pipe;
            CreateDataReader();
        }

        #endregion

        private DataReader dataReader;
        private ILog logger;

        #region Properties 

        public Windows.Devices.Usb.UsbBulkInPipe InPipe { get; private set; }

        public override bool IsByteToRead
        {
            get
            {
                return dataReader.UnconsumedBufferLength > 0;
            }
        }

        #endregion


        #region Public Methods 

        public override bool ReadBytes(out byte[] values, int length, int timeout = -1)
        {
            return this.ReadBytesAsync(out values, length, CancellationToken.None, timeout).GetAwaiter().GetResult();
        }

        public override Task<bool> ReadBytesAsync(out byte[] values, int length, CancellationToken cancellationToken, int timeout = -1)
        {
            try
            {
                values = null;
                List<byte> buffer = new List<byte>();
                while (length > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    DataReaderLoadOperation result = dataReader.LoadAsync((uint)length);
                    if (result.AsTask().Wait(timeout, cancellationToken))
                    {
                        var bufValues = new byte[dataReader.UnconsumedBufferLength];
                        length -= bufValues.Length;
                        dataReader.ReadBytes(bufValues);
                        buffer.AddRange(bufValues);
                    }
                    else
                    {
                        result.Cancel();
                        values = null;
                        return Task.FromResult(false);
                    }

                }

                values = buffer.ToArray();
                return Task.FromResult(true);
            }
            catch (COMException ex)
            {
                logger.Error($"COMException occurred while reading bytes asynchronously.", ex);
                values = null;
                return Task.FromResult(false);

            }
            catch (ArithmeticException)
            {
                values = null;
                return Task.FromResult(false);
            }
            catch (OperationCanceledException ex)
            {
                logger.Warn($"OperationCanceledException occurred while reading bytes asynchronously.", ex);
                values = null;
                return Task.FromResult(false);
            }
        }

        public override bool ReadStructure<T>(ref T obj, int timeout = -1) where T : struct
        {
            int size = Marshal.SizeOf<T>();

            if (this.ReadBytes(out byte[]? values, size, timeout))
            {
                Span<byte> rspan = new Span<byte>(values);
                obj = MemoryMarshal.Read<T>(rspan);
                return values.Length == size;
            }
            else
                return false;
        }


        public override async Task<T?> ReadStructureAsync<T>(CancellationToken cancellationToken, int timeout = -1)
            where T : struct
        {
            int size = Marshal.SizeOf<T>();

            if (await this.ReadBytesAsync(out byte[]? values, size, cancellationToken, timeout))
            {
                Span<byte> rspan = new Span<byte>(values);
                T obj = MemoryMarshal.Read<T>(rspan);
                return obj;
            }
            else
                return null;
        }

        public override void Reset()
        {
            logger.Debug($"Reset UnconsumedBufferLength:{dataReader.UnconsumedBufferLength}");

            CreateDataReader();

            logger.Debug($"Reset completed.");
        }

        protected override void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                if (disposing)
                {
                    dataReader.Dispose();
                    dataReader = null;
                    this.InPipe = null;
                }
            }

            base.Dispose(disposing);
        }


        private void CreateDataReader()
        {
            dataReader = new DataReader(InPipe.InputStream);
            dataReader.ByteOrder = this.ByteOrder;
            dataReader.UnicodeEncoding = this.UnicodeEncoding;
            dataReader.InputStreamOptions = InputStreamOptions.None;
        }

        #endregion
    }
}
