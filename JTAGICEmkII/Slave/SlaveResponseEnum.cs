using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Slave
{
    public enum SlaveResponseEnum
    {
        RSP_OK = 0x80,
        RSP_FAILED = 0xA0,
        RSP_ILLEGAL_PARAMETER = 0xA1,
        RSP_PARAMETER = 0x81,
        RSP_ILLEGAL_MEMORY_TYPE = 0xA2,
        RSP_ILLEGAL_MEMORY_RANGE = 0xA3,
        RSP_MEMORY = 0x82,
        RSP_GET_BREAK = 0x83,
        RSP_ILLEGAL_EMULATOR_MODE = 0xA4,
        RSP_ILLEGAL_MCU_STATE = 0xA5,
        RSP_PC = 0x84,
        RSP_SELFTEST = 0x85,
        RSP_SPI_DATA = 0x88,
        RSP_ILLEGAL_COMMAND = 0xAA,
        RSP_ILLEGAL_VALUE = 0xA6,
        RSP_SIGN_ON = 0x86,
        RSP_ILLEGAL_BREAKPOINT = 0xA8,
        RSP_ILLEGAL_JTAG_ID = 0xA9,
        RSP_NO_TARGET_POWER = 0xAB,
        RSP_DEBUGWIRE_SYNC_FAILED = 0xAC,
        RSP_ILLEGAL_POWER_STATE = 0xAD,

        // Events
        EventRangeMin = 0xE0,
        EVT_BREAK = EventRangeMin,
        EVT_RUN = 0xE1,
        EVT_TARGET_POWER_ON = 0xE4,
        EVT_TARGET_POWER_OFF = 0xE5,
        EVT_DEBUG = 0xE6,
        EVT_EXTERNAL_RESET = 0xE7,
        EVT_TARGET_SLEEP = 0xE8,
        EVT_TARGET_WAKEUP = 0xE9,
        EVT_ICE_POWER_ERROR_STATE = 0xEA,
        EVT_ICE_POWER_OK = 0xEB,
        EVT_IDR_DIRTY = 0xEC,
        EVT_NONE = 0xEF, //dummy event used to indicate no events in the queue
        EVT_PROGRAM_BREAK = 0xF1,
        EVT_PDSB_BREAK = 0xF2,
        EVT_PDSMB_BREAK = 0xF3,
        EVT_ERROR_PHY_FROECE_BREAK_TIMEOUT = 0xE2,
        EVT_ERROR_PHY_RELEASE_BREAK_TIMEOUT = 0xE3,
        EVT_ERROR_PHY_MAX_BIT_LENGHT_DIFF = 0xED,
        EVT_ERROR_PHY_SYNC_TIMEOUT = 0xF0,
        EVT_ERROR_PHY_SYNC_TIMEOUT_BAUD = 0xF4,
        EVT_ERROR_PHY_SYNC_OUT_OF_RANGE = 0xF5,
        EVT_ERROR_PHY_SYNC_WAIT_TIMEOUT = 0xF6,
        EVT_ERROR_PHY_RECEIVE_TIMEOUT = 0xF7,
        EVT_ERROR_PHY_RECEIVE_BREAK = 0xF8,
        EVT_ERROR_PHY_OPT_RECEIVE_TIMEOUT = 0xF9,
        EVT_ERROR_PHY_OPT_RECEIVED_BREAK = 0xFA,
        EVT_ERROR_PHY_NO_ACTIVITY = 0xFB,
        EventRangeMax = 0xFF,

    }
}
