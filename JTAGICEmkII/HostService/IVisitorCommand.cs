using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal interface IVisitorCommand
    {
        //        bool Visit(IAcitityElement element);
        bool Visit(ActivitySignOn element);
        bool Visit(ActivitySignOff element);
        bool Visit(ActivityReset element);
        bool Visit(ActivityClearEvents element);

        bool Visit(ActivitySetDeviceDescriptor element);

        bool Visit(ActivityGetAllParameter element);
        bool Visit(ActivitySetAllParameter element);

    }
}
