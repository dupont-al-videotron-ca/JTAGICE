using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal enum EmulatorModeEnum : byte
    {
        MODE_DEBUG_WIRE = 0x00,
        MODE_JTAG = 0x01,
        UNKNOWN = 0x02
    }
}
