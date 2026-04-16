using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public sealed class TargetConnected : ActivityBaseComp
    {
        private TargetStopped _targetStopped;
        private TargetRunning _targetRunning;
        private TargetProgramming _targetProgramming;

        public TargetConnected(StructureActivity activityStructure) : this(activityStructure, null)
        {
        }

        public TargetConnected(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {
            _targetStopped = new TargetStopped(activityStructure, this);
            this.AddNext(_targetStopped);

            _targetRunning = new TargetRunning(activityStructure, this);
            this.AddNext(_targetRunning);

            _targetProgramming = new TargetProgramming(activityStructure, this);
            this.AddNext(_targetProgramming);

        }

        public override bool Accept(IVisitorActivity visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override bool ActivityAction()
        {
            return base.ActivityAction();
        }
        public override bool ActivityEntry()
        {

            this.NextActivity = this._targetStopped;

            return base.ActivityEntry();
        }

        public override bool ActivityExit(bool lastRequest)
        {
            NextActivity = this.Find<TargetDisonnecting>();
            return base.ActivityExit(lastRequest);
        }

        public override bool RequestCompleted(CommandRequestBase<IMasterCommand, ISlaveResponse> request)
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
                        case MasterCommandEnum.CMND_LEAVE_PROGMODE:
                            NextActivity = this.Find<TargetStopped>();
                            break;
                        case MasterCommandEnum.CMND_ENTER_PROGMODE:
                            NextActivity = this.Find<TargetProgramming>();
                            break;
                        case MasterCommandEnum.CMND_SINGLE_STEP:
                        case MasterCommandEnum.CMND_RUN_TO_ADDR:
                        case MasterCommandEnum.CMND_GO:
                            NextActivity = this.Find<TargetRunning>();
                            break;

                        case MasterCommandEnum.CMND_GET_SYNC:
                        case MasterCommandEnum.CMND_RESET:
                        case MasterCommandEnum.CMND_FORCED_STOP:
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

    }
}
