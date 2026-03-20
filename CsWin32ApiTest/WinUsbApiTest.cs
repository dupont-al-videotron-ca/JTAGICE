#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8603, CS8604, CS8602
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using CsWin32Api;
using log4net.Core;
using Xunit;

namespace CsWin32ApiTest
{
    // The ATU90USBKEY device must be connected to the system before running these tests.
    public class WinUsbApiTest : WinUsbApiBaseTest
    {
        public WinUsbApiTest() : base()
        {

        }

        [Fact]
        public void InitializeUsbHandle_shall_return_a_valid_WinUsbDeviceApi()
        {
            var connectionPath = WinUsbApi.FindDevicePathForUsbDevice(deviceDescription);
            Assert.True(!String.IsNullOrWhiteSpace(connectionPath));

            this._fileHandle = CsWin32Api.WinUsbApi.CreateFileHandle(connectionPath);
            Assert.NotNull(_fileHandle);
            Assert.False(_fileHandle.IsInvalid);

            WinUsbDeviceApi result = WinUsbApi.WinUsbDeviceApiFactory(_fileHandle);
            this._winUsbDevice = result;
            Assert.NotNull(this._winUsbDevice);
        }

        [Fact]
        public void InitializeUsbHandle_shall_return_null_WinUsbDeviceApi()
        {
            var connectionPath = WinUsbApi.FindDevicePathForUsbDevice(deviceDescription);
            Assert.True(!String.IsNullOrWhiteSpace(connectionPath));

            this._fileHandle = CsWin32Api.WinUsbApi.CreateFileHandle(connectionPath);
            Assert.NotNull(_fileHandle);
            Assert.False(_fileHandle.IsInvalid);

            _fileHandle.Dispose();
            WinUsbDeviceApi result = WinUsbApi.WinUsbDeviceApiFactory(_fileHandle);
            Assert.Null(result);

            this.VerifyLogForError = false;
            var logevent = this.LoggedEvents.FirstOrDefault(e => e.Level == Level.Error);
            Assert.Contains("Exception in WinUsbDeviceApiFactory: System.ObjectDisposedException: Cannot access a disposed object.", logevent?.RenderedMessage);
            Assert.NotNull(logevent);

        }

        [Theory]
        [InlineData(0)]
        //[InlineData(1)]
        //[InlineData(2)]
        public void GetAssociatedInterface_shall_return_true_and_interface_handle(byte interfaceIndex)
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);

            bool result = this._winUsbDevice.GetAssociatedInterface(interfaceIndex, out SafeHandle interfaceHandle);
            Assert.True(result);
            Assert.NotNull(interfaceHandle);
        }

        [Fact]
        public void Close_shall_close_device_handle()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);

            this._winUsbDevice.Close();
            Assert.True(this._winUsbDevice.IsClosed);

        }

        [Fact]
        public void Close_shall_failed_after_second_close()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            this._winUsbDevice.Close();
            Assert.True(this._winUsbDevice.IsClosed);
            Assert.Throws<ObjectDisposedException>(() => this._winUsbDevice.Close());
        }
        [Fact]
        public void QueryDeviceInformation_shall_return_the_speed()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            UsbDeviceSpeedEnum result = this._winUsbDevice.QueryDeviceInformation();
            Assert.NotEqual(UsbDeviceSpeedEnum.UsbUndefinedSpeed, result);
        }
        [Fact]
        public void QueryDeviceInformation_shall_failed()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            this._winUsbDevice.Close();
            Assert.Throws<ObjectDisposedException>(() => this._winUsbDevice.QueryDeviceInformation());
        }

        [Fact]
        public void QueryInterfaceSettings_shall_return_interface_settings()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            bool result = this._winUsbDevice.QueryInterfaceSettings(0, out UsbInterfaceDescriptor descriptor);
            Assert.True(result);
            Assert.Equal(9, descriptor.Length);
            Assert.Equal(4, descriptor.DescriptorType);
            Assert.Equal(0, descriptor.InterfaceNumber);
            Assert.Equal(0, descriptor.AlternateSetting);
            Assert.Equal(3, descriptor.NumEndpoints);
            Assert.Equal(0xff, descriptor.InterfaceClass);
            Assert.Equal(0xff, descriptor.InterfaceSubClass);
            Assert.Equal(0xff, descriptor.InterfaceProtocol);
            Assert.Equal(0, descriptor.Interface);
        }

        [Fact]
        public void QueryInterfaceSettings_shall_failed_with_bad_number()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            bool result = this._winUsbDevice.QueryInterfaceSettings(1, out UsbInterfaceDescriptor descriptor);
            Assert.False(result);
            Assert.Equal(0, descriptor.Length);
            Assert.Equal(0, descriptor.DescriptorType);
            Assert.Equal(0, descriptor.InterfaceNumber);
            Assert.Equal(0, descriptor.AlternateSetting);
            Assert.Equal(0, descriptor.NumEndpoints);
            Assert.Equal(0x00, descriptor.InterfaceClass);
            Assert.Equal(0x00, descriptor.InterfaceSubClass);
            Assert.Equal(0x00, descriptor.InterfaceProtocol);
            Assert.Equal(0, descriptor.Interface);

            this.VerifyLogForError = false;
            var logevent = this.LoggedEvents.FirstOrDefault(e => e.Level == Level.Error);
            Assert.NotNull(logevent);
            Assert.Contains("No more data is available.", logevent?.RenderedMessage);
        }

        [Fact]
        public void QueryPipe_shall_return_usbPipeInformation_from_pipe_0()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            var result = this._winUsbDevice.QueryPipe(0, 0, out UsbPipeInformation usbPipeInformation);
            Assert.True(result);
            Assert.NotNull(usbPipeInformation);
            Assert.Equal(0x01, usbPipeInformation.EndpointNumber);
            Assert.Equal(0x00, usbPipeInformation.Interval);
            Assert.True(usbPipeInformation.IsEndpointIn);
            Assert.False(usbPipeInformation.IsEndpointOut);
            Assert.Equal(0x0008, usbPipeInformation.MaximumPacketSize);
            Assert.Equal(0x81, usbPipeInformation.PipeId);
            Assert.Equal(USBPipeTypeEnum.UsbdPipeTypeBulk, usbPipeInformation.PipeType);

        }
        [Fact]
        public void QueryPipe_shall_return_usbPipeInformationfrom_pipe_1()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            var result = this._winUsbDevice.QueryPipe(0, 1, out UsbPipeInformation usbPipeInformation);
            Assert.True(result);
            Assert.NotNull(usbPipeInformation);
            Assert.Equal(0x02, usbPipeInformation.EndpointNumber);
            Assert.Equal(0x00, usbPipeInformation.Interval);
            Assert.True(usbPipeInformation.IsEndpointIn);
            Assert.False(usbPipeInformation.IsEndpointOut);
            Assert.Equal(0x0040, usbPipeInformation.MaximumPacketSize);
            Assert.Equal(0x82, usbPipeInformation.PipeId);
            Assert.Equal(USBPipeTypeEnum.UsbdPipeTypeBulk, usbPipeInformation.PipeType);
        }

        [Fact]
        public void QueryPipe_shall_return_usbPipeInformationfrom_pipe_2()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            var result = this._winUsbDevice.QueryPipe(0, 2, out UsbPipeInformation usbPipeInformation);
            Assert.True(result);
            Assert.NotNull(usbPipeInformation);
            Assert.Equal(0x03, usbPipeInformation.EndpointNumber);
            Assert.Equal(0x00, usbPipeInformation.Interval);
            Assert.False(usbPipeInformation.IsEndpointIn);
            Assert.True(usbPipeInformation.IsEndpointOut);
            Assert.Equal(0x0008, usbPipeInformation.MaximumPacketSize);
            Assert.Equal(0x03, usbPipeInformation.PipeId);
            Assert.Equal(USBPipeTypeEnum.UsbdPipeTypeBulk, usbPipeInformation.PipeType);
        }

        [Fact]
        public void QueryPipe_shall_failed_with_pipe_3()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            var result = this._winUsbDevice.QueryPipe(0, 3, out UsbPipeInformation usbPipeInformation);
            Assert.False(result);
            Assert.NotNull(usbPipeInformation);
            Assert.Equal(0x00, usbPipeInformation.EndpointNumber);
            Assert.Equal(0x00, usbPipeInformation.Interval);
            Assert.False(usbPipeInformation.IsEndpointIn);
            Assert.True(usbPipeInformation.IsEndpointOut);
            Assert.Equal(0x0000, usbPipeInformation.MaximumPacketSize);
            Assert.Equal(0x00, usbPipeInformation.PipeId);
            Assert.Equal(USBPipeTypeEnum.UsbdPipeTypeControl, usbPipeInformation.PipeType);

            this.VerifyLogForError = false;
            var logevent = this.LoggedEvents.FirstOrDefault(e => e.Level == Level.Error);
            Assert.NotNull(logevent);
            Assert.Contains("No more data is available.", logevent?.RenderedMessage);

        }

        [Theory]
        [InlineData(0, USBPipePolicyEnum.PipeTransferTimeout, true, 5000)]
        [InlineData(0, USBPipePolicyEnum.ResetPipeOnResume, false, 0)]
        [InlineData(0, USBPipePolicyEnum.AllowPartialReads, false, 0)]
        [InlineData(0, USBPipePolicyEnum.MaximumTransferSize, true, 0)]
        [InlineData(0, USBPipePolicyEnum.RawIo, false, 0)]
        [InlineData(0, USBPipePolicyEnum.ShortPacketTerminate, false, 0)]
        [InlineData(1, USBPipePolicyEnum.PipeTransferTimeout, false, 5000)]
        [InlineData(1, USBPipePolicyEnum.ResetPipeOnResume, false, 0)]
        [InlineData(1, USBPipePolicyEnum.AllowPartialReads, false, 0)]
        [InlineData(1, USBPipePolicyEnum.MaximumTransferSize, false, 0)]
        [InlineData(1, USBPipePolicyEnum.RawIo, false, 0)]
        [InlineData(1, USBPipePolicyEnum.ShortPacketTerminate, false, 0)]
        [InlineData(2, USBPipePolicyEnum.PipeTransferTimeout, false, 5000)]
        [InlineData(2, USBPipePolicyEnum.ResetPipeOnResume, false, 0)]
        [InlineData(2, USBPipePolicyEnum.AllowPartialReads, false, 0)]
        [InlineData(2, USBPipePolicyEnum.MaximumTransferSize, false, 0)]
        [InlineData(2, USBPipePolicyEnum.RawIo, false, 0)]
        [InlineData(2, USBPipePolicyEnum.ShortPacketTerminate, false, 0)]
        public void GetPipePolicy_shall_return_pipe_policy(byte pipeId, USBPipePolicyEnum testPolicy, bool expectedResult, UInt64 expectedData)
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            UInt64 data = 0;
            var result = this._winUsbDevice.WinUsbPipeApiFactory(pipeId).GetPipePolicy(testPolicy, ref data);
            Assert.Equal(expectedResult, result);

            if (result)
            {
                Logger.Info($"GetPipePolicy for {testPolicy} returned data: {data}");
                Assert.Equal(expectedData, data);
            }
            else
            {
                this.VerifyLogForError = false;
                var logevent = this.LoggedEvents.FirstOrDefault(e => e.Level == Level.Error);
                Assert.NotNull(logevent);
                Assert.Contains("The parameter is incorrect.", logevent?.RenderedMessage);
            }
        }

        [Theory]
        [InlineData(0, USBPipePolicyEnum.PipeTransferTimeout, true, 5000)]
        [InlineData(0, USBPipePolicyEnum.ResetPipeOnResume, false, 0)]
        [InlineData(0, USBPipePolicyEnum.AllowPartialReads, false, 0)]
        [InlineData(0, USBPipePolicyEnum.MaximumTransferSize, false, 0)]
        [InlineData(0, USBPipePolicyEnum.RawIo, false, 0)]
        [InlineData(0, USBPipePolicyEnum.ShortPacketTerminate, false, 0)]
        [InlineData(1, USBPipePolicyEnum.PipeTransferTimeout, false, 5000)]
        [InlineData(1, USBPipePolicyEnum.ResetPipeOnResume, false, 0)]
        [InlineData(1, USBPipePolicyEnum.AllowPartialReads, false, 0)]
        [InlineData(1, USBPipePolicyEnum.MaximumTransferSize, false, 0)]
        [InlineData(1, USBPipePolicyEnum.RawIo, false, 0)]
        [InlineData(1, USBPipePolicyEnum.ShortPacketTerminate, false, 0)]
        [InlineData(2, USBPipePolicyEnum.PipeTransferTimeout, false, 5000)]
        [InlineData(2, USBPipePolicyEnum.ResetPipeOnResume, false, 0)]
        [InlineData(2, USBPipePolicyEnum.AllowPartialReads, false, 0)]
        [InlineData(2, USBPipePolicyEnum.MaximumTransferSize, false, 0)]
        [InlineData(2, USBPipePolicyEnum.RawIo, false, 0)]
        [InlineData(2, USBPipePolicyEnum.ShortPacketTerminate, false, 0)]
        public void SetPipePolicy_shall_return_pipe_policy(byte pipeId, USBPipePolicyEnum testPolicy, bool expectedResult, UInt64 expectedData)
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);

            var result = this._winUsbDevice.WinUsbPipeApiFactory(pipeId).SetPipePolicy(testPolicy, expectedData);
            Assert.Equal(expectedResult, result);

            if (!result)
            {
                this.VerifyLogForError = false;
                var logevent = this.LoggedEvents.FirstOrDefault(e => e.Level == Level.Error);
                Assert.NotNull(logevent);
                Assert.Contains("The parameter is incorrect.", logevent?.RenderedMessage);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AbortPipe_shall_return_true_for_valid_pipe(byte pipeId)
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            var result = this._winUsbDevice.WinUsbPipeApiFactory(pipeId).AbortPipe();
            Assert.True(result);
        }

        [Theory]
        [InlineData(3)]
        public void AbortPipe_shall_return_true_for_invalid_pipe(byte pipeId)
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            var result = this._winUsbDevice.WinUsbPipeApiFactory(pipeId).AbortPipe();
            Assert.False(result);

            this.VerifyLogForError = false;
            var logevent = this.LoggedEvents.FirstOrDefault(e => e.Level == Level.Error);
            Assert.NotNull(logevent);
            Assert.Contains("The parameter is incorrect.", logevent?.RenderedMessage);
        }

        [Fact]
        public void GetDeviceDescriptor_shall_return_a_valid_descriptor()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);

            bool result = this._winUsbDevice.GetDeviceDescriptor(out UsbDeviceDescriptor descriptor);
            Assert.True(result);

            Assert.Equal(18, descriptor.Length);
            Assert.Equal(UsbDescriptorTypeEnum.USB_DEVICE_DESCRIPTOR_TYPE, (UsbDescriptorTypeEnum)descriptor.DescriptorType);
            Assert.Equal(1, descriptor.Manufacturer);
            Assert.Equal(2, descriptor.Product);
            Assert.Equal(3, descriptor.SerialNumber);
            Assert.Equal(0xFF, descriptor.DeviceClass);
            Assert.Equal(0xFF, descriptor.DeviceSubClass);
            Assert.Equal(0xFF, descriptor.DeviceProtocol);
            Assert.Equal(0x08, descriptor.MaxPacketSize0);
            Assert.Equal(0x03eb, descriptor.idVendor);
            Assert.Equal(1, descriptor.idProduct);
            Assert.Equal(1, descriptor.NumConfigurations);
            Assert.Equal(0x0110, descriptor.bcdUSB);
            Assert.Equal(1, descriptor.bcdDevice);
        }

        [Fact]
        public void GetConfigurationDescriptor_shall_return_a_valid_descriptor()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);

            bool result = this._winUsbDevice.GetConfigurationDescriptor(0, out UsbConfigurationDescriptor descriptor);
            Assert.True(result);

            Assert.Equal(9, descriptor.Length);
            Assert.Equal(UsbDescriptorTypeEnum.USB_CONFIGURATION_DESCRIPTOR_TYPE, (UsbDescriptorTypeEnum)descriptor.DescriptorType);
            Assert.Equal(0, descriptor.Configuration);
            Assert.Equal(0x80, descriptor.Attributes);
            Assert.Equal(0x32, descriptor.MaxPower);
            Assert.Equal(01, descriptor.NumInterfaces);
            Assert.Equal(0x27, descriptor.TotalLength);

        }

        [Fact]
        public void GetInterfaceDescriptor_shall_return_a_valid_descriptor()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);

            bool result = this._winUsbDevice.GetInterfaceDescriptor(0, out UsbInterfaceDescriptor descriptor);
            Assert.True(result);

            Assert.Equal(9, descriptor.Length);
            Assert.Equal(UsbDescriptorTypeEnum.USB_INTERFACE_DESCRIPTOR_TYPE, (UsbDescriptorTypeEnum)descriptor.DescriptorType);
            Assert.Equal(0x00, descriptor.Interface);
            Assert.Equal(0xff, descriptor.InterfaceProtocol);
            Assert.Equal(0x00, descriptor.InterfaceNumber);
            Assert.Equal(0x00, descriptor.AlternateSetting);
            Assert.Equal(0xff, descriptor.InterfaceClass);
            Assert.Equal(0xff, descriptor.InterfaceSubClass);
            Assert.Equal(0x03, descriptor.NumEndpoints);
        }

        [Fact]
        public void GetEndpointDescriptor_shall_return_a_valid_descriptor()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);

            bool result = this._winUsbDevice.GetEndpointDescriptor(0, out UsbEndpointDescriptor descriptor);
            Assert.True(result);

            Assert.Equal(9, descriptor.Length);
            Assert.Equal(UsbDescriptorTypeEnum.USB_ENDPOINT_DESCRIPTOR_TYPE, (UsbDescriptorTypeEnum)descriptor.DescriptorType);
            Assert.Equal(0x00, descriptor.mAttributes);
            Assert.Equal(0x00, descriptor.MaxPacketSize);
            Assert.Equal(0x00, descriptor.EndpointAddress);
            Assert.Equal(0x00, descriptor.Interval);
        }

        [Fact]
        public void GetCurrentAlternateSetting_shall_return_a_valid_setting()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            bool result = this._winUsbDevice.GetCurrentAlternateSetting(out byte alternateSetting);
            Assert.True(result);
            Assert.Equal(0x00, alternateSetting);
        }

        [Fact]
        public void SetCurrentAlternateSetting_shall_return_a_valid_setting()
        {
            this.CreateWinUSBDeviceHandle(deviceDescription);
            bool result = this._winUsbDevice.SetCurrentAlternateSetting(0x00);
            Assert.True(result);
        }
    }
}
