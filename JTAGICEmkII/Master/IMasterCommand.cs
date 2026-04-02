using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.Master
{
    internal interface IMasterCommand
    {
        MasterCommandEnum MessageId { get; }

        int MessageLength { get; }
    }
}
