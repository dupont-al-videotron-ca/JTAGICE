using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbDeviceBase
{
    public abstract class PipeOutBase : UsbPipeBase
    {
        public PipeOutBase(Windows.Devices.Usb.UsbEndpointDescriptor descriptor): base(descriptor)
        {
        }
    }
}
