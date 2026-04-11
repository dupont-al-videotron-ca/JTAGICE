namespace JTAGICEmkII.HostService
{
    public sealed class Parameters : Dictionary<Master.ParameterEnum, Parameter>
    {
        public Parameters() : base()
        {
            this.Add(Master.ParameterEnum.PARAM_HWD_VERSION, new Parameter(Master.ParameterEnum.PARAM_HWD_VERSION, ParameterOption.ReadOnly));
            this.Add(Master.ParameterEnum.PARAM_FW_VERSION, new Parameter(Master.ParameterEnum.PARAM_FW_VERSION, ParameterOption.ReadOnly));
            this.Add(Master.ParameterEnum.PARAM_EMULATOR_MODE, new Parameter(Master.ParameterEnum.PARAM_EMULATOR_MODE, ParameterOption.ReadWrite));
            this.Add(Master.ParameterEnum.PARAM_IRE, new Parameter(Master.ParameterEnum.PARAM_IRE, ParameterOption.ReadWrite, 2));
            this.Add(Master.ParameterEnum.PARAM_BAUD_RATE, new Parameter(Master.ParameterEnum.PARAM_BAUD_RATE, ParameterOption.ReadWrite));
            this.Add(Master.ParameterEnum.PARAM_OCD_VTARGET, new Parameter(Master.ParameterEnum.PARAM_OCD_VTARGET, ParameterOption.ReadOnly, 2));
            this.Add(Master.ParameterEnum.PARAM_OCD_JTAG_CLOCK, new Parameter(Master.ParameterEnum.PARAM_OCD_JTAG_CLOCK, ParameterOption.ReadWrite));
            this.Add(Master.ParameterEnum.PARAM_OCD_BREAK_CAUSE, new Parameter(Master.ParameterEnum.PARAM_OCD_BREAK_CAUSE, ParameterOption.ReadOnly));
            this.Add(Master.ParameterEnum.PARAM_TIMERS_RUNNING, new Parameter(Master.ParameterEnum.PARAM_TIMERS_RUNNING, ParameterOption.ReadWrite));
            this.Add(Master.ParameterEnum.PARAM_BREAK_ON_CHANGE_FLOW, new Parameter(Master.ParameterEnum.PARAM_BREAK_ON_CHANGE_FLOW, ParameterOption.ReadWrite));
            this.Add(Master.ParameterEnum.PARAM_BREAK_ADDR1, new Parameter(Master.ParameterEnum.PARAM_BREAK_ADDR1, ParameterOption.ReadWrite, 2, true));
            this.Add(Master.ParameterEnum.PARAM_BREAK_ADDR2, new Parameter(Master.ParameterEnum.PARAM_BREAK_ADDR2, ParameterOption.ReadWrite, 2, true));
            this.Add(Master.ParameterEnum.PARAM_COMB_BREAK_CTRL, new Parameter(Master.ParameterEnum.PARAM_COMB_BREAK_CTRL, ParameterOption.ReadWrite, 1, true));
            this.Add(Master.ParameterEnum.PARAM_JTAGID_STRING, new Parameter(Master.ParameterEnum.PARAM_JTAGID_STRING, ParameterOption.ReadOnly, 4));
            this.Add(Master.ParameterEnum.PARAM_UNITS_BEFORE, new Parameter(Master.ParameterEnum.PARAM_UNITS_BEFORE, ParameterOption.ReadWrite, 1, true));
            this.Add(Master.ParameterEnum.PARAM_UNITS_AFTER, new Parameter(Master.ParameterEnum.PARAM_UNITS_AFTER, ParameterOption.ReadWrite, 1, true));
            this.Add(Master.ParameterEnum.PARAM_BIT_BEFORE, new Parameter(Master.ParameterEnum.PARAM_BIT_BEFORE, ParameterOption.ReadWrite, 1, true));
            this.Add(Master.ParameterEnum.PARAM_BIT_AFTER, new Parameter(Master.ParameterEnum.PARAM_BIT_AFTER, ParameterOption.ReadWrite, 1, true));

            this.Add(Master.ParameterEnum.PARAM_EXTERNAL_RESET, new Parameter(Master.ParameterEnum.PARAM_EXTERNAL_RESET, ParameterOption.ReadWrite));
            this.Add(Master.ParameterEnum.PARAM_FLASH_PAGE_SIZE, new Parameter(Master.ParameterEnum.PARAM_FLASH_PAGE_SIZE, ParameterOption.ReadWrite, 2));
            this.Add(Master.ParameterEnum.PARAM_EEPROM_PAGE_SIZE, new Parameter(Master.ParameterEnum.PARAM_EEPROM_PAGE_SIZE, ParameterOption.ReadWrite));
            this.Add(Master.ParameterEnum.PARAM_PSB0, new Parameter(Master.ParameterEnum.PARAM_PSB0, ParameterOption.ReadWrite, 2, true));
            this.Add(Master.ParameterEnum.PARAM_PSB1, new Parameter(Master.ParameterEnum.PARAM_PSB1, ParameterOption.ReadWrite, 2, true));
            this.Add(Master.ParameterEnum.PARAM_PROTOCOL_DEBUG_EVENT, new Parameter(Master.ParameterEnum.PARAM_PROTOCOL_DEBUG_EVENT, ParameterOption.ReadWrite));
            this.Add(Master.ParameterEnum.PARAM_TARGET_MCU_STATE, new Parameter(Master.ParameterEnum.PARAM_TARGET_MCU_STATE, ParameterOption.ReadOnly));
            this.Add(Master.ParameterEnum.PARAM_DAISY_CHAIN_INFO, new Parameter(Master.ParameterEnum.PARAM_DAISY_CHAIN_INFO, ParameterOption.ReadWrite, 4));
            this.Add(Master.ParameterEnum.PARAM_BOOT_ADDRESS, new Parameter(Master.ParameterEnum.PARAM_BOOT_ADDRESS, ParameterOption.ReadWrite, 4));
            this.Add(Master.ParameterEnum.PARAM_TARGET_SIGNATURE, new Parameter(Master.ParameterEnum.PARAM_TARGET_SIGNATURE, ParameterOption.ReadOnly, 2));
            this.Add(Master.ParameterEnum.PARAM_DEBUGWIRE_BAUDRATE, new Parameter(Master.ParameterEnum.PARAM_DEBUGWIRE_BAUDRATE, ParameterOption.ReadWrite, 4, true));
            this.Add(Master.ParameterEnum.PARAM_PROGRAM_ENTRY_POINT, new Parameter(Master.ParameterEnum.PARAM_PROGRAM_ENTRY_POINT, ParameterOption.WriteOnly, 4));
            this.Add(Master.ParameterEnum.PARAM_PACKET_PARSING_ERROR, new Parameter(Master.ParameterEnum.PARAM_PACKET_PARSING_ERROR, ParameterOption.ReadOnly, 4));
            this.Add(Master.ParameterEnum.PARAM_VALID_PACKETS_RECEIVED, new Parameter(Master.ParameterEnum.PARAM_VALID_PACKETS_RECEIVED, ParameterOption.ReadOnly, 4));
            this.Add(Master.ParameterEnum.PARAM_INTERCOMMUNICATION_TX_ERROR, new Parameter(Master.ParameterEnum.PARAM_INTERCOMMUNICATION_TX_ERROR, ParameterOption.ReadOnly, 4));
            this.Add(Master.ParameterEnum.PARAM_INTERCOMMUNICATION_RX_ERROR, new Parameter(Master.ParameterEnum.PARAM_INTERCOMMUNICATION_RX_ERROR, ParameterOption.ReadOnly, 4));
            this.Add(Master.ParameterEnum.PARAM_CRC_ERROR, new Parameter(Master.ParameterEnum.PARAM_CRC_ERROR, ParameterOption.ReadOnly, 4));
            this.Add(Master.ParameterEnum.PARAM_POWER_SOURCE, new Parameter(Master.ParameterEnum.PARAM_POWER_SOURCE, ParameterOption.ReadOnly));
            this.Add(Master.ParameterEnum.PARAM_CAN_FLAG, new Parameter(Master.ParameterEnum.PARAM_CAN_FLAG, ParameterOption.ReadWrite));
            this.Add(Master.ParameterEnum.PARAM_PAR_ENABLE_IDR_IN_RUN_MODE, new Parameter(Master.ParameterEnum.PARAM_PAR_ENABLE_IDR_IN_RUN_MODE, ParameterOption.WriteOnly));
            this.Add(Master.ParameterEnum.PARAM_PAR_ALLOW_PAGEPROGRAMMING_INSCANCHAIN, new Parameter(Master.ParameterEnum.PARAM_PAR_ALLOW_PAGEPROGRAMMING_INSCANCHAIN, ParameterOption.WriteOnly));
        }

        public bool GetIsRead(Master.ParameterEnum paramId)
        {
            return this[paramId].IsRead;
        }

        public bool GetIsWrite(Master.ParameterEnum paramId)
        {
            return this[paramId].IsWrite;
        }

        public IReadOnlyDictionary<Master.ParameterEnum, Parameter> GetAllRead()
        {
            return this.Where(kv => kv.Value.IsRead).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public IReadOnlyDictionary<Master.ParameterEnum, Parameter> GetAllWrite()
        {
            return this.Where(kv => kv.Value.IsWrite).ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}