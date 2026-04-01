using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbDeviceBase;

namespace MyUsbDevice
{
    public class At90UsbFlashInterface : UsbDeviceBase.UsbInterfaceBase
    {
        public At90UsbFlashInterface(Windows.Devices.Usb.UsbInterfaceDescriptor descriptor, IEnumerable<KeyValuePair<int, PipeOutBase>> outPipes, IEnumerable<KeyValuePair<int, PipeInBase>> inPipes) : base(descriptor, outPipes, inPipes)
        {
        }
    }
}
