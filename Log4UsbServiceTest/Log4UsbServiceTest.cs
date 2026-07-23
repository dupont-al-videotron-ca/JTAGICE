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
using Microsoft.Extensions.Logging;
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
        Mock<IUsbDevice> IUsbDeviceMock;
        Mock<IUsbPipeIn> IUsbPipeInMock;
        Mock<ILogger<Log4UsbService.Log4UsbService>> ILoggerMock;

        public Log4UsbServiceTest()
        {
            IUsbDeviceMock = this.MockRepository.Create<IUsbDevice>();
            IUsbPipeInMock = this.MockRepository.Create<IUsbPipeIn>();
            ILoggerMock = this.MockRepository.Create<ILogger<Log4UsbService.Log4UsbService>>(MockBehavior.Loose);
        }

        [Fact]
        public void Log4UsbService_Constructor_null_parameters()
        {
            var result = Assert.Throws<ArgumentNullException>(() => new Log4UsbService.Log4UsbService(0,0,null!, this.Logger));
            Assert.Equal("controlDevice", result.ParamName);
            result = Assert.Throws<ArgumentNullException>(() => new Log4UsbService.Log4UsbService(0, 0, IUsbDeviceMock.Object, null!, this.Logger));
            Assert.Equal("pipeIn", result.ParamName);
            result = Assert.Throws<ArgumentNullException>(() => new Log4UsbService.Log4UsbService(0, 0, IUsbDeviceMock.Object, IUsbPipeInMock.Object, null!));
            Assert.Equal("logger", result.ParamName);
        }


        [Fact]
        public void Log4UsbService_Constructor_test_good_parameters()
        {
            var testObj = CreateLog4UsbService();

            Assert.NotNull(testObj);
        }

        [Fact]
        public void Log4UsbService_Dispose()
        {
            var testObj = CreateLog4UsbService();
            testObj.Dispose();
        }

        [Fact]
        public void Log4UsbService_StartReceivingLog_when_device_is_opened()
        {
            //--- Setup
            Level testLevel = Level.Fatal;
            byte testLevelEmb = (byte)USB_LevelType.Fatal;

            var testObj = CreateLog4UsbService();
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
            this.IUsbDeviceMock.Setup(u => u.SendControlOutTransfer(It.IsAny<UsbSetupPacket>())).Returns(0);
            this.IUsbDeviceMock.Setup(u => u.IsOpened).Returns(true);
            this.IUsbDeviceMock.Setup(u => u.WaitOpenned(It.IsAny<TimeSpan>())).Returns(true);

            this.IUsbPipeInMock.Setup(p => p.ReadBytes(out b, It.IsAny<int>(), It.IsAny<int>())).Callback(() =>
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
            this.IUsbDeviceMock.Setup(u => u.IsConnected).Returns(true);


            //--- Action
            testObj.StartReceivingLog(Level.Debug);

            //--- Verification
            while (nbLogged < 2)
            {
                Thread.Sleep(2);
            }

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
            Assert.True(testObj.IsReceivingLog);

            testObj.Dispose();

        }

        [Fact]
        public void Log4UsbService_StartReceivingLog_when_device_is_not_opened()
        {
            //--- Setup
            var testObj = CreateLog4UsbService();

            //--- Expectations
            this.IUsbDeviceMock.Setup(u => u.WaitOpenned(It.IsAny<TimeSpan>())).Returns(false);

            //--- Action
            var result = testObj.StartReceivingLog(Level.Debug);
            Assert.False(result);
            testObj.Dispose();

            this.VerifyLogForError = false;
            var resultLog = this.LoggedEvents.FirstOrDefault(e =>
                e.Level == Level.Error);

            Assert.NotNull(resultLog);
            Assert.NotNull(resultLog.RenderedMessage);
            Assert.Equal("USB device is not opened.", resultLog.RenderedMessage);
            Assert.False(testObj.IsReceivingLog);

        }

        [Fact]
        public void Log4UsbService_StopReceivingLog_shall_stop_running_task()
        {
            //--- Setup
            Level testLevel = Level.Fatal;
            byte testLevelEmb = (byte)USB_LevelType.Fatal;

            var testObj = CreateLog4UsbService();
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
            this.IUsbDeviceMock.Setup(u => u.SendControlOutTransfer(It.IsAny<UsbSetupPacket>())).Returns(0);
            this.IUsbDeviceMock.Setup(u => u.WaitOpenned(It.IsAny<TimeSpan>())).Returns(true);

            this.IUsbPipeInMock.Setup(p => p.ReadStructure<USB_LoggingEventData_t>(ref It.Ref<USB_LoggingEventData_t>.IsAny, It.IsAny<int>()))
                .Callback((ref USB_LoggingEventData_t frame, int timeout) =>
                {
                    frame = refUSB_LoggingEventData_t;
                    nbLogged++;
                }).Returns(false);
            this.IUsbDeviceMock.Setup(u => u.IsOpened).Returns(true);
            this.IUsbDeviceMock.Setup(u => u.IsConnected).Returns(true);

            testObj.StartReceivingLog(Level.Debug);
            Assert.True(testObj.IsReceivingLog);
            while (nbLogged < 1)
            {
                Thread.Sleep(2);
            }


            //--- Action
            var result = testObj.StopReceivingLog();
            Assert.True(result);
            Assert.False(testObj.IsReceivingLog);

            //--- Verification

            Assert.True(nbLogged >= 2, "Log was not received");
            var resultLog = this.LoggedEvents.FirstOrDefault(e =>
                e.Level == testLevel &&
                e.LoggerName == expectedLogName);

            Assert.Null(resultLog);

            testObj.Dispose();
        }

        [Fact]
        public void Log4UsbService_StopReceivingLog_shall_stop_task_when_task_is_not_running()
        {
            //--- Setup
            Level testLevel = Level.Fatal;
            byte testLevelEmb = (byte)USB_LevelType.Fatal;

            var testObj = CreateLog4UsbService();
            USB_LoggingEventData_t refUSB_LoggingEventData_t = new USB_LoggingEventData_t();

            refUSB_LoggingEventData_t.Header.Version = 1;
            refUSB_LoggingEventData_t.TickCount = 0x12345678;
            refUSB_LoggingEventData_t.Level = (byte)testLevelEmb;

            //--- Expectations
            this.IUsbDeviceMock.Setup(u => u.SendControlOutTransfer(It.IsAny<UsbSetupPacket>())).Returns(0);
            this.IUsbDeviceMock.Setup(u => u.IsOpened).Returns(true);

            string expectedLogName = "TestLog";

            Assert.False(testObj.IsReceivingLog);

            //--- Action
            var result = testObj.StopReceivingLog();
            Assert.True(result);
            Assert.False(testObj.IsReceivingLog);

            //--- Verification
            var resultLog = this.LoggedEvents.FirstOrDefault(e =>
                e.Level == testLevel &&
                e.LoggerName == expectedLogName);

            Assert.Null(resultLog);

            testObj.Dispose();
        }

        [Fact]
        public void Log4UsbService_StopReceivingLog_shall_fail_to_stop_Embedded_when_device_is_opened()
        {
            //--- Setup
            Level testLevel = Level.Fatal;

            var testObj = CreateLog4UsbService();

            //--- Expectations
            this.IUsbDeviceMock.Setup(u => u.SendControlOutTransfer(It.IsAny<UsbSetupPacket>())).Returns(1);
            this.IUsbDeviceMock.Setup (u=> u.IsOpened).Returns(true);

            //--- Action
            var result = testObj.StopReceivingLog();
            Assert.False(result);
            Assert.False(testObj.IsReceivingLog);

            //--- Verification
            this.VerifyLogForError = false;

            var logResult = this.LoggedEvents.FirstOrDefault(e => e.Level == Level.Error);
            Assert.NotNull(logResult);
            Assert.Equal("Failed to send logging configuration to USB device. Error code: 1", logResult.RenderedMessage);

            testObj.Dispose();
        }

        [Fact]
        public void Log4UsbService_StopReceivingLog_shall_fail_to_stop_Embedded_when_device_is_not_opened()
        {
            //--- Setup
            Level testLevel = Level.Fatal;

            var testObj = CreateLog4UsbService();

            //--- Expectations
            this.IUsbDeviceMock.Setup(u => u.IsOpened).Returns(false);
            Assert.False(testObj.IsReceivingLog);

            //--- Action
            var result = testObj.StopReceivingLog();
            Assert.False(result);
            Assert.False(testObj.IsReceivingLog);

            //--- Verification
            this.VerifyLogForError = false;

            var logResult = this.LoggedEvents.FirstOrDefault(e => e.Level == Level.Error);
            Assert.NotNull(logResult);
            Assert.Equal("USB device is not opened.", logResult.RenderedMessage);

            testObj.Dispose();
        }

        private Log4UsbService.Log4UsbService CreateLog4UsbService()
        {
            return new Log4UsbService.Log4UsbService(1,1,IUsbDeviceMock.Object, IUsbPipeInMock.Object, this.Logger);
        }
    }
}
