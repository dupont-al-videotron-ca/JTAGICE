using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal class VisitorCommandSent : VisitorCommandBase
    {


        #region Constructors 

        internal VisitorCommandSent(Master.IMasterCommand command) : base()
        {
            this.Command = command ?? throw new ArgumentNullException(nameof(command));
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 
        public Master.IMasterCommand Command { get; }


        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public override bool Visit(ActivitySignOn element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivitySignOff element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityReset element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityClearEvents element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivitySetDeviceDescriptor element)
        {
            return element.CommandSent();
        }
        public override bool Visit(ActivityGetParameter element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivitySetParameter element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityWriteMemory element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityReadMemory element)
        {
            return element.CommandSent();
        }
        public override bool Visit(ActivityWritePC element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityReadPC element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityGo element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivitySingleStep element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityForceStop element)
        {
            return element.CommandSent();
        }
        public override bool Visit(ActivityErasePageSpm element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityGetSync element)
        {
            return element.CommandSent();
        }
        public override bool Visit(ActivitySelfTest element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivitySetBreak element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityGetBreak element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityChipErase element)
        {
            return element.CommandSent();
        }
        public override bool Visit(ActivityEnterProgMode element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivityLeaveProgMode element)
        {
            return element.CommandSent();
        }
        public override bool Visit(ActivityClearBreak element)
        {
            return element.CommandSent();
        }
        public override bool Visit(ActivityRunToAddr element)
        {
            return element.CommandSent();
        }

        public override bool Visit(ActivitySPICmd element)
        {
            return element.CommandSent();
        }
        public override bool Visit(ActivityRestoreTarget element)
        {
            return element.CommandSent();
        }
        #endregion

    }
}
