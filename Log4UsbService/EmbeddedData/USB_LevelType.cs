using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4UsbService.EmbeddedData
{
    public enum USB_LevelType : byte
    {
        None = 0x00,
        Fatal = 0x01,
        Error = 0x02,
        Warning = 0x03,
        Info = 0x04,
        Debug  = 0x05,
        All = 0x06
    }
}
