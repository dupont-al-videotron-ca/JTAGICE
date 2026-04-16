using JTAGICEmkII.Master;

namespace JTAGICEmkII.HostService
{
    public partial class ProgrammingOptions
    {

        public ProgrammingOptions() 
        {
            IsEraseDestination = true;
            IsProgrammDestination = true;
            IsVerifyDestination = true;
            MemoryType = MemoryTypeEnum.MT_FLASH_PAGE;
            PageSize = 0x10000; // Default page size for flash memory, can be changed based on the target device
            ByteCount = 0;
            Address = 0;
            ResultErrorAddress = Address;
        }

        public bool IsEraseDestination { get; set; }
        public bool IsVerifyDestination { get; set; }
        public bool IsProgrammDestination { get; set; }

        public MemoryTypeEnum MemoryType { get; set; }

        public UInt32 PageSize { get; set; } 

        public uint ByteCount { get; set; }
        public UInt64 Address { get; set; }

        public List<byte> Data { get; init; } = new List<byte>();

        public bool IsProgrammingSuccessfull { get => ProgrammingResult == ProgrammingResultEnum.PR_SUCCESS; }

        public ProgrammingResultEnum ProgrammingResult { get; set; } = ProgrammingResultEnum.PR_SUCCESS;
        
        public UInt64 ResultErrorAddress { get; set; } = 0;

        public byte ExpectedData { get; set; } = 0;

        public byte ReadData { get; set; } = 0;

        public override string? ToString()
        {
            if(IsProgrammingSuccessfull)
            {
                return $"Programming successful: MemoryType={MemoryType}, Address=0x{Address:X}, ByteCount={ByteCount}";
            }
            else
            {
                return $"Programming failed: MemoryType={MemoryType}, Address=0x{Address:X}, ByteCount={ByteCount}, ResultErrorAddress=0x{ResultErrorAddress:X}, ExpectedData=0x{ExpectedData:X2}, ReadData=0x{ReadData:X2}";
            }
        }
    }
}