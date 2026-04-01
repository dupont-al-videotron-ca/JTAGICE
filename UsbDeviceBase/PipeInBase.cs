using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbDeviceBase
{
    public abstract class PipeInBase : UsbPipeBase
    {
        public PipeInBase(Windows.Devices.Usb.UsbEndpointDescriptor descriptor):base(descriptor)
        {
        }

    }
}
