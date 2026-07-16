using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using Windows.Devices.Usb;

namespace UsbDeviceBase
{
    public abstract class UsbInterfaceBase : IUsbInterface
    {


        #region Constructors 
        public UsbInterfaceBase(Windows.Devices.Usb.UsbInterfaceDescriptor descriptor, IEnumerable<KeyValuePair<int, PipeOutBase>> outPipes, IEnumerable<KeyValuePair<int, PipeInBase>> inPipes)
        {
            UsbInterfaceDescriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
            InPipes = new Dictionary<int, PipeInBase>(inPipes);

            OutPipes = new Dictionary<int, PipeOutBase>(outPipes);
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 
        //
        // Summary:
        //     Gets the **bAlternateSetting** field of the USB interface descriptor. The value
        //     is a number that identifies the alternate setting defined by the interface.
        //
        // Returns:
        //     A number that identifies the alternate setting defined by the interface.
        public byte AlternateSettingNumber => UsbInterfaceDescriptor.AlternateSettingNumber;

        //
        // Summary:
        //     Gets the **bInterfaceClass** field of the USB interface descriptor. The value
        //     indicates the USB-defined class to which the interface conforms.
        //
        // Returns:
        //     The USB-defined class to which the interface conforms.
        public byte ClassCode => UsbInterfaceDescriptor.ClassCode;

        //
        // Summary:
        //     Gets the **bInterfaceNumber** field of the USB interface descriptor. The value
        //     is the index that identifies the interface.
        //
        // Returns:
        //     The index that identifies the interface.
        public byte InterfaceNumber => UsbInterfaceDescriptor.InterfaceNumber;

        //
        // Summary:
        //     Gets the **bInterfaceProtocol** field of the interface descriptor. The value
        //     is a USB-assigned identifier that specifies a USB-defined protocol to which the
        //     interface conforms.
        //
        // Returns:
        //     A USB-assigned identifier that specifies a USB-defined protocol to which the
        //     interface conforms.
        public byte ProtocolCode => UsbInterfaceDescriptor.ProtocolCode;

        //
        // Summary:
        //     Gets the **bInterfaceSubClass** field of the USB interface descriptor. The value
        //     is a USB-assigned identifier that specifies a USB-defined subclass to which the
        //     interface.
        //
        // Returns:
        //     A USB-assigned identifier that specifies a USB-defined subclass to which the
        //     interface.
        public byte SubclassCode => UsbInterfaceDescriptor.SubclassCode;

        public Dictionary<int, PipeInBase> InPipes { get; }
        public Dictionary<int, PipeOutBase> OutPipes { get; }

        protected Windows.Devices.Usb.UsbInterfaceDescriptor UsbInterfaceDescriptor { get; }

        #endregion


        #region Public Methods 

        internal static IEnumerable<KeyValuePair<int, PipeInBase>> CreateInPipes(UsbInterface intf)
        {
            var logger = LogManager.GetLogger(typeof(UsbInterfaceBase));
            var retval = new Dictionary<int, PipeInBase>();

            foreach (var endpointDdescriptor in intf.Descriptors.Where(e => (UsbDescriptorTypeEnum)e.DescriptorType == UsbDescriptorTypeEnum.USB_ENDPOINT_DESCRIPTOR_TYPE))
            {
                Windows.Devices.Usb.UsbEndpointDescriptor usbEndpointDescriptor = UsbEndpointDescriptor.Parse(endpointDdescriptor);
                if (usbEndpointDescriptor.Direction == Windows.Devices.Usb.UsbTransferDirection.In)
                {
                    switch (usbEndpointDescriptor.EndpointType)
                    {
                        case UsbEndpointType.Bulk:
                            if (intf.BulkInPipes.Any())
                            {
                                foreach (var pipe in intf.BulkInPipes)
                                {
                                    if (pipe.EndpointDescriptor.EndpointNumber == usbEndpointDescriptor.EndpointNumber)
                                    {
                                        retval.Add(usbEndpointDescriptor.EndpointNumber, new BulkInPipeImpl(usbEndpointDescriptor, pipe));
                                    }
                                }
                            }
                            else
                            {
                                logger.Warn($"Interface {intf.InterfaceNumber} has no bulk in or interrupt in pipes. Skipping pipe creation.");
                            }

                            break;
                        default:
                            logger.Warn($"Endpoint {usbEndpointDescriptor.EndpointNumber} of interface {intf.InterfaceNumber} is of unsupported type {usbEndpointDescriptor.EndpointType}. Skipping pipe creation.");
                            break;
                    }
                }
                else
                {
                    //logger.Debug($"Endpoint {usbEndpointDescriptor.EndpointNumber} of interface {intf.InterfaceNumber} is not an IN endpoint. Skipping pipe creation.");
                }
            }
            return retval;
        }

        internal static IEnumerable<KeyValuePair<int, PipeOutBase>> CreateOutPipes(UsbInterface intf)
        {
            var logger = LogManager.GetLogger(typeof(UsbInterfaceBase));
            var retval = new Dictionary<int, PipeOutBase>();

            foreach (var endpointDdescriptor in intf.Descriptors.Where(e => (UsbDescriptorTypeEnum)e.DescriptorType == UsbDescriptorTypeEnum.USB_ENDPOINT_DESCRIPTOR_TYPE))
            {

                Windows.Devices.Usb.UsbEndpointDescriptor usbEndpointDescriptor = UsbEndpointDescriptor.Parse(endpointDdescriptor);
                if (usbEndpointDescriptor.Direction == Windows.Devices.Usb.UsbTransferDirection.Out)
                {
                    switch (usbEndpointDescriptor.EndpointType)
                    {
                        case UsbEndpointType.Bulk:
                            if (intf.BulkOutPipes.Any())
                            {
                                foreach (var pipe in intf.BulkOutPipes)
                                {
                                    if (pipe.EndpointDescriptor.EndpointNumber == usbEndpointDescriptor.EndpointNumber)
                                    {
                                        retval.Add(usbEndpointDescriptor.EndpointNumber, new BulkOutPipeImpl(usbEndpointDescriptor, pipe));
                                    }
                                }
                            }
                            else
                            {
                                logger.Warn($"Interface {intf.InterfaceNumber} has no bulk in or interrupt in pipes. Skipping pipe creation.");
                            }
                            break;                            
                        default:
                            throw new NotImplementedException("InterruptOutPipeImpl is not implemented yet. Cannot create interrupt in pipe.");
                    }
                }
                else
                {
                    //logger.Debug($"Endpoint {usbEndpointDescriptor.EndpointNumber} of interface {intf.InterfaceNumber} is not an OUT endpoint. Skipping pipe creation.");
                }
            }
            return retval;
        }

        #endregion


        #region Private/Protected Methods 

        #endregion


        #region Private Classes / Enum 

        #endregion

    }
}
