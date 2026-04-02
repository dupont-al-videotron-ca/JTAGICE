using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal enum EventBreakCauseEnum : byte
    {
        UNKNOWN = 0x00,
        PROGRAMME_BREAK= 0x01,
        DATA_BREAK_PDSB = 0x02,
        DATA_BREAK_PDMSB = 0x03,
    }
}
