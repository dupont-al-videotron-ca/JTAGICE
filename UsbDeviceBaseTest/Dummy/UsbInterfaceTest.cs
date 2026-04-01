using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbDeviceBase;
using Xunit;

namespace UsbDeviceBaseTest.Dummy
{
    public class UsbInterfaceTest : UsbDeviceBase.UsbInterfaceBase
    {
        public UsbInterfaceTest(Windows.Devices.Usb.UsbInterfaceDescriptor descriptor, IEnumerable<KeyValuePair<int, PipeOutBase>> outPipes, IEnumerable<KeyValuePair<int, PipeInBase>> inPipes) : base(descriptor, outPipes, inPipes)
        {
        }

        internal void VerifyInterface()
        {
            Assert.Single(this.OutPipes);
            Assert.Equal(2, this.InPipes.Count);
        }
    }
}
