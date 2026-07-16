using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbDeviceBase;
using Windows.Devices.Usb;
using Windows.Storage.Streams;
using Xunit;

namespace UsbDeviceBaseTest.Dummy
{
    /// <summary>
    /// A test implementation of the UsbDeviceBase class for unit testing purposes.
    /// It using the AT90USB device as a reference for the test values.
    /// Device driver shall be installed prior using this test. Refer to https://github.com/pbatard/libwdi/wiki/Zadig.
    /// </summary>
    public class UsbDeviceTest : UsbDeviceBase.UsbDeviceBase
    {
        public  int CreateInterfaceCnt = 0;

        public UsbDeviceTest() : base()
        {
        }
        public ushort TestVendorId { get; set; } = 0x03EB;

        public ushort TestProductId = 0x0001;

        public override ushort VendorId => TestVendorId;

        public override ushort ProductId => TestProductId;
        
        public override string DeviceId => "\\\\?\\USB#VID_03EB&PID_0001#7&4110fb5&0&4#{a5dcbf10-6530-11d2-901f-00c04fb951ed}";

        public override string DeviceName => "AT90USB"; // name of the device given when installing the device driver.


        protected override UsbInterfaceBase CreateInterface(UsbInterfaceDescriptor descriptor, IEnumerable<KeyValuePair<int, PipeOutBase>> outPipes, IEnumerable<KeyValuePair<int, PipeInBase>> inPipes)
        {
            CreateInterfaceCnt++;
            return new UsbInterfaceTest(descriptor, outPipes, inPipes);
        }

        public void VerifyDevice()
        {
            this.ThrowIfNoWindowsDevice();
            Assert.Equal(0x03EB, this.VendorId);
            Assert.Equal(0x01, this.ProductId);
            Assert.Equal(ByteOrder.LittleEndian, this.ByteOrder);
            Assert.Equal(Windows.Storage.Streams.UnicodeEncoding.Utf8, this.UnicodeEncoding);
            Assert.Equal(0x01, this.ConfigurationValue);
            Assert.Equal((uint)0x64, this.MaxPowerMilliamps);
            Assert.False(this.RemoteWakeup);
            Assert.False(this.SelfPowered);

            // protected
            Assert.NotNull(this.UsbInterfaceControl);
            Assert.NotNull(this.ImplInterfaces);
            Assert.Single(this.ImplInterfaces);

            UsbInterfaceBase implInterface = this.ImplInterfaces.First().Value;
            Assert.Single(implInterface.OutPipes);
            Assert.Equal(2, implInterface.InPipes.Count);

        }
    }
}
