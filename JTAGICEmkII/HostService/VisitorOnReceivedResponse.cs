using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using JTAGICEmkII.Master;

namespace JTAGICEmkII.HostService
{
    internal class VisitorOnReceivedResponse : VisitorCommandBase
    {


        #region Constructors 

        internal VisitorOnReceivedResponse(Slave.ISlaveResponse response) : base()
        {
            this.Response = response ?? throw new ArgumentNullException(nameof(response));
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 
        public Slave.ISlaveResponse Response { get; }

        public override bool Visit(ActivitySignOn element)
        {
            return element.OnReceivedResponse(this.Response);
        }

        public override bool Visit(ActivitySignOff element)
        {
            return element.OnReceivedResponse(this.Response);
        }

        public override bool Visit(ActivityReset element)
        {
            return element.OnReceivedResponse(this.Response);
        }

        public override bool Visit(ActivityClearEvents element)
        {
            return element.OnReceivedResponse(this.Response);
        }

        public override bool Visit(ActivitySetDeviceDescriptor element)
        {
            return element.OnReceivedResponse(this.Response);
        }

        public override bool Visit(ActivityGetParameter element)
        {
            return element.OnReceivedResponse(this.Response);
        }

        public override bool Visit(ActivitySetParameter element)
        {
            return element.OnReceivedResponse(this.Response);
        }

        public override bool Visit(ActivityWriteMemory element)
        {
            return element.OnReceivedResponse(this.Response);
        }

        public override bool Visit(ActivityReadMemory element)
        {
            return element.OnReceivedResponse(this.Response);
        }

        public override bool Visit(ActivityWritePC element)
        {
            return element.OnReceivedResponse(this.Response);
        }

        public override bool Visit(ActivityReadPC element)
        {
            return element.OnReceivedResponse(this.Response);
        }

        public override bool Visit(ActivityGo element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivitySingleStep element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivityForceStop element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivityErasePageSpm element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivityGetSync element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivitySelfTest element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivitySetBreak element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivityGetBreak element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivityChipErase element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivityEnterProgMode element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivityLeaveProgMode element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivityClearBreak element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivityRunToAddr element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivitySPICmd element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        public override bool Visit(ActivityRestoreTarget element)
        {
            return element.OnReceivedResponse(this.Response);
        }
        #endregion


    }
}
