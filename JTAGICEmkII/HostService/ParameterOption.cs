namespace JTAGICEmkII.HostService
{
    [Flags]
    public enum ParameterOption : byte
    {
        None = 0,
        ReadOnly = 0x01,
        WriteOnly= 0x02,
        ReadWrite = ReadOnly | WriteOnly
    };
}