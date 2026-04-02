using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.Master
{
    internal enum MemoryTypeEnum : byte
    {
        MT_IO_SHADOW = 0x30,
        MT_SRAM = 0x20,
        MT_EEPROM = 0x22,
        MT_EVENT = 0x60,
        MT_SPM = 0xA0,
        MT_FLASH_PAGE = 0xB0,
        MT_EEPROM_PAGE = 0xB1,
        MT_FUSE_BITS = 0xB2,
        MT_LOCK_BITS = 0xB3,
        MT_SIGN_JTAG = 0xB4,
        MT_OSCAL_BYTE = 0xB5,
        MT_CAN = 0xB6,
    }
}
