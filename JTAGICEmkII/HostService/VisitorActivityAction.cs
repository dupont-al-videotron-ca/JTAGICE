using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal class VisitorActivityAction : VisitorOperationBase
    {


        #region Constructors 

        internal VisitorActivityAction() : base()
        {
        }

        public override bool Visit(ActivityInitial element)
        {
            return element.ActivityAction();
        }
        public override bool Visit(ActivityAction element)
        {
            return element.ActivityAction();
        }

        public override bool Visit(TargetConnecting element)
        {
            return element.ActivityAction();
        }

        public override bool Visit(TargetConnected element)
        {
            return element.ActivityAction();
        }

        public override bool Visit(TargetDisonnecting element)
        {
            return element.ActivityAction();
        }

        public override bool Visit(TargetStopped element)
        {
            return element.ActivityAction();
        }

        public override bool Visit(TargetRunning element) 
        {
            return element.ActivityAction();
        }
        public override bool Visit(TargetProgramming element)
        {
            return element.ActivityAction();
        }

        public override bool Visit(HostSession element)
        {
            return element.ActivityAction();
        }


        #endregion


    }
}
