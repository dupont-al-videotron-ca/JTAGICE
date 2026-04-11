using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal abstract class ActivitySlipMerge : ActivityBaseComp
    {

        #region Constructors 

        internal ActivitySlipMerge(StructureActivity activityStructure, IEnumerable<IActivityElement> elements) : base(activityStructure)
        {
            ArgumentNullException.ThrowIfNull(elements);
            _activities = new List<IActivityElement>(elements);
        }

        #endregion


        #region Fields 

        private List<IActivityElement> _activities;

        #endregion


        #region Properties 

        public IReadOnlyList<IActivityElement> Activities => _activities;

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        /// <summary>
        /// Chain of responsability pattern, each activity will be visited until one of them returns non-zero result, 
        /// which means the visitor has handled the activity and no need to visit the rest of the activities.
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            bool result = false;
            foreach (var activity in _activities)
            {
                result = activity.Accept(visitor);

                if(result)
                {
                    break;
                }
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
