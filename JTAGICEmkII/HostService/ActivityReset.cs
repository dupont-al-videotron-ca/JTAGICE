using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivityReset : ActivityProcessCommandBase
    {

        #region Constructors 

        public ActivityReset(StructureActivity activityStructure) :
            base(activityStructure, MasterCommandEnum.CMND_RESET, SlaveResponseEnum.RSP_OK)
        {
        }
        #endregion

        #region Public Methods 

        public override bool Accept(IVisitorCommand visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool OnReceivedResponse(ISlaveResponse response)
        {
            switch (response.ResponseId)
            {
                case SlaveResponseEnum.RSP_OK:
                    this.ActivityStructure.TargetMcuState.GoStopped();
                    return true;
                default:
                    return base.OnReceivedResponse(response);
            }
        }

        #endregion

    }
}
