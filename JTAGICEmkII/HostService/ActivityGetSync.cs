using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivityGetSync : ActivityProcessCommandBase
    {


        #region Constructors 
        public ActivityGetSync(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_GET_SYNC, SlaveResponseEnum.RSP_OK)
        {
        }
        #endregion


        #region Public Methods 
        public override bool Accept(IVisitorCommand visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool CanSendCommand(IMasterCommand command)
        {
            return base.CanSendCommand(command);
        }

        #endregion

    }
}
