using Common.Test.Xunit;
using Windows.Devices.Usb;
using log4net.Core;

namespace Log4UsbServiceTest
{
    public class USB_LoggingCfgTest
            : XUnitTestBase
    {
        public USB_LoggingCfgTest() : base()
        {
        }

        [Fact]
        public void ConstructorDefault_shall_initialize_class() 
        {
            var testObject = new Log4UsbService.EmbeddedData.USB_LoggingCfg();

            Assert.NotNull(testObject.SetupPacket);
            Assert.Equal(UsbTransferDirection.Out, testObject.SetupPacket.RequestType.Direction);
            Assert.Equal(UsbControlTransferType.Vendor, testObject.SetupPacket.RequestType.ControlTransferType);
            Assert.Equal(UsbControlRecipient.Device, testObject.SetupPacket.RequestType.Recipient);
            Assert.Equal(0x01, testObject.SetupPacket.Request);
            Assert.Equal((uint)0x0000, testObject.SetupPacket.Value);
            Assert.Equal((uint)0x0000, testObject.SetupPacket.Index);
            Assert.False(testObject.IsLoggingEnabled);
            Assert.Equal(Level.Off,testObject.LoggingLevel);
        }

        [Fact]
        public void Constructor_with_Level_and_enable_shall_initialize_class()
        {
            var testObject = new Log4UsbService.EmbeddedData.USB_LoggingCfg(Level.Fatal, true);

            Assert.NotNull(testObject.SetupPacket);
            Assert.Equal(UsbTransferDirection.Out, testObject.SetupPacket.RequestType.Direction);
            Assert.Equal(UsbControlTransferType.Vendor, testObject.SetupPacket.RequestType.ControlTransferType);
            Assert.Equal(UsbControlRecipient.Device, testObject.SetupPacket.RequestType.Recipient);
            Assert.Equal(0x01, testObject.SetupPacket.Request);
            Assert.Equal((uint)0x0001, testObject.SetupPacket.Value);
            Assert.Equal((uint)0x0001, testObject.SetupPacket.Index);
            Assert.True(testObject.IsLoggingEnabled);
            Assert.Equal(Level.Fatal, testObject.LoggingLevel);

        }


    }
}
