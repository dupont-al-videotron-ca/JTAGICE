using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MemoryPack;
using Windows.Devices.Usb;
using Windows.Storage.Streams;

namespace UsbDeviceBase
{
    public class BulkOutPipeImpl : PipeOutBase
    {

        #region Constructors 
        public BulkOutPipeImpl(Windows.Devices.Usb.UsbEndpointDescriptor descriptor, Windows.Devices.Usb.UsbBulkOutPipe pipe) : base(descriptor)
        {
            this.OutPipe = pipe;
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        internal Windows.Devices.Usb.UsbBulkOutPipe OutPipe { get; }

        //
        // Summary:
        //     Gets the object that represents the endpoint descriptor associated with the USB
        //     bulk OUT endpoint.
        //
        // Returns:
        //     A UsbBulkOutEndpointDescriptor that represents the endpoint descriptor associated
        //     with the USB bulk OUT endpoint.
        internal UsbBulkOutEndpointDescriptor OutEndpointDescriptor => OutPipe.EndpointDescriptor;

        //
        // Summary:
        //     Gets an output stream to which the app can write data to send to the endpoint.
        //
        //
        // Returns:
        //     The output steam that contains the transfer data.
        internal IOutputStream OutputStream => OutPipe.OutputStream;

        //
        // Summary:
        //     Gets or sets configuration flags that controls the behavior of the pipe that
        //     writes data to a USB bulk IN endpoint.
        //
        // Returns:
        //     A UsbWriteOptions constant that indicates the pipe policy.
        public override UsbWriteOptions WriteOptions
        {
            get
            {
                return OutPipe.WriteOptions;
            }
            set
            {
                OutPipe.WriteOptions = value;
            }
        }


        public override int Send(byte[] bytes)
        {
            return SendAsync(bytes).GetAwaiter().GetResult();
        }

        public override async Task<int> SendAsync(byte[] bytes)
        {
            if (bytes.Length == 0)
            {
                return 0;
            }

            // Create the data writer object backed by the in-memory stream.
            using (DataWriter dataWriter = new DataWriter(this.OutputStream))
            {
                IBuffer buffer = WindowsRuntimeBuffer.Create(bytes, 0, bytes.Length, bytes.Length);
                uint offset = 0;

                do
                {
                    dataWriter.UnicodeEncoding = this.UnicodeEncoding;
                    dataWriter.ByteOrder = this.ByteOrder;
                    dataWriter.WriteBuffer(buffer, offset, Math.Min((uint)bytes.Length - offset, OutEndpointDescriptor.MaxPacketSize));

                    // Send the contents of the writer to the backing stream.
                    await dataWriter.StoreAsync();

                    // For the in-memory stream implementation we are using, the flushAsync call 
                    // is superfluous,but other types of streams may require it.
                    //await dataWriter.FlushAsync();

                    // In order to prolong the lifetime of the stream, detach it from the 
                    // DataWriter so that it will not be closed when Dispose() is called on 
                    // dataWriter. Were we to fail to detach the stream, the call to 
                    // dataWriter.Dispose() would close the underlying stream, preventing 
                    // its subsequent use by the DataReader below.
                    dataWriter.DetachStream();

                    offset += Math.Min((uint)bytes.Length - offset, OutEndpointDescriptor.MaxPacketSize);
                } while (offset < bytes.Length);

                return bytes.Length;

            }

        }


        #endregion

    }
}
