using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    internal static class ResponseFactory
    {
        public static ISlaveResponse? CreateResponse(SlaveResponseEnum responseId)
        {
            switch (responseId)
            {
                // ResponseSize = 1
                case SlaveResponseEnum.RSP_OK:
                case SlaveResponseEnum.RSP_FAILED:
                case SlaveResponseEnum.RSP_ILLEGAL_PARAMETER:
                case SlaveResponseEnum.RSP_ILLEGAL_MEMORY_TYPE:
                case SlaveResponseEnum.RSP_ILLEGAL_MEMORY_RANGE:
                case SlaveResponseEnum.RSP_ILLEGAL_COMMAND:
                case SlaveResponseEnum.RSP_ILLEGAL_VALUE:
                case SlaveResponseEnum.RSP_ILLEGAL_BREAKPOINT:
                case SlaveResponseEnum.RSP_ILLEGAL_JTAG_ID:
                case SlaveResponseEnum.RSP_NO_TARGET_POWER:
                case SlaveResponseEnum.RSP_DEBUGWIRE_SYNC_FAILED:
                case SlaveResponseEnum.RSP_ILLEGAL_POWER_STATE:
                    return new Response(responseId);

                // Events with ResponseSize = 1
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
                    return new ResponseEvent(responseId);

                // ResponseSize > 1

                case SlaveResponseEnum.RSP_PARAMETER:
                case SlaveResponseEnum.RSP_MEMORY:
                case SlaveResponseEnum.RSP_SPI_DATA:
                    return new ResponseMultipleByte(responseId);

                case SlaveResponseEnum.RSP_SELFTEST:
                    return new ResponseSelfTest(responseId);

                case SlaveResponseEnum.RSP_GET_BREAK:
                    return new ResponseBreakpoint(responseId);

                case SlaveResponseEnum.RSP_ILLEGAL_EMULATOR_MODE:
                    return new ResponseEmulatorMode(responseId);

                case SlaveResponseEnum.RSP_ILLEGAL_MCU_STATE:
                    return new ResponseMcuState(responseId);

                case SlaveResponseEnum.RSP_PC:
                    return new ResponseProgranCounter(responseId);

                case SlaveResponseEnum.RSP_SIGN_ON:
                    return new ResponseSignOn(responseId);

                case SlaveResponseEnum.EVT_BREAK:
                    return new ResponseEventBreak(responseId);

                case SlaveResponseEnum.EVT_RUN:
                    return new ResponseEventRun(responseId);

                case SlaveResponseEnum.EVT_DEBUG:
                    return new ResponseEventDebug(responseId);

                default:
                    return null;
            }
        }
    }
}
