using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivitySetParameter : ActivityProcessCommand
    {


        #region Constructors 
        public ActivitySetParameter(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_SET_PARAMETER, SlaveResponseEnum.RSP_OK)
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
                case MasterCommandEnum.CMND_GET_PARAMETER:
                    return this.ActivityStructure.TargetMcuState.IsStopped; 
                default:
                    return false;
            }
        }

        #endregion
    }
}
