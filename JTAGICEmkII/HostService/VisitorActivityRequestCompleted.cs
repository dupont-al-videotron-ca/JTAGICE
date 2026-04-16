using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal class VisitorActivityRequestCompleted: VisitorOperationBase
    {
        #region Constructors 

        internal VisitorActivityRequestCompleted(CommandRequest<Master.IMasterCommand, Slave.ISlaveResponse> request) : base()
        {
            this.Request = request;
        }

        public HostService.CommandRequest<Master.IMasterCommand, Slave.ISlaveResponse> Request { get; }

        #endregion

        #region Public Methods 

        public override bool Visit(ActivityInitial element)
        {
            return element.RequestCompleted(Request);
        }

        public override bool Visit(ActivityAction element)
        {
            return element.RequestCompleted(Request);
        }

        public override bool Visit(TargetConnecting element)
        {
            return element.RequestCompleted(Request);
        }

        public override bool Visit(TargetConnected element)
        {
            return element.RequestCompleted(Request);
        }

        public override bool Visit(TargetDisonnecting element) 
        {
            return element.RequestCompleted(Request);
        }

        public override bool Visit(TargetStopped element)
        {
            return element.RequestCompleted(Request);
        }

        public override bool Visit(TargetRunning element) 
        {
            return element.RequestCompleted(Request);
        }
        public override bool Visit(TargetProgramming element)
        {
            return element.RequestCompleted(Request);
        }

        public override bool Visit(HostSession element)
        {
            return element.RequestCompleted(Request);
        }
        #endregion

    }
}
