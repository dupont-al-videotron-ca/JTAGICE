namespace JTAGICEmkII.HostService
{
    public interface ICommandRequest
    {
        bool IsRequestTimeout { get; }
        int RetryCount { get; }
        bool WaitForResponse();
    }

    public interface ICommandRequest<S, R> : ICommandRequest
            where S : class // send command type
            where R : class // receive response type
    {
        S Command { get; }

        R? Response { get; }
    }


}
