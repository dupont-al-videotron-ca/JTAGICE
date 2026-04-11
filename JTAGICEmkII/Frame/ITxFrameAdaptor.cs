
namespace JTAGICEmkII
{
    public interface ITxFrameAdaptor
    {
        byte[] Buffer { get; }

        int SendBytes(byte[] values);
        Task<int> SendBytesAsync(byte[] values, CancellationToken cancellationToken);
    }
}