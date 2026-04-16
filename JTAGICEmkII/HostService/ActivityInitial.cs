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
            if(this.HasParent)
            {
                if (this.Parents.Count == 1)
                {
                    this.NextActivity = this.Parents[0];
                }
                else
                {
                    // TODO: support multiple parent activities.
                    Logger.Error($"Activity {this.GetType()} has multiple parent activities, which is not supported yet.");
                    throw new NotImplementedException($"Activity {this.GetType()} has multiple parent activities, which is not supported yet.");
                }
            }

            if (this.HasChild)
            {
                if (this.Nexts.Count > 1)
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

            return base.ActivityEntry();

        }

        public override bool ActivityExit(bool lastRequest)
        {
            this.NextActivity = this.Nexts[0];
            return base.ActivityExit(lastRequest);
        }

    }
}