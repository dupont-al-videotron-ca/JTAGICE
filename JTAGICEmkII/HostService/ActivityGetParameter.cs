using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivityGetParameter : ActivityProcessCommandBase
    {


        #region Constructors 
        public ActivityGetParameter(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_GET_PARAMETER, SlaveResponseEnum.RSP_PARAMETER)
        {
        }
        #endregion


        #region Fields 

        #endregion


        #region Properties 

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override bool Accept(IVisitorCommand visitor)
        {
            return visitor.Visit(this);
        }

        public override bool CanSendCommand(IMasterCommand command)
        {
            switch(command.MessageId)
            {
                case MasterCommandEnum.CMND_GET_PARAMETER:
                    // TODO: We should check the parameter ID here, but for now we just check if the target is stopped, which is a requirement for all parameters.
                    return this.ActivityStructure.TargetMcuState.IsStopped; 
                default:
                    return false;
            }
        }

        public override bool OnReceivedResponse(ISlaveResponse response)
        {
            switch (response.ResponseId)
            {
                case SlaveResponseEnum.RSP_PARAMETER:
                    return true;
                default:
                    return base.OnReceivedResponse(response);
            }
        }

        #endregion
    }
}
