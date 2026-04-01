#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8618, CS8603, CS8604, CA1416
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using MemoryPack;
//using Windows.Devices.Usb;
using Windows.Foundation.Metadata;
using Windows.Storage.Streams;

namespace UsbDeviceBase
{
    public abstract class UsbDeviceBase : IDisposable, IUsbDevice
    {


        #region Constructors 

        protected UsbDeviceBase()
        {
            this.Logger = LogManager.GetLogger(this.GetType());
        }

        #endregion


        #region Fields 

        private bool disposedValue;

        #endregion


        #region Properties 

        public UsbInterfaceControl UsbInterfaceControl
        {
            get
            {
                return GetUsbInterfaceControl();
            }
        }

        public Windows.Storage.Streams.ByteOrder ByteOrder { get; init; } = Windows.Storage.Streams.ByteOrder.LittleEndian;
        public Windows.Storage.Streams.UnicodeEncoding UnicodeEncoding { get; set; } = Windows.Storage.Streams.UnicodeEncoding.Utf8;

        protected MemoryPackSerializerOptions SerializerOptions
        {
            get
            {
                if (UnicodeEncoding == Windows.Storage.Streams.UnicodeEncoding.Utf8)
                {
                    return MemoryPackSerializerOptions.Utf8;
                }
                else
                {
                    return MemoryPackSerializerOptions.Utf16;
                }
            }
        }

        protected Dictionary<int, UsbInterfaceBase> ImplInterfaces { get; } = new Dictionary<int, UsbInterfaceBase>();

        protected Windows.Devices.Usb.UsbConfigurationDescriptor UsbConfigurationDescriptor
        {
            get
            {
                ThrowIfNoWindowsDevice();
                return this.WindowsDevice.Configuration.ConfigurationDescriptor;
            }
        }

        protected ILog Logger { get; }

        public abstract ushort VendorId { get; }

        public abstract ushort ProductId { get; }

        public abstract string DeviceId { get; }

        public abstract string DeviceName { get; }

        //
        // Summary:
        //     Gets the **bConfigurationValue** field of a USB configuration descriptor. The
        //     value is the number that identifies the configuration.
        //
        // Returns:
        //     The number that identifies the configuration.
        public byte ConfigurationValue => UsbConfigurationDescriptor.ConfigurationValue;

        //
        // Summary:
        //     Gets the **bMaxPower** field of a USB configuration descriptor. The value indicates
        //     the maximum power (in milliamp units) that the device can draw from the bus,
        //     when the device is bus-powered.
        //
        // Returns:
        //     The maximum power (in milliamp units) that the device can draw from the bus.
        public uint MaxPowerMilliamps => UsbConfigurationDescriptor.MaxPowerMilliamps;

        //
        // Summary:
        //     Gets the D5 bit value of the **bmAttributes** field in the USB configuration
        //     descriptor. The value indicates whether the device can send a resume signal to
        //     wake up itself or the host system from a low power state.
        //
        // Returns:
        //     True, if the device supports remote wakeup; otherwise false.
        public bool RemoteWakeup => UsbConfigurationDescriptor.RemoteWakeup;

        //
        // Summary:
        //     Gets the D6 bit of the **bmAttributes** field in the USB configuration. This
        //     value indicates whether the device is drawing power from a local source or the
        //     bus.
        //
        // Returns:
        //     True, if the device is drawing power from a local source; false indicates that
        //     the device is only drawing power from the bus.
        public bool SelfPowered => UsbConfigurationDescriptor.SelfPowered;

        protected Windows.Devices.Usb.UsbDevice WindowsDevice { get; set; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        #endregion


        #region Protected Methods 

        #endregion

        #region Provate Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion


        // Methods
        public uint SendControlOutTransfer(Windows.Devices.Usb.UsbSetupPacket usp)
        {
            Task<uint> t = this.SendControlOutTransferAsync(usp);
            t.Wait();
            return t.Result;
        }

        public async Task<uint> SendControlOutTransferAsync(Windows.Devices.Usb.UsbSetupPacket usp)
        {
            try
            {
                this.ThrowIfNoWindowsDevice();
                return await this.WindowsDevice.SendControlOutTransferAsync(usp);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send control out transfer. Exception: {ex}");
                throw;
            }
        }

        public IBuffer SendControlInTransfer(Windows.Devices.Usb.UsbSetupPacket usp, int bufferLength)
        {
            try
            {
                this.ThrowIfNoWindowsDevice();
                var t = this.SendControlInTransferAsync(usp, bufferLength);
                t.Wait();
                return t.Result;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send control in transfer. Exception: {ex}");
                throw;
            }
        }

        public async Task<IBuffer> SendControlInTransferAsync(Windows.Devices.Usb.UsbSetupPacket usp, int bufferLength)
        {
            IBuffer buffer = WindowsRuntimeBufferExtensions.AsBuffer(new byte[bufferLength]);

            this.ThrowIfNoWindowsDevice();
            return await this.WindowsDevice.SendControlInTransferAsync(usp, buffer);
        }

        public T SendControlInTransfer<T>(Windows.Devices.Usb.UsbSetupPacket usp, int bufferLength)
        {
            IBuffer buffer = SendControlInTransfer(usp, bufferLength);
            byte[] data = buffer.ToArray();

            if (data == null || data.Length == 0)
            {
                Logger.Info($"Received empty buffer from control in transfer. Returning default value.");
                return (default);
            }

            return MemoryPackSerializer.Deserialize<T>(data, this.SerializerOptions);
        }

        public Task<T?> SendControlInTransferAsync<T>(Windows.Devices.Usb.UsbSetupPacket usp, int bufferLength)
        {
            try
            {
                this.ThrowIfNoWindowsDevice();
                Task<IBuffer> t = SendControlInTransferAsync(usp, bufferLength);
                t.Wait();
                IBuffer buffer = t.Result;
                byte[] data = buffer.ToArray();

                if(data == null || data.Length == 0)
                {
                    Logger.Info($"Received empty buffer from control in transfer. Returning default value.");
                    return Task.FromResult<T?>(default);
                }

                T? tResult = MemoryPackSerializer.Deserialize<T>(data, this.SerializerOptions);
                return Task.FromResult(tResult);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to send control in transfer. Exception: {ex}");
                throw;
            }
        }


        protected abstract UsbInterfaceBase CreateInterface(Windows.Devices.Usb.UsbInterfaceDescriptor descriptor, IEnumerable<KeyValuePair<int, PipeOutBase>> outPipes, IEnumerable<KeyValuePair<int, PipeInBase>> inPipes);


        private UsbInterfaceControl GetUsbInterfaceControl()
        {
            ThrowIfNoWindowsDevice();
            if (!ImplInterfaces.ContainsKey(0))
            {
                throw new InvalidOperationException($"Interface 0 is not initialized. Call InitializeAsync() first.");
            }

            return this.ImplInterfaces[0] as UsbInterfaceControl;
        }

        protected async Task CreateWindowsDevicesAsync()
        {
            try
            {
                string aqs = Windows.Devices.Usb.UsbDevice.GetDeviceSelector(this.VendorId, this.ProductId);

                var myDevices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(aqs);

                // Windows cannot find the device, which may be caused by wrong VendorId/ProductId or missing USB driver.
                // In this case, we should not throw an exception, but log an error a.
                if (myDevices.Count == 0)
                {
                    // try using DeviceId to find the device.

                    // This is for the case where the device is found by Windows but VendorId/ProductId does not match,
                    // which may be caused by wrong VendorId/ProductId or USB driver that does not report correct device information.
                    Logger.Warn($"No devices found with VendorId: 0x{this.VendorId:X4} and ProductId: 0x{this.ProductId:X4}");

                    this.WindowsDevice = await Windows.Devices.Usb.UsbDevice.FromIdAsync(this.DeviceId);

                    if (this.WindowsDevice == null)
                    {
                        Logger.Info($"Device found using DeviceId: {this.DeviceId}, but VendorId/ProductId does not match.");
                        myDevices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync();
                        Windows.Devices.Enumeration.DeviceInformation? info = myDevices.FirstOrDefault(d => d.Name == this.DeviceName);

                        // Is device's info found?
                        if (info != null)
                        {
                            // Yes
                            Logger.Info($"Device with name {this.DeviceName} found, but VendorId/ProductId does not match. DeviceId: {info.Id}");
                            this.WindowsDevice = await Windows.Devices.Usb.UsbDevice.FromIdAsync(info.Id);
                        }
                        else
                        {
                            Logger.Error($"Device with name {this.DeviceName} not found. Failed to create WindowsDevice.");
                            return;
                        }
                    }
                    else
                    {
                        // No, use DeviceId directly to create WindowsDevice,
                        // which may throw an exception if device is not found or USB driver is missing.
                        Logger.Info($"Device with name {this.DeviceName} found using it's DeviceId: {this.DeviceId}");
                    }
                }
                else
                {
                    this.WindowsDevice = await Windows.Devices.Usb.UsbDevice.FromIdAsync(myDevices[0].Id);
                }

                if (this.WindowsDevice != null)
                {
                    Logger.Info($"Successfully created WindowsDevice from device with name {this.DeviceName}.");
                }
                else
                {
                    Logger.Error($"Failed to create WindowsDevice from device with name {this.DeviceName}.");
                    throw new InvalidOperationException($"Failed to create WindowsDevice from device with name {this.DeviceName}.");
                }

                // Send a control transfer. 

                Windows.Devices.Usb.UsbSetupPacket initSetupPacket = new Windows.Devices.Usb.UsbSetupPacket()
                {
                    Request = (byte)Windows.Devices.Usb.UsbControlTransferType.Vendor,
                    RequestType = new Windows.Devices.Usb.UsbControlRequestType()
                    {
                        Recipient = Windows.Devices.Usb.UsbControlRecipient.DefaultInterface,

                        ControlTransferType = Windows.Devices.Usb.UsbControlTransferType.Vendor
                    }
                };

                await WindowsDevice.SendControlOutTransferAsync(initSetupPacket);

                Logger.Info($"Initialized USB device with VendorId: {this.VendorId:X4} and ProductId: {this.ProductId:X4}.");
                Logger.Debug($"DefaultInterface.InterfaceNumber: {this.WindowsDevice.DefaultInterface.InterfaceNumber}.");
                Logger.Debug($"DefaultInterface.Descriptors: {this.WindowsDevice.DefaultInterface.Descriptors.Count}, DefaultInterface.InterfaceSettings: {this.WindowsDevice.DefaultInterface.InterfaceSettings.Count}.");
                Logger.Debug($"DeviceDescriptor.NumberOfConfigurations: {this.WindowsDevice.DeviceDescriptor.NumberOfConfigurations}.");
                Logger.Debug($"DefaultInterface.BulkOutPipes: {this.WindowsDevice.DefaultInterface.BulkOutPipes.Count}, DefaultInterface.BulkInPipes: {this.WindowsDevice.DefaultInterface.BulkInPipes.Count}.");
                Logger.Debug($"DefaultInterface.InterruptOutPipes: {this.WindowsDevice.DefaultInterface.InterruptOutPipes.Count}, DefaultInterface.InterruptInPipes: {this.WindowsDevice.DefaultInterface.InterruptInPipes.Count}.");
                Logger.Debug($"Configuration.UsbInterfaces: {this.WindowsDevice.Configuration.UsbInterfaces.Count}.");
                Logger.Debug($"Configuration.Descriptors: {this.WindowsDevice.Configuration.Descriptors.Count}.");
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Failed to create WindowsDevice for device with VendorId: 0x{this.VendorId:X4} and ProductId: 0x{this.ProductId:X4}. Exception: {ex}");
                throw;
            }

            return;
        }

        public virtual void Initialize()
        {
            this.InitializeAsync().Wait();
        }

        protected virtual async Task InitializeAsync()
        {
            await this.CreateWindowsDevicesAsync();
            ThrowIfNoWindowsDevice();

            // create Interface 0
            if (!CreateInterface0())
            {
                throw new InvalidOperationException("Failed to create interface 0. Device may not be usable.");
            }

            if (!CreateInterfaces())
            {
                throw new InvalidOperationException("Failed to create interfaces. Device may not be usable.");
            }
        }

        protected bool CreateInterface0()
        {
            ThrowIfNoWindowsDevice();
            Windows.Devices.Usb.UsbInterface intf = this.WindowsDevice.DefaultInterface;

            if (intf.InterfaceNumber != 0)
            {
                Logger.Warn($"Default interface number is {intf.InterfaceNumber}, expected 0. Skipping interface creation.");
                return false;
            }

            var outPipes = UsbInterfaceBase.CreateOutPipes(intf);
            var inPipes = UsbInterfaceBase.CreateInPipes(intf);

            if (inPipes.Any()  || outPipes.Any() )
            {
                var descriptor = intf.Descriptors.FirstOrDefault(e => (UsbDescriptorTypeEnum)e.DescriptorType == UsbDescriptorTypeEnum.USB_INTERFACE_DESCRIPTOR_TYPE);
                var usbInterfaceDescriptor = Windows.Devices.Usb.UsbInterfaceDescriptor.Parse(descriptor);

                var usbInterfaceControl = new UsbInterfaceControl(usbInterfaceDescriptor, outPipes, inPipes);
                ImplInterfaces.Add(intf.InterfaceNumber, usbInterfaceControl);
                return true;
            }
            else
            {
                Logger.Error($"Interface 0 has no pipes. Skipping interface creation.");
                return false;
            }
        }


        protected bool CreateInterfaces()
        {
            ThrowIfNoWindowsDevice();
            Windows.Devices.Usb.UsbConfiguration uc = this.WindowsDevice.Configuration;

            Logger.Debug($"Device use configuration {uc.ConfigurationDescriptor.ConfigurationValue}.");
            Logger.Debug($"Device has {uc.UsbInterfaces.Count} interfaces and {uc.Descriptors.Count} configuration descriptors.");

            if (!uc.UsbInterfaces.Any())
            {
                Logger.Warn($"Device has no interfaces. Skipping interface creation.");
                return true;
            }

            foreach (Windows.Devices.Usb.UsbInterface item in uc.UsbInterfaces)
            {
                Logger.Debug($"Interface {item.InterfaceNumber} has {item.InterfaceSettings.Count} settings and {item.Descriptors.Count} descriptors.");
                Logger.Debug($"Interface {item.InterfaceNumber} has {item.BulkInPipes.Count} bulk in pipes and {item.BulkOutPipes.Count} bulk out pipes.");
                Logger.Debug($"Interface {item.InterfaceNumber} has {item.InterruptInPipes.Count} interrupt in pipes and {item.InterruptOutPipes.Count} interrupt out pipes.");

                if (item.InterfaceNumber == 0)
                {
                    Logger.Warn($"Interface 0 already created in CreateInterface0(). Skipping interface creation.");
                }
                else
                {
                    var inPipes = UsbInterfaceBase.CreateInPipes(item);
                    var outPipes = UsbInterfaceBase.CreateOutPipes(item);
                    var descriptor = item.Descriptors.FirstOrDefault(e => (UsbDescriptorTypeEnum)e.DescriptorType == UsbDescriptorTypeEnum.USB_ENDPOINT_DESCRIPTOR_TYPE);
                    var usbInterfaceDescriptor = Windows.Devices.Usb.UsbInterfaceDescriptor.Parse(descriptor);

                    UsbInterfaceBase? usbInterface = CreateInterface(usbInterfaceDescriptor, outPipes, inPipes);
                    if (usbInterface != null)
                    {
                        this.ImplInterfaces.Add(item.InterfaceNumber, usbInterface);
                    }
                    else
                    {
                        Logger.Warn($"Failed to create interface {item.InterfaceNumber}. Skipping interface creation.");
                    }
                }

            }

            return true;
        }

        protected void ThrowIfNoWindowsDevice()
        {
            if (this.WindowsDevice == null)
            {
                throw new InvalidOperationException("WindowsDevice is not initialized. Call InitializeAsync() first.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.WindowsDevice != null)
                    {
                        this.WindowsDevice.Dispose();
                        this.WindowsDevice = null;
                    }

                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UsbDeviceBase()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
