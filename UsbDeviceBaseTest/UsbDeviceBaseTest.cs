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
        private UsbDeviceTest? _device;

        public UsbDeviceBaseTest() : base()
        {
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _device?.Dispose();
                _device = null;
            }

            base.Dispose(disposing);
        }

        [Fact]
        public void UsbDeviceTest_Constructor()
        {
            var test = CreateDeviceTest(false);
            Assert.NotNull(test);
            Assert.Equal(0x03EB, test.VendorId);
            Assert.Equal(0x0001, test.ProductId);
            Assert.Equal(ByteOrder.LittleEndian, test.ByteOrder);
            Assert.Equal(Windows.Storage.Streams.UnicodeEncoding.Utf8, test.UnicodeEncoding);

            Assert.Throws<InvalidOperationException>(() => test.ConfigurationValue);
            Assert.Throws<InvalidOperationException>(() => test.MaxPowerMilliamps);
            Assert.Throws<InvalidOperationException>(() => test.ConfigurationValue);
            Assert.Throws<InvalidOperationException>(() => test.RemoteWakeup);
            Assert.Throws<InvalidOperationException>(() => test.SelfPowered);

        }

        [Fact]
        public void Initialize_shall_create_device_objects()
        {
            var test = CreateDeviceTest();
            test.VerifyDevice();
        }

        [Fact(Skip = "not suported by usb")]
        public void ReadStatus_Test()
        {
            var test = CreateDeviceTest();

            var s = UsbSetupPacketIn;

            s.RequestType.Direction = UsbTransferDirection.In;
            s.RequestType.ControlTransferType = UsbControlTransferType.Standard;
            s.RequestType.Recipient = UsbControlRecipient.Device;
            s.Length = 2;
            s.Request = 0x00;
            s.Value = 0x0000;
            s.Index = 0;

            byte[]? buf;
            test.SendControlInTransfer(UsbSetupPacketIn, (int)s.Length);

            Task.Delay(100)?.Wait();
            var readPipe = test.ImplInterfaces[0].InPipes[1];
            if (readPipe.IsByteToRead)
            {
                if (readPipe.ReadBytes(out buf, (int)s.Length, 5000))
                {
                    Assert.NotEmpty(buf);
                    Logger.Info($"Read {buf.Length} bytes: {BitConverter.ToString(buf)}");
                }
                else
                    Assert.Fail("pipe 1");
            }
            else
                Assert.Fail("pipe 1");

        }

        [Fact(Skip = "not suported by usb")]
        public void ReadDAC_Test()
        {
            var test = CreateDeviceTest();

            var s = UsbSetupPacketIn;

            s.RequestType.Direction = UsbTransferDirection.In;
            s.RequestType.ControlTransferType = UsbControlTransferType.Vendor;
            s.RequestType.Recipient = UsbControlRecipient.Device;
            s.Request = 0x01;
            s.Value = 0x0006;  // 1ms
            s.Index = 256;      // 256 samples
            s.Length = s.Index*2; // 256*2 bytes

            byte[]? buf;
            test.SendControlInTransfer(UsbSetupPacketIn, (int)s.Length);

            Task.Delay((int)s.Index*1)?.Wait();
            var readPipe = test.ImplInterfaces[0].InPipes[2];
            if (readPipe.IsByteToRead)
            {
                if (readPipe.ReadBytes(out buf, (int)s.Length, 5000))
                {
                    Assert.NotEmpty(buf);
                    Logger.Info($"Read {buf.Length} bytes: {BitConverter.ToString(buf)}");
                }
                else
                    Assert.Fail("pipe 1");
            }
            else
                Assert.Fail("pipe 1");

        }

        [Fact]
        public void GetUsbInterfaceControl_shall_return_control_interface()
        {
            var test = CreateDeviceTest(); ;
            Assert.NotNull(test.UsbInterfaceControl);
        }

        [Fact]
        public void SendControlOutTransfer_shall_send_control_out_transfer()
        {
            var test = CreateDeviceTest();

            uint result = test.SendControlOutTransfer(UsbSetupPacketOut);
            Assert.Equal((uint)0, result);
        }


        [Fact]
        public async Task SendControlOutTransferAsync_shall_send_control_out_transfer()
        {
            var test = CreateDeviceTest();

            uint result = await test.SendControlOutTransferAsync(UsbSetupPacketOut);
            Assert.Equal((uint)0, result);
        }

        [Fact]
        public void SendControlInTransfer_shall_send_control_in_transfer()
        {
            var test = CreateDeviceTest();
            IBuffer buffer = test.SendControlInTransfer(UsbSetupPacketIn, 4);
            byte[] data = buffer.ToArray();
            Assert.Empty(data);
        }

        [Fact]
        public async Task SendControlInTransferAsync_shall_send_control_in_transfer()
        {
            var test = CreateDeviceTest();

            IBuffer buffer = await test.SendControlInTransferAsync(UsbSetupPacketIn, 4);
            byte[] data = buffer.ToArray();
            Assert.Empty(data);
        }

        [Fact]
        public void SendControlInTransfer_template_shall_send_control_in_transfer()
        {
            var test = CreateDeviceTest();
            UInt32 buffer = test.SendControlInTransfer<UInt32>(UsbSetupPacketIn, 4);
            Assert.Equal((UInt32)0, buffer);
        }

        [Fact(Skip = "not suported by usb")]
        public async Task SendControlInTransferAsync_template_shall_send_control_in_transfer()
        {
            var test = CreateDeviceTest();

            UInt32? buffer = await test.SendControlInTransferAsync<UInt32>(UsbSetupPacketIn, 4);
            Assert.NotNull(buffer);
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

        private UsbDeviceTest CreateDeviceTest(bool callInit = true)
        {
            _device = new UsbDeviceTest();
            if (callInit)
            {
                int retry = 0;
                while (!_device.IsOpened && retry++ < 10)
                    Task.Delay(500)?.Wait();

                Assert.True(_device.IsOpened);
            }

            return _device;

        }
    }
}
