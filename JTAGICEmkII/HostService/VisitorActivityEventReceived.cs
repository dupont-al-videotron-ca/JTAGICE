using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal class VisitorActivityEventReceived: VisitorOperationBase
    {
        #region Constructors 

        internal VisitorActivityEventReceived(Slave.ISlaveResponse responce) : base()
        {
            this.EventResponse = responce;
        }

        public Slave.ISlaveResponse EventResponse { get; }

        #endregion

        #region Public Methods 

        public override bool Visit(ActivityInitial element)
        {
            return element.EventReceived(EventResponse);
        }

        public override bool Visit(ActivityAction element)
        {
            return element.EventReceived(EventResponse);
        }

        public override bool Visit(TargetConnecting element)
        {
            return element.EventReceived(EventResponse);
        }

        public override bool Visit(TargetConnected element)
        {
            return element.EventReceived(EventResponse);
        }

        public override bool Visit(TargetDisonnecting element) 
        {
            return element.EventReceived(EventResponse);
        }

        public override bool Visit(TargetStopped element)
        {
            return element.EventReceived(EventResponse);
        }

        public override bool Visit(TargetRunning element) 
        {
            return element.EventReceived(EventResponse);
        }
        public override bool Visit(TargetProgramming element)
        {
            return element.EventReceived(EventResponse);
        }

        public override bool Visit(HostSession element)
        {
            return element.EventReceived(EventResponse);
        }

        #endregion

    }
}
