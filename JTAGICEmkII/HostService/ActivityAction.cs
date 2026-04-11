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


        #region Fields 

        #endregion


        #region Properties 

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool ActivityEntry()
        {
            if(_action == null)
                throw new InvalidOperationException("Action cannot be null.");

            var result = _action();

            if(result && this.HasChild)
            {
                _nextIndex = 0;
            }
            
            return result;
        }

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion
    }
}
