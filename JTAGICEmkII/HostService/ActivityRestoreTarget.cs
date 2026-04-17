
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivityRestoreTarget : ActivityProcessCommandBase
    {


        #region Constructors 
        public ActivityRestoreTarget(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_RESTORE_TARGET, SlaveResponseEnum.RSP_OK)
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
