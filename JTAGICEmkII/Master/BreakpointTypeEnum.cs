using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal enum BreakpointTypeEnum : byte
    {
        BKPT_PRG_MEMORY = 0x00,
        BKPT_DATA = 0x01,
        BKPT_MASK_DATA = 0x02,   
    }
}
