using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal enum SelfTestReponseEnum : byte
    {
        SKIPPED = 0x00,
        OK = 0x01,
        Failed = 0x80,
        //TODO: Add more specific failure codes see p.52 for complete list.
    }
}
