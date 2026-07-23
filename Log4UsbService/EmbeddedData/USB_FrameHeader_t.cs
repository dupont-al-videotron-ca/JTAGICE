using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Log4UsbService.EmbeddedData;

/// <summary>
/// Represents the header of a USB frame.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct USB_FrameHeader_t
{
    public ushort Length;
    public byte Reseved;
    public byte Version;

    override public string ToString()
    {
        return $"Length: {Length}, Reseved: {Reseved}, Version: {Version}";
    }
}
