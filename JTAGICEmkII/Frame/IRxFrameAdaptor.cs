using JTAGICEmkII;

namespace JTAGICEmkII
{
    public interface IRxFrameAdaptor
    {
        bool IsByteToRead { get; }
        int RxTimeout { get; }
        bool WaitForTimeout { get; set; }

        void Attach(RxFrame rxFrame);
        bool ReadByte(out byte value, int timeout = -1);
        Task<bool> ReadByteAsync(out byte value, CancellationToken cancellationToken, int timeout = -1);
        bool ReadBytes(out byte[]? values, uint length, int timeout = -1);
        Task<bool> ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout = -1);
    }
}