using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DeviceDescriptorFields
    {
        public fixed byte ucReadIO[8]; //LSB = IOloc 0, MSB = IOloc63
        public fixed byte ucReadIOShadow[8]; //LSB = IOloc 0, MSB = IOloc63
        public fixed byte ucWriteIO[8]; //LSB = IOloc 0, MSB = IOloc63
        public fixed byte ucWriteIOShadow[8]; //LSB = IOloc 0, MSB = IOloc63
        public fixed byte ucReadExtIO[52]; //LSB = IOloc 96, MSB = IOloc511
        public fixed byte ucReadIOExtShadow[52]; //LSB = IOloc 96, MSB = IOloc511
        public fixed byte ucWriteExtIO[52]; //LSB = IOloc 96, MSB = IOloc511
        public fixed byte ucWriteIOExtShadow[52];//LSB = IOloc 96, MSB = IOloc511
        public byte ucIDRAddress; //IDR address
        public byte ucSPMCRAddress; //SPMCR Register address and dW BasePC
        public UInt64 ulBootAddress; //Device Boot Loader Start Address
        public byte ucRAMPZAddress; //RAMPZ Register address in SRAM I/O space
        public UInt32 uiFlashPageSize; //Device Flash Page Size, Size = 2 exp ucFlashPageSize
        public byte ucEepromPageSize; //Device Eeprom Page Size in bytes
        public UInt32 uiUpperExtIOLoc; //Topmost (last) extended I/O
                                       //location, 0 if no external I/O
        public UInt64 ulFlashSize; //Device Flash Size
        public fixed byte ucEepromInst[20]; //Instructions for W/R EEPROM
        public fixed byte ucFlashInst[3]; //Instructions for W/R FLASH
        public byte ucSPHaddr; // Stack pointer high
        public byte ucSPLaddr; // Stack pointer low
        public UInt32 uiFlashpages; // number of pages in flash
        public byte ucDWDRAddress; // DWDR register address
        public byte ucDWBasePC; // Base/mask value of the PC
        public byte ucAllowFullPageBitstream; // FALSE on ALL new parts
        public UInt32 uiStartSmallestBootLoaderSection; //
        public byte EnablePageProgramming; // For JTAG parts only, default TRUE
        public byte ucCacheType; // CacheType_Normal 0x00,
                                 // CacheType_CAN 0x01,
                                 // CacheType_HEIMDALL 0x02
        public UInt32 uiSramStartAddr; // Start of SRAM
        public byte ucResetType; // Selects reset type. ResetNormal = 0x00
                                 // ResetAT76CXXX = 0x01
        public byte ucPCMaskExtended; // For parts with extended PC
        public byte ucPCMaskHigh; // PC high mask
        public byte ucEindAddress; // Selects reset type.
        public UInt32 EECRAddress; // EECR IO address
    }
}
