using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryPack;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Usb;

namespace UsbDeviceBase
{
    public abstract class UsbPipeBase : IUsbPipe
    {

        #region Constructors 

        public UsbPipeBase(Windows.Devices.Usb.UsbEndpointDescriptor descriptor)
        {
            EndpointDescriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
            UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
        }

        #endregion

        #region Fields 

        #endregion


        #region Properties 
        public Windows.Storage.Streams.ByteOrder ByteOrder { get; init; } = Windows.Storage.Streams.ByteOrder.LittleEndian;
        public Windows.Storage.Streams.UnicodeEncoding UnicodeEncoding { get; set; }

        protected MemoryPackSerializerOptions SerializerOptions
        {
            get
            {
                if (UnicodeEncoding == Windows.Storage.Streams.UnicodeEncoding.Utf8)
                {
                    return MemoryPackSerializerOptions.Utf8;
                }
                else
                {
                    return MemoryPackSerializerOptions.Utf16;
                }
            }
        }

        protected Windows.Devices.Usb.UsbEndpointDescriptor EndpointDescriptor { get; }

        //
        // Summary:
        //     Gets an object that represents the endpoint descriptor for the USB bulk IN endpoint.
        //
        //
        // Returns:
        //     A UsbBulkInEndpointDescriptor object that describes the USB bulk IN endpoint.
        public UsbBulkInEndpointDescriptor AsBulkInEndpointDescriptor => EndpointDescriptor.AsBulkInEndpointDescriptor;

        //
        // Summary:
        //     Gets an object that represents the endpoint descriptor for the USB bulk OUT endpoint.
        //
        //
        // Returns:
        //     A UsbBulkOutEndpointDescriptor that describes the USB bulk OUT endpoint.
        public UsbBulkOutEndpointDescriptor AsBulkOutEndpointDescriptor => EndpointDescriptor.AsBulkOutEndpointDescriptor;

        //
        // Summary:
        //     Gets an object that represents the endpoint descriptor for the USB interrupt
        //     IN endpoint.
        //
        // Returns:
        //     An UsbInterruptInEndpointDescriptor that describes the USB interrupt IN endpoint.
        public UsbInterruptInEndpointDescriptor AsInterruptInEndpointDescriptor => EndpointDescriptor.AsInterruptInEndpointDescriptor;

        //
        // Summary:
        //     Gets an object that represents the endpoint descriptor for the USB interrupt
        //     OUT endpoint.
        //
        // Returns:
        //     An UsbInterruptOutEndpointDescriptor object that describes the interrupt OUT
        //     endpoint.
        public UsbInterruptOutEndpointDescriptor AsInterruptOutEndpointDescriptor => EndpointDescriptor.AsInterruptOutEndpointDescriptor;

        //
        // Summary:
        //     Gets the direction of the USB endpoint.
        //
        // Returns:
        //     A UsbTransferDirection value that indicates the direction of the endpoint. This
        //     value is Bit 7 of the **bEndpointAddress** field of an endpoint descriptor. For
        //     information, see Table 9-13 in the Universal Serial Bus Specification (version
        //     2.0) or Table 9-18 in the Universal Serial Bus 3.0 Specification.
        public UsbTransferDirection Direction => EndpointDescriptor.Direction;

        //
        // Summary:
        //     Gets the USB endpoint number.
        //
        // Returns:
        //     The USB endpoint number. That number is in Bit 3...0 of the **bEndpointAddress**
        //     field of an endpoint descriptor. For information, see Table 9-13 in the Universal
        //     Serial Bus Specification (version 2.0) or Table 9-18 in the Universal Serial
        //     Bus 3.0 Specification.
        public byte EndpointNumber => EndpointDescriptor.EndpointNumber;

        //
        // Summary:
        //     Gets the type of USB endpoint.
        //
        // Returns:
        //     A UsbEndpointType constant that indicates the type of USB endpoint. This value
        //     is Bit 1...0 of the **bmAttributes** field of an endpoint descriptor. For information,
        //     see Table 9-13 in the Universal Serial Bus Specification (version 2.0) or Table
        //     9-18 in the Universal Serial Bus 3.0 Specification.
        public UsbEndpointType EndpointType => EndpointDescriptor.EndpointType;

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        #endregion


        #region Protected Methods 

        #endregion

        #region Provate Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion


    }
}
