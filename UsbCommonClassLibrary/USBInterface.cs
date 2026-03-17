using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Windows.Devices.Usb;
using System.Linq;
using LibUsbDotNet;


namespace UsbCommonClassLibrary
{
    /// <summary>
    /// Base class for USB Interface.
    /// </summary>
    /// <seealso cref="UsbCommonClassLibrary.IUSBInterface" />
    /// <seealso cref="System.IDisposable" />
    public class USBInterface : IUSBInterface, IDisposable
    {
        // Member variables
        private int _interfaceNumber;
        private string _interfacePath = string.Empty;
        private uint _interfaceHanle;
        protected USBDevice? _parentDevice;
        private bool _disposedValue;
        protected List<UsbPipeInformation> _inPipe = new List<UsbPipeInformation>();
        protected List<UsbPipeInformation> _outPipe = new List<UsbPipeInformation>();


        /// <summary>
        /// Creates the usb interfaces.
        /// </summary>
        /// <param name="usbDevice">The usb device.</param>
        /// <param name="requiredPipeTypes">The pipe types required.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static int CreateUsbInterface(USBDevice usbDevice, IEnumerable<USBPipeTypeEnum> requiredPipeTypes)
        {
            ArgumentNullException.ThrowIfNull(usbDevice);
            ArgumentNullException.ThrowIfNull(requiredPipeTypes);

            usbDevice.Interfaces.Clear();

            for (int i = 0; i < usbDevice.InterfaceDescriptor.NumEndpoints; i++)
            {
                UsbPipeInformation? pipeInformation = new UsbPipeInformation();
                if (!WinUsbApi.QueryPipe(usbDevice.UsbHandle, 0, (byte)i, out pipeInformation))
                {
                    usbDevice.Logger.LogError("Failed to query pipe information for interface {InterfaceNumber}, pipe index {PipeIndex}, {message}", usbDevice.InterfaceDescriptor.InterfaceNumber, i, Marshal.GetLastPInvokeErrorMessage());
                    pipeInformation = null;
                }
                else
                {
                    if (requiredPipeTypes.Any(pt => pt == pipeInformation.PipeType))
                    {
                        usbDevice.Logger.LogInformation("Creating USB Interface for interface {InterfaceNumber}, pipe index {PipeIndex}, PipeType {PipeType}, EndpointAddress {EndpointAddress}", usbDevice.InterfaceDescriptor.InterfaceNumber, i, pipeInformation.PipeType, pipeInformation.PipeId);
                        USBInterface usbInterface = new USBInterface(usbDevice.Logger, usbDevice);

                        if (pipeInformation.IsEndpointOut)
                        {
                            usbInterface._outPipe.Add(pipeInformation);
                        }
                        else
                        {
                            usbInterface._inPipe.Add(pipeInformation);

                            if(pipeInformation.PipeType == USBPipeTypeEnum.UsbdPipeTypeBulk)
                            {
                                if (!WinUsbApi.SetPipePolicy(usbDevice.UsbHandle, pipeInformation.PipeId, USBPipePolicyEnum.RawIo, 1))
                                {
                                    usbDevice.Logger.LogError("Failed to set pipe policy for interface {InterfaceNumber}, pipe id {PipeId}, {message}", usbDevice.InterfaceDescriptor.InterfaceNumber, pipeInformation.PipeId, Marshal.GetLastPInvokeErrorMessage());
                                }
                            }
                        }

                        usbDevice.Interfaces.Add(usbInterface);
                    }
                }
            }

            return usbDevice.Interfaces.Count();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="USBInterface"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="device">The device.</param>
        public USBInterface(ILogger<USBInterface> logger, IUSBDevice device)
        {
            this.Logger = logger;
            this._parentDevice = (USBDevice)device;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="USBInterface"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="device">The device.</param>
        internal USBInterface(ILogger logger, USBDevice device)
        {
            this.Logger = logger;
            this._parentDevice = (USBDevice)device;
        }


        // Properties
        public int InterfaceNumber { get => this._interfaceNumber; set => this._interfaceNumber = value; }
        public string InterfacePath { get => this._interfacePath; set => this._interfacePath = value; }
        public ILogger Logger { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this.Logger.LogInformation("Disposing USB Interface: {InterfaceNumber}", this._interfaceNumber);
                    // TODO: dispose managed state (managed objects)
                }

                this._parentDevice = null;

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this._disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~USBInterface()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
