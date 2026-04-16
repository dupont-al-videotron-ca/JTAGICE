using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public class ActivityAction : ActivityBaseComp
    {
        private readonly Func<bool> _action;

        #region Constructors 
        public ActivityAction(StructureActivity activityStructure, IActivityElement parent, Func<bool> action) : base(activityStructure, parent)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }
        #endregion


        #region Public Methods 
        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool ActivityEntry()
        {
            if(_action == null || !this.HasChild || !this.HasParent)
                throw new InvalidOperationException("Action cannot be null or there are no child activities or parent.");

            var result = _action();

            if(result)
            {
                NextActivity = this.Child;
            }
            
            return result;
        }

        #endregion

    }
}
