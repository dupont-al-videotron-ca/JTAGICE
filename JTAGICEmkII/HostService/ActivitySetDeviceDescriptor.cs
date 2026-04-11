using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;


namespace JTAGICEmkII.HostService
{
    internal sealed class ActivitySetDeviceDescriptor : ActivityProcessCommand
    {

        public ActivitySetDeviceDescriptor(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent!)
        {
        }

        public override bool Accept(IVisitorCommand visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool CanSendCommand(IMasterCommand command)
        {
            switch (command.MessageId)
            {
                case MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR:
                    if(this.ActivityStructure.TargetMcuState.IsStopped)
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }
        public override bool OnReceivedResponse(ISlaveResponse response)
        {
            _nextIndex = -1;
            switch (response.ResponseId)
            {
                case SlaveResponseEnum.RSP_OK:
                    if (this.HasParent)
                    {
                        if (this.Parent is ActivityReset)
                        {
                            _nextIndex = 0;
                            return true;
                        }
                        else
                        { 
                            // todo:
                            return false;
                        }
                    }
                    return false;
                default:
                    return base.OnReceivedResponse(response);
            }
        }
    }
}
