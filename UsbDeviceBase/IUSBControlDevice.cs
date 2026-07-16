using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace UsbDeviceBase
{
    public interface IUSBControlDevice
    {
        uint SendControlOutTransfer(Windows.Devices.Usb.UsbSetupPacket usp);

        Task<uint> SendControlOutTransferAsync(Windows.Devices.Usb.UsbSetupPacket usp);

        IBuffer SendControlInTransfer(Windows.Devices.Usb.UsbSetupPacket usp, int bufferLength);

        Task<IBuffer> SendControlInTransferAsync(Windows.Devices.Usb.UsbSetupPacket usp, int bufferLength);

        T SendControlInTransfer<T>(Windows.Devices.Usb.UsbSetupPacket usp, int bufferLength);

        Task<T?> SendControlInTransferAsync<T>(Windows.Devices.Usb.UsbSetupPacket usp, int bufferLength) where T : struct;

        bool IsConnected { get; }
    }
}
