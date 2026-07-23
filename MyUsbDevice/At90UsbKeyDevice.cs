using UsbDeviceBase;
using Windows.Devices.Usb;

namespace MyUsbDevice
{
    public class At90UsbKeyDevice : UsbDeviceBase.UsbDeviceBase
    {


        #region Constructors 
        public At90UsbKeyDevice() : this(false)
        {
        }

        public At90UsbKeyDevice(bool flashInstalled = false) : base()
        {
            this._flashInterface = flashInstalled;
        }

        #endregion


        #region Fields 
        private bool _flashInterface;

        public const int InterfaceId = 0;
        public const int LogPipeInId = 1;

        #endregion


        #region Properties 

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public override ushort VendorId => 0x03EB;

        public override ushort ProductId => 0x0001;

        public override string DeviceId => "\\\\?\\usb#vid_03eb&pid_0001#001#{a5dcbf10-6530-11d2-901f-00c04fb951ed}";

        public override string DeviceName => "AT90USB";


        #endregion


        #region Protected Methods 

        public IUsbPipeIn GetLog4UsbPipe()
        {
            BulkInPipeImpl pipe = GetBulkInPipe(InterfaceId, LogPipeInId);
            if (pipe == null)
            {
                throw new InvalidOperationException("No USB interfaces available.");
            }

            return pipe;
        }

        protected override UsbInterfaceBase CreateInterface(Windows.Devices.Usb.UsbInterfaceDescriptor descriptor, IEnumerable<KeyValuePair<int, PipeOutBase>> outPipes, IEnumerable<KeyValuePair<int, PipeInBase>> inPipes)
        {
            return new At90UsbInterface(descriptor, outPipes, inPipes);
        }


        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion
    }
}
