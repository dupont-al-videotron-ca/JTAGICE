using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII
{
    internal enum RxStateEnum
    {
        WaitStart,
        WaitSequenceNumber,
        WaitMessageSize,
        WaitToken,
        WaitMessage,
        WaitCRC,
        Stop,
    }
}
