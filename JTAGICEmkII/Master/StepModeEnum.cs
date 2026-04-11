using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal enum StepModeEnum: byte
    {
        over = 0x00,
        Into = 0x01,
        Out = 0x02
    }
}
