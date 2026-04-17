using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;


namespace JTAGICEmkII.HostService
{
    public sealed class ActivitySetDeviceDescriptor : ActivityProcessCommandBase
    {

        public ActivitySetDeviceDescriptor(StructureActivity activityStructure) :
            base(activityStructure, MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR, SlaveResponseEnum.RSP_OK)
        {
        }

        public override bool Accept(IVisitorCommand visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool CanSendCommand(IMasterCommand command)
        {
            return this.ActivityStructure.TargetMcuState.IsStopped && base.CanSendCommand(command);
        }
    }
}
