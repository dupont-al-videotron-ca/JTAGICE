namespace JTAGICEmkII.Slave
{
    public static class SelfTestReponseHelper
    {
        private static readonly Dictionary<SelfTestReponseEnum, string> stringTable;

        static SelfTestReponseHelper()
        {
            stringTable = new Dictionary<SelfTestReponseEnum, string>
            {
                { SelfTestReponseEnum.SELFTEST_SKIPPED, "This self-test was not executed." },
                { SelfTestReponseEnum.SELFTEST_OK, "Self-test passed." },
                { SelfTestReponseEnum.SELFTEST_FAILED, "Self-test failed." },
                { SelfTestReponseEnum.ST_USART_FAILURE, "Internal USART test failed." },
                { SelfTestReponseEnum.ST_FIFO_M_FAILURE, "Master side FIFO reading failed." },
                { SelfTestReponseEnum.ST_FIFO_S_FAILURE, "Slave side FIFO reading failed." },
                { SelfTestReponseEnum.ST_FIFO_M_EMPTY_FAILURE, "Master side EMPTY bit reading failed." },
                { SelfTestReponseEnum.ST_FIFO_S_EMPTY_FAILURE, "Slave side EMPTY bit reading failed." },
                { SelfTestReponseEnum.ST_FIFO_M_FULL_FAILURE, "Master side FULL bit reading failed." },
                { SelfTestReponseEnum.ST_FIFO_S_FULL_FAILURE, "Slave side FULL bit reading failed." },
                { SelfTestReponseEnum.ST_FIFO_M_NINE_FAILURE, "Master side NINTH bit reading failed." },
                { SelfTestReponseEnum.ST_FIFO_S_NINE_FAILURE, "Slave side NINTH bit reading failed." },
                { SelfTestReponseEnum.ST_SRAM_FAILURE, "Internal SRAM read-write test failed." },
                { SelfTestReponseEnum.ST_JTAG_TMS_STUCK_HIGH, "JTAG TMS line cannot be driven low!" },
                { SelfTestReponseEnum.ST_JTAG_TCK_STUCK_HIGH, "JTAG TCK line cannot be driven low!" },
                { SelfTestReponseEnum.ST_JTAG_TDI_STUCK_HIGH, "JTAG TDI line cannot be driven low!" },
                { SelfTestReponseEnum.ST_JTAG_TMS_STUCK_LOW, "JTAG TMS line cannot be driven high!" },
                { SelfTestReponseEnum.ST_JTAG_TCK_STUCK_TMS, "JTAG lines TCK and TMS are possibly tied together." },
                { SelfTestReponseEnum.ST_JTAG_TDI_STUCK_TMS, "JTAG lines TDI and TMS are possibly tied together." },
                { SelfTestReponseEnum.ST_JTAG_TCK_STUCK_LOW, "JTAG TCK line cannot be driven high!" },
                { SelfTestReponseEnum.ST_JTAG_TMS_STUCK_TCK, "JTAG lines TMS and TCK are possibly tied together." },
                { SelfTestReponseEnum.ST_JTAG_TDI_STUCK_TCK, "JTAG lines TDI and TCK are possibly tied together." },
                { SelfTestReponseEnum.ST_JTAG_TDI_STUCK_LOW, "JTAG TDI line cannot be driven high!" },
                { SelfTestReponseEnum.ST_JTAG_TMS_STUCK_TDI, "JTAG lines TMS and TDI are possibly tied together." },
                { SelfTestReponseEnum.ST_JTAG_TCK_STUCK_TDI, "JTAG lines TCK and TDI are possibly tied together." }
            };
        }


        public static string ToString(SelfTestReponseEnum selfTestReponseEnum)
        {
            if (stringTable.TryGetValue(selfTestReponseEnum, out string? result))
                return result;
            else
                return $"Unknown SelfTestReponseEnum value: 0x{((byte)selfTestReponseEnum):X2}";

        }
    }
}
