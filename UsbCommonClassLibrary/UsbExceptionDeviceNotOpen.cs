namespace UsbCommonClassLibrary
{
    internal class UsbExceptionDeviceNotOpen : UsbExeception
    {
        private const string DefaultMessage = "The USB device is not open.";
        private const string DefaultMessage2 = "The USB device is not open. Path ";

        public UsbExceptionDeviceNotOpen() : base(DefaultMessage)
        {
        }
        public UsbExceptionDeviceNotOpen(string path) : base(DefaultMessage2 + path)
        {
        }
        public UsbExceptionDeviceNotOpen(string path, Exception innerException) : base(DefaultMessage2 + path, innerException)
        {
        }
        public UsbExceptionDeviceNotOpen(Exception innerException) : base(DefaultMessage, innerException)
        {
        }
    }
}
