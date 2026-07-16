using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Common.Test.Xunit;
using log4net.Core;
using Log4UsbService.EmbeddedData;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using UsbDeviceBase;
using Windows.Devices.Usb;
using WinRT;
using Xunit.Sdk;

namespace Log4UsbServiceTest
{
    public class Log4UsbServiceTest : XUnitTestBase
    {
        Mock<IUSBControlDevice> IUSBControlDeviceMock;
        Mock<IUsbPipeIn> IUsbPipeInMock;
        public Log4UsbServiceTest()
        {
            IUSBControlDeviceMock = this.MockRepository.Create<IUSBControlDevice>();
            IUsbPipeInMock = this.MockRepository.Create<IUsbPipeIn>();
        }

        [Fact]
        public void Log4UsbService_Constructor_null_parameters()
        {
            var result = Assert.Throws<ArgumentNullException>(() => new Log4UsbService.Log4UsbService(null!, null!));
            Assert.Equal("controlDevice", result.ParamName);
            result = Assert.Throws<ArgumentNullException>(() => new Log4UsbService.Log4UsbService(IUSBControlDeviceMock.Object, null!));
            Assert.Equal("pipe", result.ParamName);
        }

        [Fact]
        public void Log4UsbService_Constructor_test_good_parameters()
        {
            var testObj = new Log4UsbService.Log4UsbService(IUSBControlDeviceMock.Object, IUsbPipeInMock.Object);

            Assert.NotNull(testObj);
        }

        [Fact]
        public void Log4UsbService_Dispose()
        {
            var testObj = new Log4UsbService.Log4UsbService(IUSBControlDeviceMock.Object, IUsbPipeInMock.Object);
            testObj.Dispose();
        }

        [Fact]
        public void Log4UsbService_StartReceivingLog()
        {
            //--- Setup
            Level testLevel = Level.Fatal;
            byte testLevelEmb = (byte)USB_LevelType.Fatal;

            var testObj = new Log4UsbService.Log4UsbService(IUSBControlDeviceMock.Object, IUsbPipeInMock.Object);
            USB_LoggingEventData_t refUSB_LoggingEventData_t = new USB_LoggingEventData_t();

            refUSB_LoggingEventData_t.Header.Version = 1;
            refUSB_LoggingEventData_t.TickCount = 0x12345678;
            refUSB_LoggingEventData_t.Level = (byte)testLevelEmb;

            //--- Expectations
            USB_LoggingEventData_t expectedFrame = new USB_LoggingEventData_t();

            expectedFrame.Header.Version = refUSB_LoggingEventData_t.Header.Version;
            expectedFrame.TickCount = refUSB_LoggingEventData_t.TickCount;
            expectedFrame.Level = refUSB_LoggingEventData_t.Level;
            string expectedLogName = "TestLog";
            string expectedFileName = "Test.c";
            string expectedMessage = "Test";

            List<byte> bytes = new List<byte>();

            bytes.AddRange(System.Text.Encoding.UTF8.GetBytes(expectedLogName+"\0"));
            bytes.AddRange(System.Text.Encoding.UTF8.GetBytes(expectedFileName + "\0"));
            bytes.AddRange(System.Text.Encoding.UTF8.GetBytes(expectedMessage + "\0"));
            refUSB_LoggingEventData_t.Header.Length += (ushort) bytes.Count();
            expectedFrame.Header.Length += (ushort)bytes.Count();

            int nbLogged = 0;
            
            byte[] b = bytes.ToArray();
            this.IUSBControlDeviceMock.Setup(u => u.SendControlOutTransfer(It.IsAny<UsbSetupPacket>())).Returns(0);

            this.IUsbPipeInMock.Setup(p => p.ReadBytes(out b, It.IsAny<uint>(), It.IsAny<int>())).Callback(() =>
            {
                b = bytes.ToArray();
                nbLogged++;
            }).Returns(true);

            this.IUsbPipeInMock.Setup(p => p.ReadStructure<USB_LoggingEventData_t>(ref It.Ref<USB_LoggingEventData_t>.IsAny, It.IsAny<int>()))
                .Callback((ref USB_LoggingEventData_t frame, int timeout) =>
                {
                    frame = refUSB_LoggingEventData_t;
                    nbLogged++;
                }).Returns(true);
            this.IUSBControlDeviceMock.Setup(u => u.IsConnected).Returns(true);


            //--- Action
            testObj.StartReceivingLog(Level.Debug);

            //--- Verification
            while (nbLogged < 2)
            {
                Thread.Sleep(2);
            }
            testObj.Dispose();
            Assert.True(nbLogged >= 2, "Log was not received");

            if (testLevel >= Level.Error)
            {
                this.VerifyLogForError = false;
            }

            var result = this.LoggedEvents.FirstOrDefault(e => 
                e.Level == testLevel &&
                e.LoggerName == expectedLogName);

            Assert.NotNull(result);
            Assert.NotNull(result.RenderedMessage);
            Assert.Equal("[305419896 ms]: Test.c, Test", result.RenderedMessage);

        }

        [Fact]
        public void Log4UsbService_StopReceivingLog_shall_stop_running_task()
        {
            //--- Setup
            Level testLevel = Level.Fatal;
            byte testLevelEmb = (byte)USB_LevelType.Fatal;

            var testObj = new Log4UsbService.Log4UsbService(IUSBControlDeviceMock.Object, IUsbPipeInMock.Object);
            USB_LoggingEventData_t refUSB_LoggingEventData_t = new USB_LoggingEventData_t();

            refUSB_LoggingEventData_t.Header.Version = 1;
            refUSB_LoggingEventData_t.TickCount = 0x12345678;
            refUSB_LoggingEventData_t.Level = (byte)testLevelEmb;

            //--- Expectations
            USB_LoggingEventData_t expectedFrame = new USB_LoggingEventData_t();

            expectedFrame.Header.Version = refUSB_LoggingEventData_t.Header.Version;
            expectedFrame.TickCount = refUSB_LoggingEventData_t.TickCount;
            expectedFrame.Level = refUSB_LoggingEventData_t.Level;
            string expectedLogName = "TestLog";
            string expectedFileName = "Test.c";
            string expectedMessage = "Test";

            List<byte> bytes = new List<byte>();

            bytes.AddRange(System.Text.Encoding.UTF8.GetBytes(expectedLogName + "\0"));
            bytes.AddRange(System.Text.Encoding.UTF8.GetBytes(expectedFileName + "\0"));
            bytes.AddRange(System.Text.Encoding.UTF8.GetBytes(expectedMessage + "\0"));
            refUSB_LoggingEventData_t.Header.Length += (ushort)bytes.Count();
            expectedFrame.Header.Length += (ushort)bytes.Count();

            int nbLogged = 0;

            byte[] b = bytes.ToArray();
            this.IUSBControlDeviceMock.Setup(u => u.SendControlOutTransfer(It.IsAny<UsbSetupPacket>())).Returns(0);

            this.IUsbPipeInMock.Setup(p => p.ReadStructure<USB_LoggingEventData_t>(ref It.Ref<USB_LoggingEventData_t>.IsAny, It.IsAny<int>()))
                .Callback((ref USB_LoggingEventData_t frame, int timeout) =>
                {
                    frame = refUSB_LoggingEventData_t;
                    nbLogged++;
                }).Returns(false);

            this.IUSBControlDeviceMock.Setup(u => u.IsConnected).Returns(true);
            testObj.StartReceivingLog(Level.Debug);
            while (nbLogged < 1)
            {
                Thread.Sleep(2);
            }


            //--- Action
            var result = testObj.StopReceivingLog();
            Assert.True(result);

            //--- Verification

            Assert.True(nbLogged >= 2, "Log was not received");
            var resultLog = this.LoggedEvents.FirstOrDefault(e =>
                e.Level == testLevel &&
                e.LoggerName == expectedLogName);

            Assert.Null(resultLog);

            testObj.Dispose();
        }

        [Fact]
        public void Log4UsbService_StopReceivingLog_shall_stop_task()
        {
            //--- Setup
            Level testLevel = Level.Fatal;
            byte testLevelEmb = (byte)USB_LevelType.Fatal;

            var testObj = new Log4UsbService.Log4UsbService(IUSBControlDeviceMock.Object, IUsbPipeInMock.Object);
            USB_LoggingEventData_t refUSB_LoggingEventData_t = new USB_LoggingEventData_t();

            refUSB_LoggingEventData_t.Header.Version = 1;
            refUSB_LoggingEventData_t.TickCount = 0x12345678;
            refUSB_LoggingEventData_t.Level = (byte)testLevelEmb;

            //--- Expectations
            this.IUSBControlDeviceMock.Setup(u => u.SendControlOutTransfer(It.IsAny<UsbSetupPacket>())).Returns(0);
            string expectedLogName = "TestLog";


            //--- Action
            var result = testObj.StopReceivingLog();
            Assert.True(result);

            //--- Verification
            var resultLog = this.LoggedEvents.FirstOrDefault(e =>
                e.Level == testLevel &&
                e.LoggerName == expectedLogName);

            Assert.Null(resultLog);

            testObj.Dispose();
        }
        [Fact]
        public void Log4UsbService_StopReceivingLog_shall_fail_to_stop_Embedded_device()
        {
            //--- Setup
            Level testLevel = Level.Fatal;

            var testObj = new Log4UsbService.Log4UsbService(IUSBControlDeviceMock.Object, IUsbPipeInMock.Object);

            //--- Expectations
            this.IUSBControlDeviceMock.Setup(u => u.SendControlOutTransfer(It.IsAny<UsbSetupPacket>())).Returns(1);

            //--- Action
            var result = testObj.StopReceivingLog();
            Assert.False(result);

            //--- Verification
            this.VerifyLogForError = false;

            var logResult = this.LoggedEvents.FirstOrDefault(e => e.Level == Level.Error);
            Assert.NotNull(logResult);
            Assert.Equal("Failed to send logging configuration to USB device. Error code: 1", logResult.RenderedMessage);

            testObj.Dispose();
        }

        [Fact]
        public void Log4UsbService_StopReceivingLog_shall_timeout_to_stop_running_task()
        {
            //--- Setup
            Level testLevel = Level.Fatal;
            byte testLevelEmb = (byte)USB_LevelType.Fatal;

            var testObj = new Log4UsbService.Log4UsbService(IUSBControlDeviceMock.Object, IUsbPipeInMock.Object);
            USB_LoggingEventData_t refUSB_LoggingEventData_t = new USB_LoggingEventData_t();

            refUSB_LoggingEventData_t.Header.Version = 1;
            refUSB_LoggingEventData_t.TickCount = 0x12345678;
            refUSB_LoggingEventData_t.Level = (byte)testLevelEmb;

            //--- Expectations
            USB_LoggingEventData_t expectedFrame = new USB_LoggingEventData_t();

            expectedFrame.Header.Version = refUSB_LoggingEventData_t.Header.Version;
            expectedFrame.TickCount = refUSB_LoggingEventData_t.TickCount;
            expectedFrame.Level = refUSB_LoggingEventData_t.Level;
            string expectedLogName = "TestLog";
            string expectedFileName = "Test.c";
            string expectedMessage = "Test";

            List<byte> bytes = new List<byte>();

            bytes.AddRange(System.Text.Encoding.UTF8.GetBytes(expectedLogName + "\0"));
            bytes.AddRange(System.Text.Encoding.UTF8.GetBytes(expectedFileName + "\0"));
            bytes.AddRange(System.Text.Encoding.UTF8.GetBytes(expectedMessage + "\0"));
            refUSB_LoggingEventData_t.Header.Length += (ushort)bytes.Count();
            expectedFrame.Header.Length += (ushort)bytes.Count();

            int nbLogged = 0;

            byte[] b = bytes.ToArray();
            this.IUSBControlDeviceMock.Setup(u => u.SendControlOutTransfer(It.IsAny<UsbSetupPacket>())).Returns(0);

            this.IUsbPipeInMock.Setup(p => p.ReadStructure<USB_LoggingEventData_t>(ref It.Ref<USB_LoggingEventData_t>.IsAny, It.IsAny<int>()))
                .Callback((ref USB_LoggingEventData_t frame, int timeout) =>
                {
                    frame = refUSB_LoggingEventData_t;
                    nbLogged++;
                    Thread.Sleep(timeout*3);
                }).Returns(false);

            this.IUSBControlDeviceMock.Setup(u => u.IsConnected).Returns(true);
            testObj.StartReceivingLog(Level.Debug);
            while(nbLogged < 1)
            {
                Thread.Sleep(1);
            }

            //--- Action
            var result = testObj.StopReceivingLog();
            Assert.False(result);

            //--- Verification
            VerifyLogForError = false;
            var resultLog = this.LoggedEvents.FirstOrDefault(e => e.Level == Level.Error);
            Assert.NotNull(resultLog);
            Assert.Equal("Timeout while waiting for receive task to complete.", resultLog.RenderedMessage);

            testObj.Dispose();
        }

    }
}
