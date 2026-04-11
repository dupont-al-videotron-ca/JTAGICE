using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public class ActivityInitial : ActivityBaseComp
    {
        public ActivityInitial(StructureActivity activityStructure) : this(activityStructure, null)
        {

        }
        public ActivityInitial(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {

        }

        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool ActivityEntry()
        {
            if (this.HasChild)
            {
                if (this.Nexts.Count == 1)
                {
                    this.ActivityStructure.CurrentActivity = this.Nexts[0];
                    return this.ActivityStructure.CurrentActivity.ActivityEntry();
                }
                else
                {
                    // TODO: support multiple child activities.
                    Logger.Error($"Activity {this.GetType()} has multiple child activities, which is not supported yet.");
                    throw new NotImplementedException($"Activity {this.GetType()} has multiple child activities, which is not supported yet.");
                }
            }
            else
            {
                Logger.Error($"Activity {this.GetType()} has no child activity to enter.");
                throw new InvalidOperationException($"Activity {this.GetType()} has no child activity to enter.");
            }
            
//            return false;
        }

        public override bool ActivityExit(bool lastRequest)
        {
            return base.ActivityExit(lastRequest);
        }

    }
}