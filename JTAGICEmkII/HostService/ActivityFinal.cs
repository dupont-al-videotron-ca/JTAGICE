


using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public class ActivityFinal : ActivityBaseComp
    {
        public ActivityFinal(StructureActivity activityStructure, IActivityElement parent) : this(activityStructure, parent, false)
        {
        }

        public ActivityFinal(StructureActivity activityStructure, IActivityElement parent, bool isExitPoint) : base(activityStructure, parent)
        {
            ArgumentNullException.ThrowIfNull(parent);
            IsExitPoint = isExitPoint;
        }

        public bool IsExitPoint { get; set; }

        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override void AddNext(IActivityElement element)
            => throw new InvalidOperationException($"Activity {this.GetType()} cannot be exited because it has child activities, which is invalid.");

        public override void AddNextRange(IEnumerable<IActivityElement> elements)
            => throw new InvalidOperationException($"Activity {this.GetType()} cannot be exited because it has child activities, which is invalid.");

        public override bool ActivityEntry()
        {
            if (!this.HasParent)
            {
                Logger.Error($"Activity {this.GetType()} cannot be exited because it has no parent activity, which is invalid.");
                throw new InvalidOperationException($"Activity {this.GetType()} cannot be exited because it has no parent activity, which is invalid.");
            }

            return base.ActivityEntry();
        }

        public override bool ActivityExit(bool lastRequest)
        {
            return true;
        }
   }    
}