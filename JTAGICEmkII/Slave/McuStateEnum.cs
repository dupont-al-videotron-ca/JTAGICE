using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal enum McuStateEnum : byte
    {
        STOPPED = 0x00,
        RUNNING = 0x01,
        PROGRAMMING = 0x02,
    }
}
