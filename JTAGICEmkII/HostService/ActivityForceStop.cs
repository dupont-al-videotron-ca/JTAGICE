using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivityForceStop : ActivityProcessCommandBase
    {


        #region Constructors 
        public ActivityForceStop(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_FORCED_STOP, SlaveResponseEnum.RSP_OK)
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
            return (this.ActivityStructure.TargetMcuState.IsStopped || this.ActivityStructure.TargetMcuState.IsRunning)
                && base.CanSendCommand(command);
        }

        #endregion

    }
}
