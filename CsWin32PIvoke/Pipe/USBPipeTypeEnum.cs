using Windows.Win32.Devices.Usb;
namespace CsWin32Api
{
    public enum USBPipeTypeEnum : byte
    {
        UsbdPipeTypeControl = USBD_PIPE_TYPE.UsbdPipeTypeControl,
        UsbdPipeTypeIsochronous = USBD_PIPE_TYPE.UsbdPipeTypeIsochronous,
        UsbdPipeTypeBulk = USBD_PIPE_TYPE.UsbdPipeTypeBulk,
        UsbdPipeTypeInterrupt = USBD_PIPE_TYPE.UsbdPipeTypeInterrupt,
        UsbdPipeTypeMask = 0x03
    }
}
