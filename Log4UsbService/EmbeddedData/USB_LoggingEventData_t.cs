using System.Runtime.InteropServices;

namespace Log4UsbService.EmbeddedData;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct USB_LoggingEventData_t
{
    public USB_LoggingEventData_t()
    {
        Header.Version = 1;
        Header.Length = (ushort)Marshal.SizeOf<USB_LoggingEventData_t>();
    }

    public USB_FrameHeader_t Header;
    public byte Level;
    public UInt32 TickCount;
    //public byte StartOfString;
}
