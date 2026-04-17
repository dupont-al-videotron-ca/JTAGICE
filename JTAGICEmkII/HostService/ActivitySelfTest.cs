using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class ActivitySelfTest : ActivityProcessCommandBase
    {


        #region Constructors 
        public ActivitySelfTest(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_SELFTEST, SlaveResponseEnum.RSP_SELFTEST)
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
