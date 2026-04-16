using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using Windows.Web.Http.Diagnostics;

namespace JTAGICEmkII.HostService
{
    public abstract class ActivityProcessCommandBase : ActivityBase, IActivityComElement
    {
        internal ActivityProcessCommandBase(StructureActivity activityStructure,
            MasterCommandEnum commandEnum,
            SlaveResponseEnum responseEnum) : base(activityStructure)
        {
            this.CommandEnum = commandEnum;
            this.ResponseEnum = responseEnum;
            this.LastError = SlaveResponseEnum.RSP_OK;
        }

        public bool HasError { get; protected set; }

        public SlaveResponseEnum LastError { get; protected set; }

        public MasterCommandEnum CommandEnum { get; }
        public SlaveResponseEnum ResponseEnum { get; }

        public abstract bool Accept(IVisitorCommand visitor);

        public override bool Accept(IVisitorActivity visitor) 
            => throw new NotImplementedException();
        public override bool ActivityAction() => throw new NotImplementedException();
        public override bool ActivityEntry() => throw new NotImplementedException();
        public override bool ActivityExit(bool lastRequest) => throw new NotImplementedException();

        public virtual bool CanSendCommand(IMasterCommand command)
        {
            Logger.Debug($"{this.GetType()} Executing can send command called.");
            // all target states should be able to send the command, so we only check the command type here.
            return command.MessageId == this.CommandEnum;
        }

        public virtual bool CanSendResponse(ISlaveResponse response)
        {
            Logger.Debug($"{this.GetType()} Executing can send response called.");
            // all target states should be able to send the reponse, so we only check the command type here.
            return response.ResponseId == this.ResponseEnum;
        }

        public virtual bool CommandSent()
        {
            Logger.Debug($"{this.GetType()} Executing command sent called.");
            return true;
        }

        public virtual bool OnReceivedCommand(IMasterCommand command)
        {
            switch (command.MessageId)
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
                case MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR:
                case MasterCommandEnum.CMND_SELFTEST:
                case MasterCommandEnum.CMND_SPI_CMD:
                case MasterCommandEnum.CMND_SET_PARAMETER:
                case MasterCommandEnum.CMND_GET_PARAMETER:
                case MasterCommandEnum.CMND_WRITE_MEMORY:
                case MasterCommandEnum.CMND_READ_MEMORY:
                case MasterCommandEnum.CMND_WRITE_PC:
                case MasterCommandEnum.CMND_RUN_TO_ADDR:
                case MasterCommandEnum.CMND_SINGLE_STEP:
                case MasterCommandEnum.CMND_FORCED_STOP:
                case MasterCommandEnum.CMND_RESET:
                case MasterCommandEnum.CMND_ERASEPAGE_SPM:
                case MasterCommandEnum.CMND_GET_BREAK:
                case MasterCommandEnum.CMND_SET_BREAK:
                case MasterCommandEnum.CMND_CLR_BREAK:
                case MasterCommandEnum.CMND_SET_N_PARAMETERS:
                    return true;
                default:
                    return false;
            }
        }

        public virtual bool OnReceivedResponse(ISlaveResponse response)
        {
            switch (response.ResponseId)
            {
                case SlaveResponseEnum.RSP_OK:
                    this.HasError = false;
                    this.LastError = SlaveResponseEnum.RSP_OK;
                    this.Logger.Debug($"Received response: {SlaveResponseEnum.RSP_OK}.");
                    return true;

                case SlaveResponseEnum.RSP_FAILED:
                    OnFailed(response);
                    return true;
                case SlaveResponseEnum.RSP_ILLEGAL_MEMORY_TYPE:
                    OnIllegalMemoryType(response);
                    return true;
                case SlaveResponseEnum.RSP_ILLEGAL_PARAMETER:
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
                    this.HasError = true;
                    this.LastError = response.ResponseId;
                    this.Logger.Debug($"Received error response: {response.ResponseId}.");
                    return true;
                default:
                    this.Logger.Debug($"Response handling not implemented for response: {response.ResponseId}.");
                    throw new NotImplementedException($"Response handling not implemented for response: {response.ResponseId}.");
            }
        }

        protected virtual void OnFailed(ISlaveResponse response)
        {
        }

        protected virtual void OnIllegalMemoryType(ISlaveResponse response)
        {
        }
        protected virtual void OnIllegalMemoryRange(ISlaveResponse response)
        {
        }
    }
}
