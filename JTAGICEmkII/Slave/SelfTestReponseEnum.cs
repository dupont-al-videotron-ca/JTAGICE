using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace JTAGICEmkII.Slave
{

    public enum SelfTestReponseEnum : byte
    {
        SELFTEST_SKIPPED = 0x00, // This self-test was not executed
        SELFTEST_OK = 0x01, //Self-test passed
        SELFTEST_FAILED = 0x80, //Self-test failed
        ST_USART_FAILURE = 0x81, // Internal USART test failed
        ST_FIFO_M_FAILURE = 0x82, // Master side FIFO reading failed
        ST_FIFO_S_FAILURE = 0x83, // Slave side FIFO reading failed
        ST_FIFO_M_EMPTY_FAILURE = 0x84, // Master side EMPTY bit reading failed
        ST_FIFO_S_EMPTY_FAILURE = 0x85, // Slave side EMPTY bit reading failed
        ST_FIFO_M_FULL_FAILURE = 0x86, // Master side FULL bit reading failed
        ST_FIFO_S_FULL_FAILURE = 0x87, // Slave side FULL bit reading failed
        ST_FIFO_M_NINE_FAILURE = 0x88, // Master side NINTH bit reading failed
        ST_FIFO_S_NINE_FAILURE = 0x89, // Slave side NINTH bit reading failed
        ST_SRAM_FAILURE = 0x8A, // Internal SRAM read-write test failed
        ST_JTAG_TMS_STUCK_HIGH = 0x8B, // JTAG TMS line cannot be driven low!
        ST_JTAG_TCK_STUCK_HIGH = 0x8C, //JTAG TCK line cannot be driven low! 
        ST_JTAG_TDI_STUCK_HIGH = 0x8D, //JTAG TDI line cannot be driven low!
        ST_JTAG_TMS_STUCK_LOW = 0x8E, //JTAG TMS line cannot be driven high!
        ST_JTAG_TCK_STUCK_TMS = 0x8F, //JTAG lines TCK and TMS are possibly tied together.
        ST_JTAG_TDI_STUCK_TMS = 0x90, //JTAG lines TDI and TMS are possibly tied together.
        ST_JTAG_TCK_STUCK_LOW = 0x91, //JTAG TCK line cannot be driven high!
        ST_JTAG_TMS_STUCK_TCK = 0x92, //JTAG lines TMS and TCK are possibly tied together.
        ST_JTAG_TDI_STUCK_TCK = 0x93, //JTAG lines TDI and TCK are possibly tied together.
        ST_JTAG_TDI_STUCK_LOW = 0x94, //JTAG TDI line cannot be driven high!
        ST_JTAG_TMS_STUCK_TDI = 0x95, //JTAG lines TMS and TDI are possibly tied together.
        ST_JTAG_TCK_STUCK_TDI = 0x96, //JTAG lines TCK and TDI are possibly tied together.
    }
}
