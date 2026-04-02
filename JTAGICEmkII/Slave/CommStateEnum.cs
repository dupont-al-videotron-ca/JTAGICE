using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal enum CommStateEnum : byte
    {
        Start = 0,
        GetSequence = 1,
        GetSize = 2,
        GetToken = 3,
        GetData = 4,
        GetCRC = 5,
    }
}
