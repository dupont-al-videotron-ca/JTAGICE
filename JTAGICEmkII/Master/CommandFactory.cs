using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal static class CommandFactory
    {
        public static IMasterCommand CreateCommand(MasterCommandEnum messageId)
        {
            switch (messageId)
            {
                // Single byte commands
                case MasterCommandEnum.CMND_SIGN_OFF:
                case MasterCommandEnum.CMND_GET_SIGN_ON:
                case MasterCommandEnum.CMND_READ_PC:
                case MasterCommandEnum.CMND_GO:
                case MasterCommandEnum.CMND_GET_SYNC:
                case MasterCommandEnum.CMND_CHIP_ERASE:
                case MasterCommandEnum.CMND_ENTER_PROGMODE:
                case MasterCommandEnum.CMND_LEAVE_PROGMODE:
                case MasterCommandEnum.CMND_CLEAR_EVENTS:
                case MasterCommandEnum.CMND_RESTORE_TARGET:
                    return new Command(messageId);

                // multiple byte commands
                case MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR:
                case MasterCommandEnum.CMND_SELFTEST:
                case MasterCommandEnum.CMND_SPI_CMD:
                    return new CommandMultipleByte(messageId);

                case MasterCommandEnum.CMND_SET_PARAMETER:
                case MasterCommandEnum.CMND_GET_PARAMETER:
                    return new CommandParameter(messageId);

                case MasterCommandEnum.CMND_WRITE_MEMORY:
                case MasterCommandEnum.CMND_READ_MEMORY:
                    return new CommandMemory(messageId);

                case MasterCommandEnum.CMND_WRITE_PC:
                case MasterCommandEnum.CMND_RUN_TO_ADDR:
                    return new CommandProgramCounter(messageId);

                case MasterCommandEnum.CMND_SINGLE_STEP:
                    return new CommandSingleStep(messageId);

                case MasterCommandEnum.CMND_FORCED_STOP:
                case MasterCommandEnum.CMND_RESET:
                    return new CommandPCMode(messageId);

                case MasterCommandEnum.CMND_ERASEPAGE_SPM:
                    return new CommandAddress(messageId);

                case MasterCommandEnum.CMND_GET_BREAK:
                    return new CommandBreakNumber(messageId);

                case MasterCommandEnum.CMND_SET_BREAK:
                    return new CommandBreakpoint(messageId);

                case MasterCommandEnum.CMND_CLR_BREAK:
                    return new CommandBreakAddress(messageId);

                case MasterCommandEnum.CMND_SET_N_PARAMETERS:
                    return new CommandNParameter(messageId);

                default:
                    return null!;
            }
        }
    }
}
