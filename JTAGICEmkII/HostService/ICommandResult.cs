namespace JTAGICEmkII.HostService
{
    public interface ICommandResult
    {
        int ErrorCode { get; }
        string ErrorDesctiption { get; }
        bool IsSuccess { get; }
    }
}
