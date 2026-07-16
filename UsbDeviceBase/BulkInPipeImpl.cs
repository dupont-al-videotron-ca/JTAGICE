#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8618, CS8603, CS8604, CA1416
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MemoryPack;
using Windows.Storage.Streams;

namespace UsbDeviceBase
{
    public sealed class BulkInPipeImpl : PipeInBase
    {

        #region Constructors 
        public BulkInPipeImpl(Windows.Devices.Usb.UsbEndpointDescriptor descriptor, Windows.Devices.Usb.UsbBulkInPipe pipe) : base(descriptor)
        {
            this.InPipe = pipe;
        }

        #endregion

        #region Properties 

        public Windows.Devices.Usb.UsbBulkInPipe InPipe { get; private set; }

        public override bool IsByteToRead => InPipe.InputStream.AsStreamForRead().CanRead;

        #endregion


        #region Public Methods 

        public override bool ReadBytes(out byte[] values, uint length, int timeout = -1)
        {
            return this.ReadBytesAsync(out values, length, CancellationToken.None, timeout).GetAwaiter().GetResult();
        }

        public override Task<bool> ReadBytesAsync(out byte[] values, uint length, CancellationToken cancellationToken, int timeout = -1)
        {
            using (DataReader dataReader = new DataReader(InPipe.InputStream))
            {
                dataReader.ByteOrder = this.ByteOrder;
                dataReader.UnicodeEncoding = this.UnicodeEncoding;

                List<byte> buffer = new List<byte>();
                while (length > 0)
                {
                    var result = dataReader.LoadAsync(length);
                    if (timeout >= 0)
                    {
                        var timeoutTask = Task.Delay(timeout, cancellationToken);
                        var completedTask = Task.WhenAny(result.AsTask(), timeoutTask).GetAwaiter().GetResult();
                        if (completedTask == timeoutTask)
                        {
                            values = null;
                            return Task.FromResult(false);
                        }
                    }
                    else
                    {
                        result.AsTask().Wait(cancellationToken);
                    }

                    var bufValues = new byte[dataReader.UnconsumedBufferLength];
                    length -= (uint)bufValues.Length;
                    dataReader.ReadBytes(bufValues);

                    buffer.AddRange(bufValues);
                }

                dataReader.DetachStream();
                values = buffer.ToArray();
                return Task.FromResult(true);
            }
        }

        public override bool ReadStructure<T>(ref T obj, int timeout) where T : struct
        {
            int size = Marshal.SizeOf<T>();

            if (this.ReadBytes(out byte[]? values, (uint)size, timeout))
            {
                Span<byte> rspan = new Span<byte>(values);
                obj = MemoryMarshal.Read<T>(rspan);
                return true;
            }
            else
                return false;
        }

        #endregion
    }
}
