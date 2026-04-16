using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivityClearEvents : ActivityProcessCommandBase
    {


        #region Constructors 
        public ActivityClearEvents(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_CLEAR_EVENTS, SlaveResponseEnum.RSP_OK)
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
            switch(command.MessageId)
            {
                case MasterCommandEnum.CMND_CLEAR_EVENTS:
                    return this.ActivityStructure.TargetMcuState.IsStopped;
                default:
                    return false;
            }
        }

        #endregion

    }
}
