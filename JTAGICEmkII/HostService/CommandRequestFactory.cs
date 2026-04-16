using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JTAGICEmkII;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using MyFramework;
using MyFramework.Threading;

namespace JTAGICEmkII.HostService
{
    internal static class CommandRequestFactory 
    {
        public static CommandRequest<IMasterCommand, ISlaveResponse> CreateRequest(StructureActivity structureElement, MasterCommandEnum messageId)
        {
            return CreateRequest(structureElement, messageId, TimeSpan.FromSeconds(45));

        }
        public static CommandRequest<IMasterCommand, ISlaveResponse> CreateRequest(StructureActivity structureElement, MasterCommandEnum messageId, TimeSpan timeout)
        {
            var command = CommandFactory.CreateCommand(messageId);
            switch (messageId)
            {
                // Single byte commands
                case MasterCommandEnum.CMND_SIGN_OFF:
                    return new CommandRequest<IMasterCommand, ISlaveResponse>(command, new ActivitySignOff(structureElement), timeout);
                case MasterCommandEnum.CMND_GET_SIGN_ON:
                    return new CommandRequest<IMasterCommand, ISlaveResponse>(command, new ActivitySignOn(structureElement), timeout);
                case MasterCommandEnum.CMND_CLEAR_EVENTS:
                    return new CommandRequest<IMasterCommand, ISlaveResponse>(command, new ActivityClearEvents(structureElement), timeout);
                case MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR:
                    return new CommandRequest<IMasterCommand, ISlaveResponse>(command, new ActivitySetDeviceDescriptor(structureElement), timeout);
                case MasterCommandEnum.CMND_RESET:
                    return new CommandRequest<IMasterCommand, ISlaveResponse>(command, new ActivityReset(structureElement), timeout);
                case MasterCommandEnum.CMND_SET_PARAMETER:
                    return new CommandRequest<IMasterCommand, ISlaveResponse>(command, new ActivitySetParameter(structureElement), timeout);
                case MasterCommandEnum.CMND_GET_PARAMETER:
                    return new CommandRequest<IMasterCommand, ISlaveResponse>(command, new ActivityGetParameter(structureElement), timeout);

                case MasterCommandEnum.CMND_READ_PC:
                case MasterCommandEnum.CMND_GO:
                case MasterCommandEnum.CMND_GET_SYNC:
                case MasterCommandEnum.CMND_CHIP_ERASE:
                case MasterCommandEnum.CMND_ENTER_PROGMODE:
                case MasterCommandEnum.CMND_LEAVE_PROGMODE:
                case MasterCommandEnum.CMND_RESTORE_TARGET:
                case MasterCommandEnum.CMND_SELFTEST:
                case MasterCommandEnum.CMND_SPI_CMD:
                case MasterCommandEnum.CMND_WRITE_MEMORY:
                case MasterCommandEnum.CMND_READ_MEMORY:
                case MasterCommandEnum.CMND_WRITE_PC:
                case MasterCommandEnum.CMND_RUN_TO_ADDR:
                case MasterCommandEnum.CMND_SINGLE_STEP:
                case MasterCommandEnum.CMND_FORCED_STOP:
                case MasterCommandEnum.CMND_ERASEPAGE_SPM:
                case MasterCommandEnum.CMND_GET_BREAK:
                case MasterCommandEnum.CMND_SET_BREAK:
                case MasterCommandEnum.CMND_CLR_BREAK:
                case MasterCommandEnum.CMND_SET_N_PARAMETERS:
                default:
                    throw new NotImplementedException($"Command {messageId} is not implemented in the CommandRequestFactory.");
            }
        }


    }
}
