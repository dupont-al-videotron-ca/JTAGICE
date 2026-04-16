using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class TargetRunning : ActivityBaseComp
    {
        public TargetRunning(StructureActivity activityStructure) : this(activityStructure, null!)
        {
        }

        public TargetRunning(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {
        }

        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool ActivityAction()
        {
            // TODO: This is a temporary implementation to allow the activity structure to be executed. The actual implementation will be added later.
            return base.ActivityAction();
        }

        public override bool ActivityEntry()
        {
            this.ActivityStructure.TargetMcuState.GoRunning();
            return base.ActivityEntry();
        }

        public override bool ActivityExit(bool lastRequest)
        {

            return base.ActivityExit(lastRequest);
        }

        public override bool RequestCompleted(CommandRequest<IMasterCommand, ISlaveResponse> request)
        {
            var response = request.Response;
            var messageId = request.Command.MessageId;

            switch (response?.ResponseId)
            {
                case SlaveResponseEnum.RSP_OK:
                    switch (messageId)
                    {
                        case MasterCommandEnum.CMND_RESTORE_TARGET:
                            NextActivity = this.Find<TargetDisonnecting>();
                            break;
                        case MasterCommandEnum.CMND_GET_SYNC:
                        case MasterCommandEnum.CMND_RESET:
                        case MasterCommandEnum.CMND_FORCED_STOP:
                            NextActivity = this.Find<TargetStopped>();
                            break;
                        case MasterCommandEnum.CMND_SINGLE_STEP:
                        case MasterCommandEnum.CMND_RUN_TO_ADDR:
                        case MasterCommandEnum.CMND_GO:
                            NextActivity = this.Find<TargetRunning>();
                            break;
                        case MasterCommandEnum.CMND_ENTER_PROGMODE:
                        case MasterCommandEnum.CMND_CHIP_ERASE:
                        case MasterCommandEnum.CMND_SELFTEST:
                        case MasterCommandEnum.CMND_GET_BREAK:
                        case MasterCommandEnum.CMND_SET_BREAK:
                        case MasterCommandEnum.CMND_ERASEPAGE_SPM:
                        case MasterCommandEnum.CMND_SPI_CMD:
                        case MasterCommandEnum.CMND_CLR_BREAK:
                        case MasterCommandEnum.CMND_WRITE_MEMORY:
                        case MasterCommandEnum.CMND_READ_MEMORY:
                        case MasterCommandEnum.CMND_READ_PC:
                        case MasterCommandEnum.CMND_WRITE_PC:
                        case MasterCommandEnum.CMND_CLEAR_EVENTS:
                        case MasterCommandEnum.CMND_SIGN_OFF:
                        case MasterCommandEnum.CMND_GET_SIGN_ON:
                        case MasterCommandEnum.CMND_LEAVE_PROGMODE:
                        case MasterCommandEnum.CMND_SET_DEVICE_DESCRIPTOR:
                        case MasterCommandEnum.CMND_SET_PARAMETER:
                        case MasterCommandEnum.CMND_GET_PARAMETER:
                        case MasterCommandEnum.CMND_SET_N_PARAMETERS:
                        default:
                            break;
                    }
                    break;

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
                    break;
                default:
                    this.Logger.Debug($"Response handling not implemented for response: {response?.ResponseId}.");
                    throw new NotImplementedException($"Response handling not implemented for response: {response?.ResponseId}.");
            }

            return true;
        }

        public override bool RequestTimeout(CommandRequest<IMasterCommand, ISlaveResponse> request)
        {
            this.Logger.Debug("Host service request timed out.");
            NextActivity = this.Find<TargetRunning>();
            return true;
        }

        public override bool EventReceived(ISlaveResponse response)
        {
            switch (response?.ResponseId)
            {
                case SlaveResponseEnum.EVT_PROGRAM_BREAK:
                case SlaveResponseEnum.EVT_BREAK:
                case SlaveResponseEnum.EVT_PDSB_BREAK:
                case SlaveResponseEnum.EVT_PDSMB_BREAK:
                    NextActivity = this.Find<TargetStopped>();
                    break;
                case SlaveResponseEnum.EVT_RUN:
                    NextActivity = this.Find<TargetRunning>();
                    break;
                case SlaveResponseEnum.EVT_TARGET_POWER_ON:
                    //_nextIndex = this.Nexts.FindIndex(n => n is );
                    break;

                case SlaveResponseEnum.EVT_DEBUG:
                    NextActivity = this.Find<TargetStopped>();
                    break;
                case SlaveResponseEnum.EVT_EXTERNAL_RESET:
                case SlaveResponseEnum.EVT_TARGET_SLEEP:
                case SlaveResponseEnum.EVT_TARGET_WAKEUP:
                    NextActivity = this.Find<TargetRunning>();
                    break;
                case SlaveResponseEnum.EVT_ICE_POWER_ERROR_STATE:
                case SlaveResponseEnum.EVT_ICE_POWER_OK:
                case SlaveResponseEnum.EVT_IDR_DIRTY:
                case SlaveResponseEnum.EVT_NONE:
                case SlaveResponseEnum.EVT_ERROR_PHY_FROECE_BREAK_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_RELEASE_BREAK_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_MAX_BIT_LENGHT_DIFF:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT_BAUD:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_OUT_OF_RANGE:
                case SlaveResponseEnum.EVT_ERROR_PHY_SYNC_WAIT_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_BREAK:
                case SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVE_TIMEOUT:
                case SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVED_BREAK:
                case SlaveResponseEnum.EVT_ERROR_PHY_NO_ACTIVITY:

                    break;
                default:
                    this.Logger.Debug($"Event handling not implemented for event: {response?.ResponseId}.");
                    throw new NotImplementedException($"Event handling not implemented for event: {response?.ResponseId}.");
            }

            return true;
        }
    }
}
