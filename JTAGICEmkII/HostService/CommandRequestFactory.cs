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
        public static CommandRequest CreateRequest(StructureActivity structureElement, MasterCommandEnum messageId)
        {
            return CreateRequest(structureElement, messageId, TimeSpan.FromSeconds(45));

        }
        public static CommandRequest CreateRequest(StructureActivity structureElement, MasterCommandEnum messageId, TimeSpan timeout)
        {
            var command = CommandFactory.CreateCommand(messageId);
            switch (messageId)
            {
                // Single byte commands
                case MasterCommandEnum.CMND_SIGN_OFF:
                    return new CommandRequest(command, new ActivitySignOff(structureElement), timeout);
                case MasterCommandEnum.CMND_GET_SIGN_ON:
                    return new CommandRequest(command, new ActivitySignOn(structureElement), timeout);
                case MasterCommandEnum.CMND_CLEAR_EVENTS:
                    return new CommandRequest(command, new ActivityClearEvents(structureElement), timeout);
                case MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR:
                    return new CommandRequest(command, new ActivitySetDeviceDescriptor(structureElement), timeout);
                case MasterCommandEnum.CMND_RESET:
                    return new CommandRequest(command, new ActivityReset(structureElement), timeout);
                case MasterCommandEnum.CMND_SET_PARAMETER:
                    return new CommandRequest(command, new ActivitySetParameter(structureElement), timeout);
                case MasterCommandEnum.CMND_GET_PARAMETER:
                    return new CommandRequest(command, new ActivityGetParameter(structureElement), timeout);
                case MasterCommandEnum.CMND_GO:
                    return new CommandRequest(command, new ActivityGo(structureElement), timeout);
                case MasterCommandEnum.CMND_GET_SYNC:
                    return new CommandRequest(command, new ActivityGetSync(structureElement), timeout);
                case MasterCommandEnum.CMND_CHIP_ERASE:
                    return new CommandRequest(command, new ActivityChipErase(structureElement), timeout);
                case MasterCommandEnum.CMND_ENTER_PROGMODE:
                    return new CommandRequest(command, new ActivityEnterProgMode(structureElement), timeout);
                case MasterCommandEnum.CMND_LEAVE_PROGMODE:
                    return new CommandRequest(command, new ActivityLeaveProgMode(structureElement), timeout);
                case MasterCommandEnum.CMND_RESTORE_TARGET:
                    return new CommandRequest(command, new ActivityRestoreTarget(structureElement), timeout);
                case MasterCommandEnum.CMND_SELFTEST:
                    return new CommandRequest(command, new ActivitySelfTest(structureElement), timeout);
                case MasterCommandEnum.CMND_SPI_CMD:
                    return new CommandRequest(command, new ActivitySPICmd(structureElement), timeout);
                case MasterCommandEnum.CMND_WRITE_MEMORY:
                    return new CommandRequest(command, new ActivityWriteMemory(structureElement), timeout);
                case MasterCommandEnum.CMND_READ_MEMORY:
                    return new CommandRequest(command, new ActivityReadMemory(structureElement), timeout);
                case MasterCommandEnum.CMND_WRITE_PC:
                    return new CommandRequest(command, new ActivityWritePC(structureElement), timeout);
                case MasterCommandEnum.CMND_READ_PC:
                    return new CommandRequest(command, new ActivityReadPC(structureElement), timeout);
                case MasterCommandEnum.CMND_SINGLE_STEP:
                    return new CommandRequest(command, new ActivitySingleStep(structureElement), timeout);
                case MasterCommandEnum.CMND_FORCED_STOP:
                    return new CommandRequest(command, new ActivityForceStop(structureElement), timeout);
                case MasterCommandEnum.CMND_ERASEPAGE_SPM:
                    return new CommandRequest(command, new ActivityErasePageSpm(structureElement), timeout);
                case MasterCommandEnum.CMND_GET_BREAK:
                    return new CommandRequest(command, new ActivityGetBreak(structureElement), timeout);
                case MasterCommandEnum.CMND_SET_BREAK:
                    return new CommandRequest(command, new ActivitySetBreak(structureElement), timeout);
                case MasterCommandEnum.CMND_CLR_BREAK:
                    return new CommandRequest(command, new ActivityClearBreak(structureElement), timeout);
                case MasterCommandEnum.CMND_RUN_TO_ADDR:
                    return new CommandRequest(command, new ActivityRunToAddr(structureElement), timeout);
                case MasterCommandEnum.CMND_SET_N_PARAMETERS:
                default:
                    throw new NotImplementedException($"Command {messageId} is not implemented in the CommandRequestFactory.");
            }
        }


    }
}
