using UsbDeviceBase;
using Windows.Devices.Usb;

namespace MyUsbDevice
{
    public class At90UsbKeyDevice : UsbDeviceBase.UsbDeviceBase
    {
        private bool _flashInterface;
        public At90UsbKeyDevice(bool flashInterface = true) : base()
        {
            this.Initialize();
            this._flashInterface = flashInterface;
        }

        public override ushort VendorId => 0x03EB;

        public override ushort ProductId => 0x0001;

        public override string DeviceId => "\\\\?\\usb#vid_03eb&pid_0001#001#{a5dcbf10-6530-11d2-901f-00c04fb951ed}";
        public override string DeviceName => "AT90USB";

        public override void Initialize()
        {
            base.Initialize();

        }

        protected override UsbInterfaceBase CreateInterface(Windows.Devices.Usb.UsbInterfaceDescriptor descriptor, IEnumerable<KeyValuePair<int, PipeOutBase>> outPipes, IEnumerable<KeyValuePair<int, PipeInBase>> inPipes)
        {

            if (_flashInterface)
            {
                return new At90UsbFlashInterface(descriptor, outPipes, inPipes);
            }
            else
            {
                throw new NotImplementedException("Only flash interface is implemented for now.");
            }
        }
    }
}
