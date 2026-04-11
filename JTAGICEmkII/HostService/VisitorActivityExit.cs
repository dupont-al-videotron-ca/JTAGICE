using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal class VisitorActivityExit : VisitorOperationBase
    {


        #region Constructors 

        internal VisitorActivityExit(bool lastRequest) : base()
        {
            this.LastRequest = lastRequest;
        }
        #endregion

        #region Properties 

        public bool LastRequest { get; }

        #endregion

        #region Public Methods 

        public override bool Visit(ActivityInitial element)
        {
            return element.ActivityExit(this.LastRequest);
        }

        public override bool Visit(ActivityFinal element)
        {
            return element.ActivityExit(this.LastRequest);
        }

        public override bool Visit(ActivityAction element)
        {
            return element.ActivityExit(this.LastRequest);
        }

        public override bool Visit(TargetConnecting element)
        {
            return element.ActivityExit(this.LastRequest);
        }

        public override bool Visit(TargetConnected element)
        {
            return element.ActivityExit(this.LastRequest);
        }

        #endregion

    }
}
