namespace UsbDeviceBase
{
    public interface IUsbPipeIn: IUsbPipe
    {

        bool IsByteToRead { get; }

        bool ReadBytes(out byte[] values, int length, int timeout = -1);

        Task<bool> ReadBytesAsync(out byte[] values, int length, CancellationToken cancellationToken, int timeout = -1);

        bool ReadStructure<T>(ref T obj, int timeout = -1) where T : struct;

        Task<T?> ReadStructureAsync<T>(CancellationToken cancellationToken, int timeout = -1) where T : struct;

    }
}