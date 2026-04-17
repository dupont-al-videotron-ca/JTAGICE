using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivitySignOn : ActivityProcessCommandBase
    {


        #region Constructors 

        public ActivitySignOn(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_GET_SIGN_ON, SlaveResponseEnum.RSP_SIGN_ON)
        {
        }

        #endregion


        #region Public Methods 

        public override bool Accept(IVisitorCommand visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        #endregion
    }
}
