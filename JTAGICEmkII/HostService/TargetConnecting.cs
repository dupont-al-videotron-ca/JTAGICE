using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;

namespace JTAGICEmkII.HostService
{
    public sealed class TargetConnecting : ActivityComposite
    {
        public TargetConnecting(StructureActivity activityStructure) : this(activityStructure, null!)
        {
        }

        public TargetConnecting(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {
            this.Initial = new ActivityInitial(activityStructure, parent);
            var signOn = new ActivitySignOn(activityStructure, this.Initial);
            this.Initial.AddNext(signOn);

            var reset = new ActivityReset(activityStructure, signOn);
            var setDeviceDescriptor = new ActivitySetDeviceDescriptor(activityStructure, reset);
            reset.AddNext(setDeviceDescriptor);

            var clearEvents = new ActivityClearEvents(activityStructure, setDeviceDescriptor);
            var getAllParameter = new ActivityGetAllParameter(activityStructure, clearEvents);
            clearEvents.AddNext(getAllParameter);

            var modifyParameter = new ActivityAction(activityStructure, getAllParameter, () => activityStructure.HostService.ModifyAllParameter());
            var setAllParameter = new ActivitySetAllParameter(activityStructure, modifyParameter);
            modifyParameter.AddNext(setAllParameter);

            this.Final = new ActivityFinal(activityStructure, setAllParameter);

            activityStructure.AddActivity(this.Initial);
            activityStructure.AddActivity(signOn);
            activityStructure.AddActivity(reset);
            activityStructure.AddActivity(setDeviceDescriptor);
            activityStructure.AddActivity(clearEvents);
            activityStructure.AddActivity(getAllParameter);
            activityStructure.AddActivity(modifyParameter);
            activityStructure.AddActivity(setAllParameter);
            activityStructure.AddActivity(this.Final);
        }

        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool ActivityAction()
        {
            Logger.Debug($"{this.GetType()} Executing activity entry called.");

            if (this.HasChild)
            {
                IActivityElement? next = this.Nexts.First();

                while(next != null)
                {
                    if (next is ActivitySignOn)
                    {
                        if (!this.ActivityStructure.HostService.SignOn(out Slave.ResponseSignOn? response))
                        {
                            Logger.Error("Failed to sign on to target.");
                            return false;
                        }

                        this.ActivityStructure.HostService.SignOnResponse = response!;
                        next = ((ActivitySignOn)next).Nexts[0];
                    }
                    else if (next is ActivityReset)
                    {
                        if (!this.ActivityStructure.HostService.Reset())
                        {
                            Logger.Error("Failed to reset target.");
                            return false;
                        }
                        next = ((ActivityReset)next).Nexts[0];
                    }
                    else if (next is ActivitySetDeviceDescriptor)
                    {
                        if (!this.ActivityStructure.HostService.SetDeviceDescriptor())
                        {
                            Logger.Error("Failed to set device descriptor.");
                            return false;
                        }
                        next = ((ActivitySetDeviceDescriptor)next).Nexts[0];
                    }
                    else if (next is ActivityClearEvents)
                    {
                        if (!this.ActivityStructure.HostService.ClearEvents())
                        {
                            Logger.Error("Failed to clear events.");
                            return false;
                        }
                        next = ((ActivityClearEvents)next).Nexts[0];
                    }
                    else if (next is ActivityGetAllParameter)
                    {
                        if (!this.ActivityStructure.HostService.GetAllParameter())
                        {
                            Logger.Error("Failed to get all parameters.");
                            return false;
                        }
                        next = ((ActivityGetAllParameter)next).Nexts[0];
                    }
                    else if (next is ActivityAction)
                    {
                        if (!next.Accept(new VisitorActivityEntry()))
                        {
                            Logger.Error("Failed to perform action.");
                            return false;
                        }
                        next = ((ActivityAction)next).Nexts[0];

                    }
                    else if (next is ActivitySetAllParameter)
                    {
                        if (!this.ActivityStructure.HostService.SetAllParameter())
                        {
                            Logger.Error("Failed to set all parameters.");
                            return false;
                        }
                        next = ((ActivitySetAllParameter)next).Nexts[0]; 
                    }
                    else if (next == this.Final)
                    {
                        if (!this.Accept(new VisitorActivityExit(true)))
                        {
                            Logger.Error("Failed to exit final activity.");
                            return false;
                        }
                        next = null;
                    }
                    else
                    {
                        Logger.Error($"Unknown activity {next.GetType()} in target connecting sequence.");
                        return false;
                    }
                }

                if (this.Nexts.Count > 0)
                {
                    _nextIndex = 0;
                    return true;
                }
                else
                {
                    Logger.Error("No next activity to transition to after target connecting sequence.");
                    return false;
                }

            }
            else
            {
                throw new InvalidOperationException("Cannot exit activity because there are no entry activities to transition to.");
            }
        }
    }
}
