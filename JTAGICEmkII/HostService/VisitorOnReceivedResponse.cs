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

        #endregion


    }
}
