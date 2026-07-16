using Windows.Devices.Usb;
using Windows.Storage.Streams;

namespace UsbDeviceBase
{
    public interface IUsbPipeOut
    {

        UsbWriteOptions WriteOptions { get; set; }

        int Send(byte[] bytes);

        Task<int> SendAsync(byte[] bytes);

    }
}