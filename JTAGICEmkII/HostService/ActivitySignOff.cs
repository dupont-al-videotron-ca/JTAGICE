using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivitySignOff : ActivityProcessCommand
    {


        #region Constructors 
        public ActivitySignOff(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_SIGN_OFF, SlaveResponseEnum.RSP_OK)
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
