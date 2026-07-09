
namespace JTAGICEmkII
{
    public interface ITxFrameAdaptor
    {
        int SendBytes(byte[] values);

        Task<int> SendBytesAsync(byte[] values, CancellationToken cancellationToken);
    }
}