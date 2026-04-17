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

        #region Public Methods 

        public override bool Accept(IVisitorCommand visitor)
        {
            return visitor.Visit(this);
        }

        public override bool CanSendCommand(IMasterCommand command)
        {
            if(this.ActivityStructure.TargetMcuState.IsStopped && base.CanSendCommand(command))
            {
                // TODO: We should check the parameter ID here, but for now we just check if the target is stopped, which is a requirement for all parameters.
                return true;
            }

            return false;
        }

        #endregion
    }
}
