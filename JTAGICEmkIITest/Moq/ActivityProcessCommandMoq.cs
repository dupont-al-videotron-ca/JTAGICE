using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.HostService;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using log4net;


namespace JTAGICEmkIITest.Moq
{
    internal class ActivityProcessCommandMoq : ActivityProcessCommandBase
    {
        internal ActivityProcessCommandMoq(StructureActivity activityStructure) : 
            base(activityStructure, MasterCommandEnum.CMND_RESET, SlaveResponseEnum.RSP_OK)
        {
            IsCanSendCommand = true;
        }

        public override bool Accept(IVisitorCommand visitor)
        {
            return true;
        }

        public override bool Accept(IVisitorActivity visitor)
        {
            return true;
        }


        public bool IsCanSendCommand { get; set; }

        public override bool OnReceivedResponse(ISlaveResponse response)
        {
            switch (response.ResponseId)
            {
                case SlaveResponseEnum.RSP_OK:
                case SlaveResponseEnum.RSP_PARAMETER:
                case SlaveResponseEnum.RSP_MEMORY:
                case SlaveResponseEnum.RSP_GET_BREAK:
                case SlaveResponseEnum.RSP_PC:
                case SlaveResponseEnum.RSP_SELFTEST:
                case SlaveResponseEnum.RSP_SPI_DATA:
                case SlaveResponseEnum.RSP_SIGN_ON:
                case SlaveResponseEnum.EVT_TARGET_POWER_ON:
                case SlaveResponseEnum.EVT_TARGET_POWER_OFF:
                case SlaveResponseEnum.EVT_EXTERNAL_RESET:
                case SlaveResponseEnum.EVT_TARGET_SLEEP:
                case SlaveResponseEnum.EVT_TARGET_WAKEUP:
                case SlaveResponseEnum.EVT_ICE_POWER_ERROR_STATE:
                case SlaveResponseEnum.EVT_ICE_POWER_OK:
                case SlaveResponseEnum.EVT_IDR_DIRTY:
                case SlaveResponseEnum.EVT_PROGRAM_BREAK:
                case SlaveResponseEnum.EVT_PDSB_BREAK:
                case SlaveResponseEnum.EVT_PDSMB_BREAK:
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
                case SlaveResponseEnum.EVT_BREAK:
                case SlaveResponseEnum.EVT_RUN:
                case SlaveResponseEnum.EVT_DEBUG:
                    return true;
                default:
                    return base.OnReceivedResponse(response);
            }
        }

        public override bool CanSendCommand(IMasterCommand command)
        {
            return this.IsCanSendCommand && base.CanSendCommand(command);
        }

        public override bool CanSendResponse(ISlaveResponse response)
        {
            return this.IsCanSendCommand && base.CanSendResponse(response);
        }
         
        public override bool OnReceivedCommand(IMasterCommand command) => base.OnReceivedCommand(command);
    }
}
