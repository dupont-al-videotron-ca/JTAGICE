using Windows.Win32.Devices.Usb;
namespace CsWin32Api
{
    public enum USBPipePolicyEnum : uint
    {
        ShortPacketTerminate = WINUSB_PIPE_POLICY.SHORT_PACKET_TERMINATE,
        AutoClearStall = WINUSB_PIPE_POLICY.AUTO_CLEAR_STALL,
        PipeTransferTimeout = WINUSB_PIPE_POLICY.PIPE_TRANSFER_TIMEOUT,
        IgnoreShortPackets = WINUSB_PIPE_POLICY.IGNORE_SHORT_PACKETS,
        AllowPartialReads = WINUSB_PIPE_POLICY.ALLOW_PARTIAL_READS,
        AutoFlush = WINUSB_PIPE_POLICY.AUTO_FLUSH,
        RawIo = WINUSB_PIPE_POLICY.RAW_IO,
        MaximumTransferSize = WINUSB_PIPE_POLICY.MAXIMUM_TRANSFER_SIZE,
        ResetPipeOnResume = WINUSB_PIPE_POLICY.RESET_PIPE_ON_RESUME,
    }
}
