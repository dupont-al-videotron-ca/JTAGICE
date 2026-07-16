using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Usb;

namespace UsbDeviceBase
{
    public abstract class PipeOutBase : UsbPipeBase, IUsbPipeOut
    {
        public PipeOutBase(Windows.Devices.Usb.UsbEndpointDescriptor descriptor): base(descriptor)
        {
        }

        public abstract UsbWriteOptions WriteOptions { get; set; }
        public abstract int Send(byte[] bytes);
        public abstract Task<int> SendAsync(byte[] bytes);
    }
}
