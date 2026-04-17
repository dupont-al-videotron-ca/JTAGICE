using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivityGetBreak : ActivityProcessCommandBase
    {


        #region Constructors 
        public ActivityGetBreak(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_GET_BREAK, SlaveResponseEnum.RSP_GET_BREAK)
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
            return this.ActivityStructure.TargetMcuState.IsStopped && base.CanSendCommand(command);
        }

        #endregion

    }
}
