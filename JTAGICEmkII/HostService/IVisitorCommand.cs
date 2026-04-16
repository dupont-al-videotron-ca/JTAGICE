using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    public interface IVisitorCommand
    {
        //        bool Visit(IAcitityElement element);
        bool Visit(ActivitySignOn element);
        bool Visit(ActivitySignOff element);
        bool Visit(ActivityReset element);
        bool Visit(ActivityClearEvents element);

        bool Visit(ActivitySetDeviceDescriptor element);

        bool Visit(ActivityGetParameter element);
        bool Visit(ActivitySetParameter element);

    }
}
