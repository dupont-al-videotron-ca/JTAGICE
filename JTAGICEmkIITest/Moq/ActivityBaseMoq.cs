using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.HostService;
using log4net;


namespace JTAGICEmkIITest.Moq
{
    internal sealed class ActivityBaseMoq : ActivityBase
    {

        #region Constructors 

        internal ActivityBaseMoq(StructureActivity activityStructure) : base(activityStructure)
        {
        }

        #endregion


        #region Properties 

        public ILog LoggerTest => this.Logger;
        public StructureActivity ActivityStructureTest => this.ActivityStructure;

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override bool Accept(IVisitorActivity visitor) => true;

        #endregion
    }
}
