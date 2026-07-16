using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbDeviceBase;

namespace MyUsbDevice
{
    public class At90UsbInterface : UsbDeviceBase.UsbInterfaceBase
    {
        public At90UsbInterface(Windows.Devices.Usb.UsbInterfaceDescriptor descriptor, IEnumerable<KeyValuePair<int, PipeOutBase>> outPipes, IEnumerable<KeyValuePair<int, PipeInBase>> inPipes) : base(descriptor, outPipes, inPipes)
        {
        }
    }
}
