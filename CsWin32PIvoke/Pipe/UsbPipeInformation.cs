using Windows.Win32.Devices.Usb;
namespace CsWin32Api
{

    /// <summary>
    /// usb pipe information class.
    /// </summary>
    public class UsbPipeInformation
    {
        internal WINUSB_PIPE_INFORMATION _src;

        internal UsbPipeInformation(WINUSB_PIPE_INFORMATION src)
        {
            this._src = src;
            this.PipeType = (USBPipeTypeEnum)src.PipeType;
            this.PipeId = src.PipeId;
            this.MaximumPacketSize = src.MaximumPacketSize;
            this.Interval = src.Interval;
        }

        public UsbPipeInformation()
        {
            this._src = new WINUSB_PIPE_INFORMATION();
        }

        public USBPipeTypeEnum PipeType { get => (USBPipeTypeEnum)this._src.PipeType; set => this._src.PipeType = (USBD_PIPE_TYPE)value; }

        /// <summary>
        /// Gets or sets the pipe identifier. Last bit is direction and rest is endpoint number.
        /// </summary>
        /// <value>
        /// The pipe identifier.
        /// </value>
        public byte PipeId { get => this._src.PipeId; set => this._src.PipeId = value; }

        public byte EndpointNumber { get => (byte)(this._src.PipeId & (byte)USBEndpointEnum.UsbdEnpointIdMask); }
        public bool IsEndpointOut { get => (byte)(this._src.PipeId & (byte)USBEndpointEnum.UsbPipeDirectionMask) == (byte)USBEndpointEnum.UsbPipeDirectionOut; }
        public bool IsEndpointIn { get => !this.IsEndpointOut; }

        public ushort MaximumPacketSize { get => this._src.MaximumPacketSize; set => this._src.MaximumPacketSize = value; }

        public byte Interval { get => this._src.Interval; set => this._src.Interval = value; }
    }
}
