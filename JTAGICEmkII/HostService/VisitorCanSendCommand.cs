using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal class VisitorCanSendCommand : VisitorCommandBase
    {


        #region Constructors 

        public VisitorCanSendCommand(Master.IMasterCommand command) :
            base()
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));

        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 
        public Master.IMasterCommand Command { get; }

        public override bool Visit(ActivitySignOn element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivitySignOff element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivityReset element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityClearEvents element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivitySetDeviceDescriptor element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityGetParameter element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivitySetParameter element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityWriteMemory element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityReadMemory element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityWritePC element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityReadPC element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityGo element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivitySingleStep element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityForceStop element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityErasePageSpm element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityGetSync element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivitySelfTest element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivitySetBreak element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivityGetBreak element)
        {
            return element.CanSendCommand(Command);
        }

        public override bool Visit(ActivityChipErase element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivityEnterProgMode element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivityLeaveProgMode element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivityClearBreak element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivityRunToAddr element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivitySPICmd element)
        {
            return element.CanSendCommand(Command);
        }
        public override bool Visit(ActivityRestoreTarget element)
        {
            return element.CanSendCommand(Command);
        }
        #endregion

    }
}
