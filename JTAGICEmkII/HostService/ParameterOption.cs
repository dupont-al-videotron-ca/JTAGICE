namespace JTAGICEmkII.HostService
{
    [Flags]
    public enum ParameterOption : byte
    {
        None = 0,
        ReadOnly = 1,
        WriteOnly= 2,
        ReadWrite = ReadOnly | WriteOnly
    };
}