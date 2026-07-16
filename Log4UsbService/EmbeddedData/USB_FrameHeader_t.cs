using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Log4UsbService.EmbeddedData;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct USB_FrameHeader_t
{
    public ushort Length;
    public byte Reseved;
    public byte Version;
}
