using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public class CommandResult : ICommandResult
    {
        public CommandResult()
        {
        }

        public CommandResult(ISlaveResponse response)
        {
            ArgumentNullException.ThrowIfNull(response);
            this.ErrorCode = (int)response.ResponseId!;
            this.response = new Response(response);
        }

        private CommandResult(SlaveResponseEnum response)
        {
            this.ErrorCode = (int)response;
        }

        private readonly static Dictionary<int, ResultMetadata> ResultMetadatas;
        public static readonly CommandResult Successs = new (SlaveResponseEnum.RSP_OK);
        public static readonly CommandResult Failed = new (SlaveResponseEnum.RSP_FAILED);


        private readonly ISlaveResponse? response;

        public static explicit operator CommandResult(bool result)
        {
            if (result)
                return CommandResult.Successs;
            else
                return CommandResult.Failed;
        }

        public static explicit operator CommandResult(SlaveResponseEnum code)
        {
            if (code == SlaveResponseEnum.RSP_OK)
                return CommandResult.Successs;
            else
                return new CommandResult(code);
        }

        public static explicit operator CommandResult(Response code)
        {
            return new CommandResult(code);
        }

        public static implicit operator bool(CommandResult result)
        {
            return result.IsSuccess;
        }

        static CommandResult()
        {
            ResultMetadatas = new Dictionary<int, ResultMetadata>();

            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_OK, (new ResultMetadata(SlaveResponseEnum.RSP_OK, "The command was executed.")));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_FAILED, new ResultMetadata(SlaveResponseEnum.RSP_FAILED, $"The command was not understood by the {HostDeviceService.HostDeviceName}."));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_ILLEGAL_PARAMETER, new ResultMetadata(SlaveResponseEnum.RSP_ILLEGAL_PARAMETER, $"The {HostDeviceService.HostDeviceName} does not support the selected parameter."));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_ILLEGAL_MEMORY_RANGE, new ResultMetadata(SlaveResponseEnum.RSP_ILLEGAL_MEMORY_RANGE, $"The memory write was outside the bounds of the selected memory area."));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_ILLEGAL_EMULATOR_MODE, new ResultMetadata(SlaveResponseEnum.RSP_ILLEGAL_EMULATOR_MODE, $"The operation cannot be performed in this emulator mode."));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_ILLEGAL_MCU_STATE, new ResultMetadata(SlaveResponseEnum.RSP_ILLEGAL_MCU_STATE, $"The operation cannot be performed with the target MCU in its current state."));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_ILLEGAL_COMMAND, new ResultMetadata(SlaveResponseEnum.RSP_ILLEGAL_COMMAND, $"The master has tried to access an illegal emulator command."));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_ILLEGAL_VALUE, new ResultMetadata(SlaveResponseEnum.RSP_ILLEGAL_VALUE, $"The master has tried to write an illegal value to an emulator parameter."));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_ILLEGAL_BREAKPOINT, new ResultMetadata(SlaveResponseEnum.RSP_ILLEGAL_BREAKPOINT, $"The master has attempted to set or get a breakpoint, which does not exist."));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_ILLEGAL_JTAG_ID, new ResultMetadata(SlaveResponseEnum.RSP_ILLEGAL_JTAG_ID, $"The master has attempted to enter programming mode but the JTAG ID does not match the target device."));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_NO_TARGET_POWER, new ResultMetadata(SlaveResponseEnum.RSP_NO_TARGET_POWER, $"The master has attempted to execute a command but the target device is switched off or disconnected"));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_DEBUGWIRE_SYNC_FAILED, new ResultMetadata(SlaveResponseEnum.RSP_DEBUGWIRE_SYNC_FAILED, $"The master has attempted to enter DebugWire mode, but the target did not respond to a reset pulse."));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_ILLEGAL_POWER_STATE, new ResultMetadata(SlaveResponseEnum.RSP_ILLEGAL_POWER_STATE, $"This response is sent to any command when the ICE is running off USB power but\r\nhas only enumerated for 100mA operation. The ICE is in power save mode and will\r\nnot respond normally to any commands."));
            ResultMetadatas.Add((int)SlaveResponseEnum.RSP_ILLEGAL_MEMORY_TYPE, new ResultMetadata(SlaveResponseEnum.RSP_ILLEGAL_MEMORY_TYPE, $"The master has tried to write or read emulator memory type that does not exist."));

        }
        public int ErrorCode { get; private set; }

        public string ErrorDesctiption
        {
            get
            {
                if (ResultMetadatas.TryGetValue(ErrorCode, out ResultMetadata? resultMetadata))
                {
                    return resultMetadata.ErrorDesctiption;
                }
                else
                    return string.Format($"There is no description available for this Error {ErrorCode}.");
            }
        }

        public bool IsSuccess => ErrorCode == (int)SlaveResponseEnum.RSP_OK;

        private class ResultMetadata
        {
            public ResultMetadata(int errorCode, string errorDesctiption)
            {
                ErrorCode = errorCode;
                ErrorDesctiption = errorDesctiption;
            }

            public ResultMetadata(SlaveResponseEnum errorCode, string errorDesctiption)
            {
                ErrorCode = (int)errorCode;
                ErrorDesctiption = errorDesctiption;
            }

            public int ErrorCode { get; }

            public string ErrorDesctiption { get; }
        }

    }
}
