using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    public enum MasterCommandEnum
    {
        CMND_SIGN_OFF = 0x00,
        CMND_GET_SIGN_ON = 0x01,
        CMND_SET_PARAMETER = 0x02,
        CMND_GET_PARAMETER = 0x03,
        CMND_WRITE_MEMORY = 0x04,
        CMND_READ_MEMORY = 0x05,
        CMND_WRITE_PC = 0x06,
        CMND_READ_PC = 0x07,
        CMND_GO = 0x08,
        CMND_SINGLE_STEP = 0x09,
        CMND_FORCED_STOP = 0x0A,
        CMND_RESET = 0x0B,
        CMND_SET_DEVICE_DESCRIPTOR = 0x0C,
        CMND_ERASEPAGE_SPM = 0x0D,
        CMND_GET_SYNC = 0x0F,
        CMND_SELFTEST = 0x10,
        CMND_SET_BREAK = 0x11,
        CMND_GET_BREAK = 0x12,
        CMND_CHIP_ERASE = 0x13,
        CMND_ENTER_PROGMODE = 0x14,
        CMND_LEAVE_PROGMODE = 0x15,
        CMND_SET_N_PARAMETERS = 0x16,
        CMND_CLR_BREAK = 0x1A,
        CMND_RUN_TO_ADDR = 0x1C,
        CMND_SPI_CMD = 0x1D,
        CMND_CLEAR_EVENTS = 0x22,
        CMND_RESTORE_TARGET = 0x23,
    }
}
