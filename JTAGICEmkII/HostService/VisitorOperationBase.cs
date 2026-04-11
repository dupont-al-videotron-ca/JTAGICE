using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal abstract class VisitorOperationBase : IVisitorActivity
    {


        #region Constructors 

        internal VisitorOperationBase()
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public abstract bool Visit(ActivityInitial element);
        public abstract bool Visit(ActivityFinal element);
        public abstract bool Visit(ActivityAction element);
        public abstract bool Visit(TargetConnecting element);
        public abstract bool Visit(TargetConnected element);

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
