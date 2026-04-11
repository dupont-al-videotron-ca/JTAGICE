using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    internal abstract class ActivityProcessCommand : ActivityComposite, IActivityComElement
    {
        internal ActivityProcessCommand(StructureActivity activityStructure) : base(activityStructure)
        {
        }

        internal ActivityProcessCommand(StructureActivity activityStructure, IActivityElement parent) : base(activityStructure, parent)
        {
        }

        public abstract bool Accept(IVisitorCommand visitor);
        public override bool Accept(IVisitorActivity visitor) => throw new NotImplementedException();

        public abstract bool CanSendCommand(IMasterCommand command);

        public virtual bool CommandSent()
        {
            Logger.Debug($"{this.GetType()} Executing command sent called.");
            return true;
        }

        public virtual bool OnReceivedResponse(ISlaveResponse response)
        {
            switch (response.ResponseId)
            {
                case SlaveResponseEnum.RSP_FAILED:
                case SlaveResponseEnum.RSP_ILLEGAL_PARAMETER:
                case SlaveResponseEnum.RSP_ILLEGAL_MEMORY_TYPE:
                case SlaveResponseEnum.RSP_ILLEGAL_MEMORY_RANGE:
                case SlaveResponseEnum.RSP_ILLEGAL_EMULATOR_MODE:
                case SlaveResponseEnum.RSP_ILLEGAL_MCU_STATE:
                case SlaveResponseEnum.RSP_ILLEGAL_COMMAND:
                case SlaveResponseEnum.RSP_ILLEGAL_VALUE:
                case SlaveResponseEnum.RSP_ILLEGAL_BREAKPOINT:
                case SlaveResponseEnum.RSP_ILLEGAL_JTAG_ID:
                case SlaveResponseEnum.RSP_NO_TARGET_POWER:
                case SlaveResponseEnum.RSP_DEBUGWIRE_SYNC_FAILED:
                case SlaveResponseEnum.RSP_ILLEGAL_POWER_STATE:
                    this.Logger.Error($"Received error response: {response.ResponseId}.");
                    return false;
                default:
                    throw new NotImplementedException($"Response handling not implemented for response: {response.ResponseId}.");
            }
        }
    }
}
