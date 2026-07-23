namespace UsbDeviceBase
{
    public class DeviceConnectEventArgs : EventArgs
    {
        public DeviceConnectEventArgs(bool isConnected)
        {
            IsConnected = isConnected;
        }

        public bool IsConnected { get; private set; }

    }
}