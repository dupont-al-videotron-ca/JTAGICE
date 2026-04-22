using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class TargetConnecting : ActivityBaseComp
    {
        public TargetConnecting(StructureActivity activityStructure) : this(activityStructure, null!)
        {
        }

        public TargetConnecting(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {
        }

        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool ActivityAction()
        {
            bool retval = false;
            if (!this.ActivityStructure.HostService.SignOn(out ResponseSignOn? response).IsSuccess)
            {
                this.ActivityStructure.Logger.Error("Failed to sign on to the target device.");
            }
            else if(!this.ActivityStructure.HostService.Reset().IsSuccess)
            {
                this.ActivityStructure.Logger.Error("Failed to reset the target device.");
            }
            else if(!this.ActivityStructure.HostService.SetDeviceDescriptor().IsSuccess)
            {
                this.ActivityStructure.Logger.Error("Failed to set device descriptor.");
            }
            else if (!this.ActivityStructure.HostService.ClearEvents().IsSuccess)
            {
                this.ActivityStructure.Logger.Error("Failed to clear events.");
            }
            else if (!this.ActivityStructure.HostService.GetAllParameter().IsSuccess)
            {
                this.ActivityStructure.Logger.Error("Failed to get all parameters.");
            }
            else if (!this.ActivityStructure.HostService.ModifyAllParameter())
            {
                this.ActivityStructure.Logger.Error("Failed to modify all parameters.");
            }
            else if (!this.ActivityStructure.HostService.SetAllParameter().IsSuccess)
            {
                this.ActivityStructure.Logger.Error("Failed to set all parameters.");
            }
            else
            {
                this.ActivityStructure.Logger.Info("Successfully signed on to the target device.");
                NextActivity = this.Find<TargetConnected>(); 
                retval = true;
            }

            var host = this.Find<HostSession>();
            if (host is HostSession hostSession)
            {
                hostSession.IsSessionActive = retval;
            }

            if (!retval)
            {
                this.NextActivity = host;
            }

            this.NextActivity = this.Find<TargetConnected>();
            return retval;
        }

        public override bool ActivityEntry() => base.ActivityEntry();
        
        public override bool ActivityExit(bool lastRequest) => base.ActivityExit(lastRequest);
    }
}
