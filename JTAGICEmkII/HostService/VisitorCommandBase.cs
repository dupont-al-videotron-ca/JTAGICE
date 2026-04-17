using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal abstract class VisitorCommandBase : IVisitorCommand
    {

        #region Constructors 

        internal VisitorCommandBase()
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public abstract bool Visit(ActivitySignOn element);
        public abstract bool Visit(ActivitySignOff element);

        public abstract bool Visit(ActivityReset element);

        public abstract bool Visit(ActivityClearEvents element);

        public abstract bool Visit(ActivitySetDeviceDescriptor element);

        public abstract bool Visit(ActivityGetParameter element);
        public abstract bool Visit(ActivitySetParameter element);
        public abstract bool Visit(ActivityWriteMemory element);
        public abstract bool Visit(ActivityReadMemory element);
        public abstract bool Visit(ActivityWritePC element);
        public abstract bool Visit(ActivityReadPC element);
        public abstract bool Visit(ActivityGo element);
        public abstract bool Visit(ActivitySingleStep element);
        public abstract bool Visit(ActivityForceStop element);
        public abstract bool Visit(ActivityErasePageSpm element);
        public abstract bool Visit(ActivityGetSync element);
        public abstract bool Visit(ActivitySelfTest element);
        public abstract bool Visit(ActivitySetBreak element);
        public abstract bool Visit(ActivityGetBreak element);
        public abstract bool Visit(ActivityChipErase element);
        public abstract bool Visit(ActivityEnterProgMode element);
        public abstract bool Visit(ActivityLeaveProgMode element);
        public abstract bool Visit(ActivityClearBreak element);
        public abstract bool Visit(ActivityRunToAddr element);
        public abstract bool Visit(ActivitySPICmd element);
        public abstract bool Visit(ActivityRestoreTarget element);


        #endregion


    }
}
