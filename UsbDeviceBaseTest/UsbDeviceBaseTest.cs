#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8603, CS8604, CS8602
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using log4net.Core;
using MemoryPack;
using UsbDeviceBase;
using UsbDeviceBaseTest.Dummy;
using Windows.Devices.Usb;
using Windows.Storage.Streams;
using Xunit;

namespace UsbDeviceBaseTest
{
    public class UsbDeviceBaseTest : XUnitTestBase
    {

        public UsbDeviceBaseTest() : base()
        {
        }


        [Fact]
        public void UsbDeviceTest_Constructor()
        {
            var device = new UsbDeviceTest();
            Assert.NotNull(device);
            Assert.Equal(0x03EB, device.VendorId);
            Assert.Equal(0x0001, device.ProductId);
            Assert.Equal(ByteOrder.LittleEndian, device.ByteOrder);
            Assert.Equal(Windows.Storage.Streams.UnicodeEncoding.Utf8, device.UnicodeEncoding);

            Assert.Throws<InvalidOperationException>(() => device.ConfigurationValue);
            Assert.Throws<InvalidOperationException>(() => device.MaxPowerMilliamps);
            Assert.Throws<InvalidOperationException>(() => device.ConfigurationValue);
            Assert.Throws<InvalidOperationException>(() => device.RemoteWakeup);
            Assert.Throws<InvalidOperationException>(() => device.SelfPowered);

        }

        [Fact]
        public void Initialize_shall_create_device_objects()
        {
            var device = new UsbDeviceTest();
            device.Initialize();

            device.VerifyDevice();
        }

        [Fact]
        public void GetUsbInterfaceControl_shall_return_control_interface()
        {
            var device = new UsbDeviceTest();
            device.Initialize();

            Assert.NotNull(device.UsbInterfaceControl);
        }

        [Fact]
        public void SendControlOutTransfer_shall_send_control_out_transfer()
        {
            var device = new UsbDeviceTest();
            device.Initialize();

            uint result = device.SendControlOutTransfer(UsbSetupPacketOut);
            Assert.Equal((uint)0, result);
        }


        [Fact]
        public async Task SendControlOutTransferAsync_shall_send_control_out_transfer()
        {
            var device = new UsbDeviceTest();
            device.Initialize();

            uint result = await device.SendControlOutTransferAsync(UsbSetupPacketOut);
            Assert.Equal((uint)0, result);
        }

        [Fact]
        public void SendControlInTransfer_shall_send_control_in_transfer()
        {
            var device = new UsbDeviceTest();
            device.Initialize();
            IBuffer buffer = device.SendControlInTransfer(UsbSetupPacketIn, 4);
            byte[] data = buffer.ToArray();
            Assert.Empty(data);
        }

        [Fact]
        public async Task SendControlInTransferAsync_shall_send_control_in_transfer()
        {
            var device = new UsbDeviceTest();

            device.Initialize();
            IBuffer buffer = await device.SendControlInTransferAsync(UsbSetupPacketIn, 4);
            byte[] data = buffer.ToArray();
            Assert.Empty(data);
        }

        [Fact]
        public void SendControlInTransfer_template_shall_send_control_in_transfer()
        {
            var device = new UsbDeviceTest();
            device.Initialize();
            UInt32 buffer = device.SendControlInTransfer<UInt32>(UsbSetupPacketIn, 4);
            Assert.Equal((UInt32)0, buffer);
        }

        [Fact]
        public async Task SendControlInTransferAsync_template_shall_send_control_in_transfer()
        {
            var device = new UsbDeviceTest();
            device.Initialize();
        
            UInt32 buffer = await device.SendControlInTransferAsync<UInt32>(UsbSetupPacketIn, 4);
            Assert.Equal((UInt32)0, buffer);
        }

        private UsbSetupPacket UsbSetupPacketOut => new UsbSetupPacket()
        {
            RequestType = new UsbControlRequestType()
            {
                Direction = UsbTransferDirection.Out,
                ControlTransferType = UsbControlTransferType.Vendor,
                Recipient = UsbControlRecipient.Device
            },
            Request = 0x00,
            Value = 0x0001,
            Index = 0x0000,
            Length = 0
        };

        private UsbSetupPacket UsbSetupPacketIn => new UsbSetupPacket()
        {
            RequestType = new UsbControlRequestType()
            {
                Direction = UsbTransferDirection.In,
                ControlTransferType = UsbControlTransferType.Vendor,
                Recipient = UsbControlRecipient.Device
            },
            Request = 0x00,
            Value = 0x0001,
            Index = 0x0001,
            Length = 1

        };
    }
}
