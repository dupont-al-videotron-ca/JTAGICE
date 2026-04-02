using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal enum BreakpointModeEnum : byte
    {
        BKPT_MODE_PROGRAM = 0x00,
        BKPT_MODE_MEMORY_READ = 0x01,
        BKPT_MODE_MEMORY_WRITE = 0x02,
        BKPT_MODE_READ_WRITE = 0x03,
    }
}
