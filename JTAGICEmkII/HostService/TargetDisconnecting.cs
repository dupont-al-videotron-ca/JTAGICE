using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class TargetDisonnecting : ActivityBaseComp
    {
        public TargetDisonnecting(StructureActivity activityStructure) : this(activityStructure, null!)
        {
        }

        public TargetDisonnecting(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {
        }

        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool ActivityEntry()
        {
            this.ActivityStructure.TargetMcuState.ResetState();
            return base.ActivityEntry();
        }
        public override bool ActivityAction()
        {
            if (!this.ActivityStructure.HostService.SignOff().IsSuccess)
            {
                this.ActivityStructure.Logger.Error("Failed to sign off from the target device.");
            }
            else
            {
                this.ActivityStructure.Logger.Info("Successfully signed off from the target device.");
            }

            this.NextActivity = this.Find<HostSession>();
            if (this.NextActivity is HostSession hostSession)
            {
                hostSession.IsSessionActive = false;
            }

            return true;
        }

        public override bool ActivityExit(bool lastRequest) => base.ActivityExit(lastRequest);
    }
}
