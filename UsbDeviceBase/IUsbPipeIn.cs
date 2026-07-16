namespace UsbDeviceBase
{
    public interface IUsbPipeIn
    {

        bool IsByteToRead { get; }

        bool ReadBytes(out byte[] values, uint length, int timeout = -1);

        Task<bool> ReadBytesAsync(out byte[] values, uint length, CancellationToken cancellationToken, int timeout = -1);

        bool ReadStructure<T>(ref T obj, int timeout) where T : struct;

    }
}