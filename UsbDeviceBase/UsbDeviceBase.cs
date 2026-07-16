#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8618, CS8603, CS8604, CA1416
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using MemoryPack;
using Windows.Devices.Enumeration;
using Windows.Devices.Usb;

using Windows.Foundation.Metadata;
using Windows.Storage.Streams;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UsbDeviceBase
{
    public abstract class UsbDeviceBase : IDisposable, IUsbDevice, IUSBControlDevice
    {


        #region Constructors 

        protected UsbDeviceBase()
        {
            this.Logger = LogManager.GetLogger(this.GetType());
            IsConnected = false;
        }

        #endregion


        #region Fields 

        private bool disposedValue;
        private DeviceWatcher deviceWatcher;

        #endregion


        #region Properties 

        public bool IsConnected { get; private set; }

        public UsbInterfaceBase UsbInterfaceControl
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

        public Dictionary<int, UsbInterfaceBase> ImplInterfaces { get; } = new Dictionary<int, UsbInterfaceBase>();

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
        public bool IsOpenned { get; private set; }

        #endregion


        #region Public Methods 

        public BulkInPipeImpl GetBulkInPipe(int interfaceNumber, int pipeId)
        {
            return this.GetPipeIn<BulkInPipeImpl>(interfaceNumber, pipeId);
        }

        public BulkOutPipeImpl GetBulkOutPipe(int OutterfaceNumber, int pipeId)
        {
            return this.GetPipeOut<BulkOutPipeImpl>(OutterfaceNumber, pipeId);
        }


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
            where T : struct
        {
            try
            {
                this.ThrowIfNoWindowsDevice();
                Task<IBuffer> t = SendControlInTransferAsync(usp, bufferLength);
                t.Wait();
                IBuffer buffer = t.Result;
                byte[] data = buffer.ToArray();

                if (data == null || data.Length == 0)
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


        private UsbInterfaceBase GetUsbInterfaceControl()
        {
            ThrowIfNoWindowsDevice();
            if (!ImplInterfaces.ContainsKey(0))
            {
                throw new InvalidOperationException($"Interface 0 is not initialized. Call InitializeAsync() first.");
            }

            return this.ImplInterfaces[0];
        }

        protected async Task<bool> CreateWindowsDevicesAsync()
        {
            try
            {
                string aqs = Windows.Devices.Usb.UsbDevice.GetDeviceSelector(this.VendorId, this.ProductId);
                Logger.Debug($"Searching for USB device with VendorId: {this.VendorId} and ProductId: {this.ProductId}. \n  AQS: {aqs}");

                var myDevices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(aqs);

                // Windows cannot find the device, which may be caused by wrong VendorId/ProductId or missing USB driver.
                // In this case, we should not throw an exception, but log an error.
                if (myDevices.Count == 0)
                {
                    // try using DeviceId to find the device.

                    // This is for the case where the device is found by Windows but VendorId/ProductId does not match,
                    // which may be caused by wrong VendorId/ProductId or USB driver that does not report correct device information.
                    Logger.Warn($"No devices found with VendorId: 0x{this.VendorId:X4} and ProductId: 0x{this.ProductId:X4}");

                    var allmyDevices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(DeviceClass.All);
                    var found = allmyDevices.FirstOrDefault(d => d.Name == this.DeviceName);

                    if (found != null)
                    {
                        aqs = Windows.Devices.Usb.UsbDevice.GetDeviceSelector((Guid)found.Properties["System.Devices.ContainerId"]);
                        SetupWatcher(aqs);

                        // yes, found the device using DeviceId, but VendorId/ProductId does not match.
                        this.WindowsDevice = await Windows.Devices.Usb.UsbDevice.FromIdAsync(found.Id);

                        if (this.WindowsDevice == null)
                        {
                            // is device found using DeviceId but VendorId/ProductId does not match?
                            Logger.Error($"Device not found using DeviceId: {found.Id}.");
                            return false;
                        }
                    }
                    else
                    {
                        // no device found using DeviceId, which may be caused by missing USB driver or the device is in an unusable state.
                        Logger.Error($"Device not found using DeviceId: {this.DeviceName}.");
                        return false;
                    }
                }
                else
                {
                    SetupWatcher(aqs);
                    this.WindowsDevice = await Windows.Devices.Usb.UsbDevice.FromIdAsync(myDevices[0].Id);
                    if (this.WindowsDevice != null)
                    {
                        Logger.Info($"Successfully created WindowsDevice from device with name {this.DeviceName}.");
                    }
                    else
                    {
                        Logger.Error($"Failed to create WindowsDevice from device with name {this.DeviceName}.");
                        throw new InvalidOperationException($"Failed to create WindowsDevice from device with name {this.DeviceName}.");
                    }
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

                ////await WindowsDevice.SendControlOutTransferAsync(initSetupPacket);

                Logger.Info($"Initialized USB device with VendorId: {this.VendorId:X4} and ProductId: {this.ProductId:X4}.");
                //Logger.Debug($"DefaultInterface.InterfaceNumber: {this.WindowsDevice.DefaultInterface.InterfaceNumber}.");
                //Logger.Debug($"DefaultInterface.Descriptors: {this.WindowsDevice.DefaultInterface.Descriptors.Count}, DefaultInterface.InterfaceSettings: {this.WindowsDevice.DefaultInterface.InterfaceSettings.Count}.");
                //Logger.Debug($"DeviceDescriptor.NumberOfConfigurations: {this.WindowsDevice.DeviceDescriptor.NumberOfConfigurations}.");
                //Logger.Debug($"DefaultInterface.BulkOutPipes: {this.WindowsDevice.DefaultInterface.BulkOutPipes.Count}, DefaultInterface.BulkInPipes: {this.WindowsDevice.DefaultInterface.BulkInPipes.Count}.");
                //Logger.Debug($"DefaultInterface.InterruptOutPipes: {this.WindowsDevice.DefaultInterface.InterruptOutPipes.Count}, DefaultInterface.InterruptInPipes: {this.WindowsDevice.DefaultInterface.InterruptInPipes.Count}.");
                //Logger.Debug($"Configuration.UsbInterfaces: {this.WindowsDevice.Configuration.UsbInterfaces.Count}.");
                //Logger.Debug($"Configuration.Descriptors: {this.WindowsDevice.Configuration.Descriptors.Count}.");
                DumpDeviceInfo();
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                if (ex.HResult == unchecked((int)0x8007001F)) // ERROR_GEN_FAILURE
                {
                    Logger.Fatal($"COMException the device attached to the system is not functioning. This may be caused by missing USB driver or the device is in an unusable state.", ex);
                }
                else
                {
                    Logger.Fatal($"COMException failed to create WindowsDevice for device with VendorId: 0x{this.VendorId:X4}, ProductId: 0x{this.ProductId:X4}.", ex);
                }

                Logger.Fatal($"COMException the device attached to the system is not functioning.", ex);
                return false;
            }
            catch (FileNotFoundException ex)
            {
                Logger.Fatal($"Device not found with VendorId: 0x{this.VendorId:X4}, ProductId: 0x{this.ProductId:X4}.", ex);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Logger.Fatal($"Failed to create WindowsDevice for device with VendorId: 0x{this.VendorId:X4}, ProductId: 0x{this.ProductId:X4}.", ex);
                return false;
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Failed to create WindowsDevice for device with VendorId: 0x{this.VendorId:X4}, ProductId: 0x{this.ProductId:X4}.", ex);
                return false;
            }

            return true;
        }


        public virtual bool Initialize()
        {
            return InitializeAsync().GetAwaiter().GetResult();
        }

        protected virtual async Task<bool> InitializeAsync()
        {
            if(IsOpenned)
            {
                Logger.Error($"Device is already initialized. Call Dispose() before initializing again.");
                throw new InvalidOperationException("Device is already initialized. Call Dispose() before initializing again.");
            }

            IsOpenned = await this.CreateWindowsDevicesAsync();
            if (!IsOpenned)
            {
                return false;
            }

            ThrowIfNoWindowsDevice();

            if (!CreateInterfaces())
            {
                throw new InvalidOperationException("Failed to create interfaces. Device may not be usable.");
            }

            IsConnected = true;
            return true;
        }

        private void SetupWatcher(string aqs)
        {
            deviceWatcher = DeviceInformation.CreateWatcher(aqs);
            deviceWatcher.Added += Watcher_Added;
            deviceWatcher.Removed += Watcher_Removed;
            deviceWatcher.Updated += Watcher_Updated;
            deviceWatcher.EnumerationCompleted += Watcher_EnumerationCompleted;
            deviceWatcher.Stopped += Watcher_Stopped;
            deviceWatcher.Start();
        }

        private void Watcher_Stopped(DeviceWatcher sender, object args)
        {
            Logger.Debug($"Device Watcher_Stopped:");
        }

        private void Watcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            Logger.Debug($"Device EnumerationCompleted:");
        }

        private void Watcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            Logger.Debug($"Device updated: {args.Id}");
            IsConnected = false;
        }

        private void Watcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            Logger.Debug($"Device removed: {args.Id}");
            IsConnected = false;
        }

        private void Watcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            Logger.Debug($"Device added: {args.Id}, Name: {args.Name}, IsEnabled: {args.IsEnabled}");
            IsConnected = args.IsEnabled;
        }

        protected bool CreateInterfaces()
        {
            ThrowIfNoWindowsDevice();
            Windows.Devices.Usb.UsbConfiguration uc = this.WindowsDevice.Configuration;

            //Logger.Debug($"Device use configuration {uc.ConfigurationDescriptor.ConfigurationValue}.");
            //Logger.Debug($"Device has {uc.UsbInterfaces.Count} interfaces and {uc.Descriptors.Count} configuration descriptors.");

            if (!uc.UsbInterfaces.Any())
            {
                Logger.Warn($"Device has no interfaces. Skipping interface creation.");
                return true;
            }

            foreach (Windows.Devices.Usb.UsbInterface item in uc.UsbInterfaces)
            {
                //Logger.Debug($"Interface {item.InterfaceNumber} has {item.InterfaceSettings.Count} settings and {item.Descriptors.Count} descriptors.");
                //Logger.Debug($"Interface {item.InterfaceNumber} has {item.BulkInPipes.Count} bulk IN pipes and {item.BulkOutPipes.Count} bulk OUT pipes.");
                //Logger.Debug($"Interface {item.InterfaceNumber} has {item.InterruptInPipes.Count} interrupt IN pipes and {item.InterruptOutPipes.Count} interrupt OUT pipes.");

                if (this.ImplInterfaces.ContainsKey(item.InterfaceNumber))
                {
                    Logger.Warn($"Interface {item.InterfaceNumber} already created.");
                }
                else
                {
                    var inPipes = UsbInterfaceBase.CreateInPipes(item);
                    var outPipes = UsbInterfaceBase.CreateOutPipes(item);
                    UsbInterfaceBase? usbInterface = null;
                    var descriptor = item.Descriptors.FirstOrDefault(e => (UsbDescriptorTypeEnum)e.DescriptorType == UsbDescriptorTypeEnum.USB_INTERFACE_DESCRIPTOR_TYPE);

                    if (descriptor != null)
                    {
                        UsbInterfaceDescriptor uid = UsbInterfaceDescriptor.Parse(descriptor);
                        usbInterface = CreateInterface(uid, outPipes, inPipes);
                    }
                    else
                    {
                        Logger.Error($"Interface {item.InterfaceNumber} has no interface descriptor. ");
                        throw new InvalidOperationException($"Interface {item.InterfaceNumber} has no interface descriptor.");
                    }

                    if (usbInterface != null)
                    {
                        if (!this.ImplInterfaces.ContainsKey(item.InterfaceNumber))
                        {
                            this.ImplInterfaces.Add(item.InterfaceNumber, usbInterface);
                            Logger.Info($"Interface {item.InterfaceNumber} has been created with {inPipes.Count()} In pipes and {outPipes.Count()} Out pipes.");
                        }
                        else
                        {
                            Logger.Warn($"Interface {item.InterfaceNumber} already exists.");
                        }
                    }
                    else
                    {
                        Logger.Error($"Failed to create interface {item.InterfaceNumber}.");
                        throw new InvalidOperationException($"Failed to create interface {item.InterfaceNumber}.");
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
                    if(this.deviceWatcher != null)
                    {
                        this.deviceWatcher.Added -= Watcher_Added;
                        this.deviceWatcher.Removed -= Watcher_Removed;
                        this.deviceWatcher.Updated -= Watcher_Updated;
                        this.deviceWatcher.EnumerationCompleted -= Watcher_EnumerationCompleted;
                        this.deviceWatcher.Stopped -= Watcher_Stopped;
                     
                        if (this.deviceWatcher.Status == DeviceWatcherStatus.Started || this.deviceWatcher.Status == DeviceWatcherStatus.EnumerationCompleted)
                        {
                            this.deviceWatcher.Stop();
                        }

                        this.deviceWatcher = null;
                    }
                    if (this.WindowsDevice != null)
                    {
                        this.WindowsDevice.Dispose();
                        this.WindowsDevice = null;
                    }

                    // dispose managed state (managed objects)
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        private void DumpDeviceInfo()
        {
            StringBuilder sb = new StringBuilder();

            UsbDeviceDescriptor DeviceDescriptor = this.WindowsDevice.DeviceDescriptor;
            sb.AppendLine($"Device Descriptor:");
            sb.AppendLine($"  Bcd Device Revision: {DeviceDescriptor.BcdDeviceRevision}, " +
                $"Bcd Usb: {DeviceDescriptor.BcdUsb}, " +
                $"MaxPacketSize0: {DeviceDescriptor.MaxPacketSize0} " +
                $"VendorId: 0x{DeviceDescriptor.VendorId:X4}, " +
                $"ProductId: 0x{DeviceDescriptor.ProductId:X4}, " +
                $"NumberOfConfigurations: {DeviceDescriptor.NumberOfConfigurations}.");

            sb.AppendLine($"Default Interface:");
            Dump(sb, this.WindowsDevice.DefaultInterface);
            Dump(sb, this.WindowsDevice.Configuration);

            Logger.Debug(sb.ToString());
        }

        private void Dump(StringBuilder sb, UsbConfiguration configuration)
        {
            sb.AppendLine($"Configuration:");
            Dump(sb, configuration.ConfigurationDescriptor);

            sb.AppendLine($"  UsbInterfaces: {configuration.UsbInterfaces.Count}, Descriptors: {configuration.Descriptors.Count}");

            foreach (Windows.Devices.Usb.UsbInterface intf in configuration.UsbInterfaces)
            {
                if (intf.InterfaceNumber == 0)
                {
                    sb.AppendLine($"Interface 0: already dumped in Default Interface. Skipping interface dump.");
                    continue;
                }
                Dump(sb, intf);
            }
        }

        private void Dump(StringBuilder sb, Windows.Devices.Usb.UsbInterface intf)
        {
            sb.AppendLine($"Interface {intf.InterfaceNumber}:");
            sb.AppendLine($"  Interface Settings count: {intf.InterfaceSettings.Count}");
            sb.AppendLine($"  Bulk IN pipes count: {intf.BulkInPipes.Count}, Bulk OUT pipes count: {intf.BulkOutPipes.Count}");
            sb.AppendLine($"  Interrupt IN pipes count: {intf.InterruptInPipes.Count}, Interrupt OUT pipes count: {intf.InterruptOutPipes.Count}");

            foreach (Windows.Devices.Usb.UsbInterfaceSetting? setting in intf.InterfaceSettings)
            {
                Dump(sb, setting);
            }

        }

        private void Dump(StringBuilder sb, UsbInterfaceSetting setting)
        {
            foreach (UsbDescriptor? descriptor in setting.Descriptors)
            {
                Dump(sb, descriptor);
            }


            Dump(sb, setting.InterfaceDescriptor);

            foreach (UsbInterruptOutEndpointDescriptor? interruptOutEndpoint in setting.InterruptOutEndpoints)
            {
                Dump(sb, interruptOutEndpoint);
            }

            foreach (UsbInterruptInEndpointDescriptor? interruptInEndpoint in setting.InterruptInEndpoints)
            {
                Dump(sb, interruptInEndpoint);
            }

            foreach (UsbBulkOutEndpointDescriptor? bulkOutEndpoint in setting.BulkOutEndpoints)
            {
                Dump(sb, bulkOutEndpoint);
            }

            foreach (UsbBulkInEndpointDescriptor? bulkInEndpoint in setting.BulkInEndpoints)
            {
                Dump(sb, bulkInEndpoint);
            }

        }

        private void Dump(StringBuilder sb, UsbBulkInEndpointDescriptor bulkInEndpoint)
        {
            if (bulkInEndpoint == null)
                return;

            sb.AppendLine($"  Bulk IN Endpoint decriptor:");
            sb.AppendLine($"    Endpoint number: 0x{bulkInEndpoint.EndpointNumber:X2}, " +
                $"MaxPacketSize: {bulkInEndpoint.MaxPacketSize}");
        }

        private void Dump(StringBuilder sb, UsbBulkOutEndpointDescriptor bulkOutEndpoint)
        {
            if (bulkOutEndpoint == null)
                return;

            sb.AppendLine($"  Bulk OUT Endpoint decriptor:");
            sb.AppendLine($"    Endpoint number: 0x{bulkOutEndpoint.EndpointNumber:X2}, " +
                $"MaxPacketSize: {bulkOutEndpoint.MaxPacketSize}");
        }


        private void Dump(StringBuilder sb, UsbInterruptInEndpointDescriptor interruptInEndpoint)
        {
            if (interruptInEndpoint == null)
                return;

            sb.AppendLine($"  Interrupt IN Endpoint decriptor:");
            sb.AppendLine($"    Endpoint number: 0x{interruptInEndpoint.EndpointNumber:X2}, " +
                $"Interval: {interruptInEndpoint.Interval}" +
                $"MaxPacketSize: {interruptInEndpoint.MaxPacketSize}");
        }

        private void Dump(StringBuilder sb, UsbInterruptOutEndpointDescriptor? interruptOutEndpoint)
        {
            if (interruptOutEndpoint == null)
                return;

            sb.AppendLine($"  Interrupt OUT Endpoint decriptor:");
            sb.AppendLine($"    Endpoint number: 0x{interruptOutEndpoint.EndpointNumber:X2}, " +
                $"Interval: {interruptOutEndpoint.Interval}" +
                $"MaxPacketSize: {interruptOutEndpoint.MaxPacketSize}");
        }

        private void Dump(StringBuilder sb, UsbInterfaceDescriptor interfaceDescriptor)
        {
            sb.AppendLine($"  Interface Descriptor:");
            sb.AppendLine($"    Interface Number: {interfaceDescriptor.InterfaceNumber}, Alternate Setting Number: {interfaceDescriptor.AlternateSettingNumber}, Interface Class: {interfaceDescriptor.ClassCode}, Interface SubClass: {interfaceDescriptor.SubclassCode}, Interface Protocol: {interfaceDescriptor.ProtocolCode}");
        }

        private void Dump(StringBuilder sb, UsbDescriptor descriptor)
        {
            switch ((UsbDescriptorTypeEnum)descriptor.DescriptorType)
            {
                case UsbDescriptorTypeEnum.USB_CONFIGURATION_DESCRIPTOR_TYPE:
                case UsbDescriptorTypeEnum.USB_INTERFACE_DESCRIPTOR_TYPE:
                    break;
                case UsbDescriptorTypeEnum.USB_ENDPOINT_DESCRIPTOR_TYPE:
                    var ued = Windows.Devices.Usb.UsbEndpointDescriptor.Parse(descriptor);
                    Dump(sb, ued);
                    // dump elsewhere, skip here to avoid duplicate dump.
                    break;
                default:
                    sb.AppendLine($"      Unknown descriptor type: {descriptor.DescriptorType}");
                    break;
            }
        }

        private void Dump(StringBuilder sb, UsbConfigurationDescriptor ucd)
        {
            sb.AppendLine($"  Configuration Descriptor:");
            sb.AppendLine($"    Configuration Value: {ucd.ConfigurationValue}, Max Power Milliamps: {ucd.MaxPowerMilliamps}, Remote Wakeup: {ucd.RemoteWakeup}, Self Powered: {ucd.SelfPowered}");
        }

        private void Dump(StringBuilder sb, UsbEndpointDescriptor ued)
        {
            switch (ued.EndpointType)
            {
                case UsbEndpointType.Bulk:
                    if (ued.Direction == UsbTransferDirection.In)
                    {
                        Dump(sb, ued.AsBulkInEndpointDescriptor);
                    }
                    else
                    {
                        Dump(sb, ued.AsBulkOutEndpointDescriptor);
                    }
                    break;
                case UsbEndpointType.Interrupt:
                    if (ued.Direction == UsbTransferDirection.In)
                    {
                        Dump(sb, ued.AsInterruptInEndpointDescriptor);
                    }
                    else
                    {
                        Dump(sb, ued.AsInterruptOutEndpointDescriptor);
                    }
                    break;
                default:
                    sb.AppendLine($"      Unknown endpoint type: {ued.EndpointType}");
                    break;
            }
        }

        private T GetPipeIn<T>(int interfaceNumber, int pipeId)
            where T : PipeInBase
        {
            try
            {
                this.ThrowIfNoWindowsDevice();

                if (!ImplInterfaces.ContainsKey(interfaceNumber))
                {
                    throw new InvalidOperationException($"Interface {interfaceNumber} is not initialized. Call InitializeAsync() first.");
                }

                KeyValuePair<int, PipeInBase> pipe = ImplInterfaces[interfaceNumber].InPipes.SingleOrDefault(p => p.Key == pipeId && p.GetType() == typeof(T));
                if (pipe.Equals(default(KeyValuePair<int, PipeInBase>)))
                {
                    Logger.Error($"BulkInPipe with pipe {pipeId}{typeof(T)} is not found in interface {interfaceNumber}.");
                    return null;
                }

                return (T)pipe.Value;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to get BulkInPipe. Exception: {ex}");
                throw;
            }
        }

        private T GetPipeOut<T>(int interfaceNumber, int pipeId)
            where T : PipeOutBase
        {
            try
            {
                this.ThrowIfNoWindowsDevice();

                if (!ImplInterfaces.ContainsKey(interfaceNumber))
                {
                    throw new InvalidOperationException($"Interface {interfaceNumber} is not initialized. Call InitializeAsync() first.");
                }

                KeyValuePair<int, PipeOutBase> pipe = ImplInterfaces[interfaceNumber].OutPipes.SingleOrDefault(p => p.Key == pipeId && p.GetType() == typeof(T));
                if (pipe.Equals(default(KeyValuePair<int, PipeOutBase>)))
                {
                    Logger.Error($"BulkInPipe with pipe {pipeId}{typeof(T)} is not found in interface {interfaceNumber}.");
                    return null;
                }

                return (T)pipe.Value;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to get BulkInPipe. Exception: {ex}");
                throw;
            }
        }

        #endregion

        #region Private Classes / Enum 

        #endregion
    }
}
