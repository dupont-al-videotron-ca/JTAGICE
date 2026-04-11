using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    internal abstract class VisitorCommandBase : IVisitorCommand
    {


        #region Constructors 

        internal VisitorCommandBase()
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public abstract bool Visit(ActivitySignOn element);
        public abstract bool Visit(ActivitySignOff element);

        public abstract bool Visit(ActivityReset element);

        public abstract bool Visit(ActivityClearEvents element);

        public abstract bool Visit(ActivitySetDeviceDescriptor element);

        public abstract bool Visit(ActivityGetAllParameter element);
        public abstract bool Visit(ActivitySetAllParameter element);
        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
