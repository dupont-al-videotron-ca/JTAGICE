using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal enum BreakpointTypeEnumS : byte
    {
        UNDEFINED = 0x00,
        BKPT_PRG_MEMORY = 0x01,
        BKPT_DATA = 0x02,
    }
}
