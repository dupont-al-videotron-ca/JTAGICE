using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal enum ExceutionModeEnum : byte
    {
        EXMODE_LOW_LEVEL = 0x01,
        EXMODE_HIGH_LEVEL = 0x02,
        EXMODE_RESET = 0x04,

    }
}
