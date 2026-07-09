using JTAGICEmkII;

namespace JTAGICEmkII
{
    public interface IRxFrameAdaptor
    {
        bool IsByteToRead { get; }

        bool ReadBytes(out byte[]? values, uint length, int timeout = -1);
        Task<bool> ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout = -1);
    }
}