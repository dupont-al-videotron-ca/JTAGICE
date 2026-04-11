using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;

namespace JTAGICEmkII.HostService
{
    public sealed class TargetConnected : ActivityComposite
    {
        public TargetConnected(StructureActivity activityStructure) : this(activityStructure, null)
        {
        }

        public TargetConnected(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {
            this.Initial = new ActivityInitial(activityStructure, parent);

            //TODO: Add activities .

            this.Final = new ActivityFinal(activityStructure, Initial);
            Initial.AddNext(Final);

            activityStructure.AddActivity(this.Initial);
            activityStructure.AddActivity(this.Final);
        }

        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool ActivityEntry()
        {
            Logger.Debug($"{this.GetType()} Executing activity entry called.");

            if (this.Initial != null && this.Initial.HasChild)
            {
                foreach (var next in this.Nexts)
                {
                    if (next is ActivitySignOn)
                    {
                        if (!this.ActivityStructure.HostService.SignOn(out Slave.ResponseSignOn? response))
                            throw new InvalidOperationException("Failed to sign on to target.");
                        this.ActivityStructure.HostService.SignOnResponse = response!;
                    }
                    else if (next is ActivityReset)
                    {
                        if (!this.ActivityStructure.HostService.Reset())
                            throw new InvalidOperationException("Failed to sign on to target.");
                    }
                    else if (next is ActivitySetDeviceDescriptor)
                    {
                        if (!this.ActivityStructure.HostService.SetDeviceDescriptor())
                            throw new InvalidOperationException("Failed to set device descriptor.");
                    }
                    else if (next is ActivityClearEvents)
                    {
                        if (!this.ActivityStructure.HostService.ClearEvents())
                            throw new InvalidOperationException("Failed to clear events.");
                    }
                    else if (next is ActivityGetAllParameter)
                    {
                        if (!this.ActivityStructure.HostService.GetAllParameter())
                            throw new InvalidOperationException("Failed to get all parameters.");
                    }
                    else if (next is ActivityAction)
                    {
                        if (!next.Accept(new VisitorActivityEntry()))
                            throw new InvalidOperationException("Failed to perform action.");
                    }
                    else if (next is ActivitySetAllParameter)
                    {
                        if (!this.ActivityStructure.HostService.SetAllParameter())
                            throw new InvalidOperationException("Failed to set all parameters.");
                    }
                    else if (next == this.Final)
                    {
                        this.Final.Accept(new VisitorActivityExit(true));
                        if (!this.Final.Accept(new VisitorActivityExit(true)))
                            throw new InvalidOperationException("Failed to exit final activity.");
                    }
                }

                this.ActivityStructure.CurrentActivity = this.Final?.Nexts[0];
//                this.ActivityStructure.CurrentActivity.Accept(new VisitorActivityEntry());
                return true;
            }
            else
                throw new InvalidOperationException("Cannot exit activity because there are no entry activities to transition to.");
        }
    }
}
