namespace CsWin32Api
{
    [Flags]
    public enum USBEndpointEnum : byte
    {
        UsbdEnpointIdMask = 0x7F,
        UsbPipeDirectionMask = 0x80,

        UsbPipeDirectionOut = 0x00,
        UsbPipeDirectionIn = 0x80,

    }
}
