using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    public enum McuStateEnum : byte
    {
        STOPPED = 0x00,
        RUNNING = 0x01,
        PROGRAMMING = 0x02,
        Unknown = 0x3,
    }
}
