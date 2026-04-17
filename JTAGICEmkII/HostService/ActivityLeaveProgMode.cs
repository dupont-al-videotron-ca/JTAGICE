using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivityLeaveProgMode : ActivityProcessCommandBase
    {


        #region Constructors 
        public ActivityLeaveProgMode(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_LEAVE_PROGMODE, SlaveResponseEnum.RSP_OK)
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
            return this.ActivityStructure.TargetMcuState.IsProgramming && base.CanSendCommand(command);
        }

        #endregion

    }
}
