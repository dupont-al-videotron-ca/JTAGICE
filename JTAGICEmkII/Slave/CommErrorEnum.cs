using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal enum CommErrorEnum : byte
    {
        ErrorInByte = 1,
        Timeout = 2,
    }
}
