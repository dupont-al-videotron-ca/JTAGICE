using System.Runtime.InteropServices;
using CsWin32Api;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;

namespace UsbCommonClassLibrary
{
    /// <summary>
    /// Usb device base class.
    /// </summary>
    /// <seealso cref="UsbCommonClassLibrary.IUSBDevice" />
    public class USBDevice : IUSBDevice, IDisposable
    {
        // Member variables
        private string _path = string.Empty;
        private Guid _guid = Guid.Empty;
        private bool _disposedValue;

        private SafeFileHandle _deviceHandle = new SafeFileHandle();
        private UsbSafeHandle _usbHandle = new UsbSafeHandle();
        private List<USBInterface> _interfaces = [];
        private UsbInterfaceDescriptor _interfaceDescriptor = new UsbInterfaceDescriptor();

        protected USBDevice(ILogger<USBDevice> logger)
        {
            this.Logger = logger;
        }

        // Properties
        public SafeFileHandle DeviceHandle { get => this._deviceHandle; }
        public Guid Guid { get => this._guid; set => this._guid = value; }
        public string Path { get => this._path; set => this._path = value; }
        public List<USBInterface> Interfaces { get => this._interfaces; }
        public ILogger<USBDevice> Logger { get; }

        public UsbSafeHandle UsbHandle { get => this._usbHandle; }

        public UsbInterfaceDescriptor InterfaceDescriptor { get => this._interfaceDescriptor; }

        public bool IsOpen
        {
            get
            {
                return !this.DeviceHandle.IsInvalid && !this.DeviceHandle.IsClosed && !this.UsbHandle.IsInvalid && !this.UsbHandle.IsClosed;
            }
        }

        // Methods   

        /// <summary>
        /// Opens the device.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public virtual int OpenDevice(string path)
        {
            this._deviceHandle = WinUsbEnumerator.CreateUsbHandle(path);
            if (this._deviceHandle.IsInvalid)
            {
                var lasterror = Marshal.GetHRForLastWin32Error();
                this.Logger.LogError("Failed to open USB device at path: {Path}, {message}", path, Marshal.GetLastPInvokeErrorMessage());
                return lasterror;
            }

            this.Logger.LogInformation("USB device at path: {Path} opened {handle}", path, this.DeviceHandle);

            if (!WinUsbEnumerator.InitializeUsbHandle(this._deviceHandle, out this._usbHandle))
            {
                var lasterror = Marshal.GetHRForLastWin32Error();
                this.Logger.LogError("Failed to initialize USB handle for device at path: {Path}, {message}", path, Marshal.GetLastPInvokeErrorMessage());
                this.CloseDevice();
                return lasterror;
            }

            this.Logger.LogInformation("USB handle initialized {handle}", this.UsbHandle);

            if (!WinUsbEnumerator.QueryInterfaceSettings(this._usbHandle, 0, out this._interfaceDescriptor))
            {
                var lasterror = Marshal.GetHRForLastWin32Error();
                this.Logger.LogError("Failed to query interface settings for device at path: {Path}, {message}", path, Marshal.GetLastPInvokeErrorMessage());
                this.CloseDevice();
                return lasterror;
            }

            this.Logger.LogInformation("USB interface settings queried for device at path: {Path} {info}", path, this._interfaceDescriptor);

            return 0;
        }

        /// <summary>
        /// Closes the device.
        /// </summary>
        public void CloseDevice()
        {
            if (this._deviceHandle != null &&
                !this._deviceHandle.IsClosed &&
                !this._deviceHandle.IsInvalid)
            {
                this._deviceHandle.Close();
            }

            if (this._usbHandle != null &&
                !this._usbHandle.IsClosed &&
                !this._usbHandle.IsInvalid)
            {
                this._usbHandle.Close();
            }
        }


        /// <summary>
        /// Throws if closed.
        /// </summary>
        /// <exception cref="UsbCommonClassLibrary.UsbExceptionDeviceNotOpen"></exception>
        protected void ThrowIfClosed()
        {
            if (!this.IsOpen)
                throw new UsbExceptionDeviceNotOpen();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    foreach (var usbInterface in this._interfaces)
                    {
                        usbInterface.Dispose();
                    }

                    if (this.DeviceHandle.IsClosed || this.DeviceHandle.IsInvalid == false)
                    {
                        this.DeviceHandle.Dispose();
                        this.Logger.LogInformation("USB device handle disposed.");
                    }

                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this._disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~USBDevice()
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
