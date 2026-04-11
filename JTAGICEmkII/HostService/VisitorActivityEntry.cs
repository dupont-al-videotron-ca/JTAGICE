using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal class VisitorActivityEntry : VisitorOperationBase
    {


        #region Constructors 

        internal VisitorActivityEntry() : base()
        {
        }
        #endregion

        #region Public Methods 

        public override bool Visit(ActivityInitial element)
        {
            return element.ActivityEntry();
        }

        public override bool Visit(ActivityFinal element)
        {
            return element.ActivityEntry();
        }

        public override bool Visit(ActivityAction element)
        {
            return element.ActivityEntry();
        }

        public override bool Visit(TargetConnecting element)
        {
            return element.ActivityEntry();
        }

        public override bool Visit(TargetConnected element)
        {
            return element.ActivityEntry();
        }
        #endregion

    }
}
