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

        public override bool ActivityEntry()
        {
            Logger.Debug($"{this.GetType()} Executing activity entry called.");
            if (!this.ActivityStructure.HostService.SetDeviceDescriptor())
                return false;

            return true;
        }

        public override bool CanSendCommand(IMasterCommand command)
        {
            switch (command.MessageId)
            {
                case MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR:
                    if (this.ActivityStructure.TargetMcuState.IsStopped)
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }
    }
}
