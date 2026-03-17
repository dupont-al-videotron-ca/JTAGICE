namespace UsbCommonClassLibrary
{
    public abstract class UsbExeception : Exception
    {
        public UsbExeception()
        {
        }
        public UsbExeception(string message) : base(message)
        {
        }
        public UsbExeception(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
