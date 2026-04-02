using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal enum StepModeEnum: byte
    {
        STEP_OVER = 0x00,
        STEP_INTO = 0x01,
        STEP_OUT = 0x02
    }
}
