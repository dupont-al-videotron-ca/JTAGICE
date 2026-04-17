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
        bool Visit(ActivityWriteMemory element);
        bool Visit(ActivityReadMemory element);
        bool Visit(ActivityWritePC element);
        bool Visit(ActivityReadPC element);
        bool Visit(ActivityGo element);
        bool Visit(ActivitySingleStep element);
        bool Visit(ActivityForceStop element);
        bool Visit(ActivityErasePageSpm element);
        bool Visit(ActivityGetSync element);
        bool Visit(ActivitySelfTest element);
        bool Visit(ActivitySetBreak element);
        bool Visit(ActivityGetBreak element);

        bool Visit(ActivityChipErase element);

        bool Visit(ActivityEnterProgMode element);
        bool Visit(ActivityLeaveProgMode element);
        bool Visit(ActivityClearBreak element);
        bool Visit(ActivityRunToAddr element);
        bool Visit(ActivitySPICmd element);
        bool Visit(ActivityRestoreTarget element);


    }
}
