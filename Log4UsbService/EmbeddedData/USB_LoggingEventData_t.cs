using System.Runtime.InteropServices;

namespace Log4UsbService.EmbeddedData;

/// <summary>
/// Represents the data structure for a USB logging event, including its header, logging level, and tick count.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct USB_LoggingEventData_t
{
    public USB_LoggingEventData_t()
    {
        Header.Version = 1;
        Header.Length = (ushort)Marshal.SizeOf<USB_LoggingEventData_t>();
    }

    public override string ToString()
    {
        return $"Header: {Header.ToString()}, Level: {Level}, TickCount: {TickCount}";
    }
    public bool IsValid
    {
        get
        {
            return Header.Version == 1 
                && Header.Length >= (ushort)Marshal.SizeOf<USB_LoggingEventData_t>() && Header.Length <= 1024
                && Header.Reseved == 0 
                && Level <= (byte) USB_LevelType.All;
        }
    }   

    public USB_FrameHeader_t Header;
    public byte Level;
    public UInt32 TickCount;
    //public byte StartOfString;
}
