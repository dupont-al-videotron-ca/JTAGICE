#pragma warning disable CS8625

using System.IO;
using System.Runtime.InteropServices;
using Common.Test.Xunit;
using CsWin32Api;
using log4net.Core;
using Xunit;

namespace CsWin32ApiTest
{
    public class WinUsbApiEnumeratorTest : XUnitTestBase
    {
        private SafeHandle? safeHandle = null;
        const string deviceDescription = "JTAGICE mkII";

        public WinUsbApiEnumeratorTest() : base(false)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (safeHandle != null)
                {
                    if (!safeHandle.IsClosed)
                    {
                        safeHandle.Close();
                    }

                    safeHandle = null;
                }
            }

            base.Dispose(disposing);
        }

        [Fact]
        /// JTAGICE mkII shall be powered on.
        public void CreateFileHandle_shall_return_valid_handle()
        {
            // Test
            this.safeHandle = WinUsbApi.CreateFileHandle(@"\\?\usb#vid_03eb&pid_2103#00b0000006b4#{a5dcbf10-6530-11d2-901f-00c04fb951ed}");

            // Verification
            Assert.NotNull(this.safeHandle);
            Assert.False(this.safeHandle.IsInvalid);

        }

        [Fact]
        public void CreateFileHandle_shall_return_invalid_handle()
        {
            // Test
            this.safeHandle = WinUsbApi.CreateFileHandle(@"\\?\usb#vid_03eb&pid_2103#00b0000006b4");

            // Verification
            Assert.NotNull(this.safeHandle);
            Assert.True(this.safeHandle.IsInvalid);
        }

        [Fact]
        public void CreateFileHandle_shall_throw_ArgumentException()
        {
            // Test
            // Verification
            Assert.Throws<ArgumentNullException>("path", () => WinUsbApi.CreateFileHandle(null));
        }

        [Fact]
        public void GetDeviceInfoListSafeHandle_shall_return_valid_handle()
        {
            this.safeHandle = WinUsbApi.GetDeviceInfoListSafeHandle(UsbConstants.GUID_DEVINTERFACE_USB_DEVICE,
                null,
                null,
                SetupDiGetClassDevsFlags.DIGCF_PRESENT | SetupDiGetClassDevsFlags.DIGCF_DEVICEINTERFACE);

            Assert.NotNull(this.safeHandle);

            Assert.False(this.safeHandle.IsInvalid);

        }

        [Fact]
        public void GetDeviceInfoListSafeHandle_shall_return_valid_handle_no_guid()
        {
            this.safeHandle = WinUsbApi.GetDeviceInfoListSafeHandle(null,
                null,
                null,
                SetupDiGetClassDevsFlags.DIGCF_PRESENT | SetupDiGetClassDevsFlags.DIGCF_ALLCLASSES);

            Assert.NotNull(this.safeHandle);

            Assert.False(this.safeHandle.IsInvalid);

        }

        [Fact]
        public void GetDeviceInfoListSafeHandle_shall_return_valid_handle_with_enumerator()
        {
            // TODO: enumerator not working.
            this.safeHandle = WinUsbApi.GetDeviceInfoListSafeHandle(null,
                "USB",
                null,
                SetupDiGetClassDevsFlags.DIGCF_ALLCLASSES | SetupDiGetClassDevsFlags.DIGCF_DEVICEINTERFACE);

            Assert.NotNull(this.safeHandle);

            Assert.True(this.safeHandle.IsInvalid);

        }

        [Fact]
        public void SetupDiGetClassDevs_shall_return_dev_info_data()
        {

            bool result = WinUsbApi.SetupDiGetClassDevs(UsbConstants.GUID_DEVINTERFACE_USB_DEVICE,
                null,
                null, SetupDiGetClassDevsFlags.DIGCF_PRESENT | SetupDiGetClassDevsFlags.DIGCF_DEVICEINTERFACE,
                out Dictionary<uint, SpDevInfoData> spDevInfoDatas,
                out Dictionary<uint, DevPropKey> devPropKeys);

            Assert.True(result);
            Assert.NotNull(spDevInfoDatas);
            Assert.NotNull(devPropKeys);

            Assert.NotEmpty(devPropKeys);
            Assert.NotEmpty(spDevInfoDatas);
            Assert.Equal(devPropKeys.Count, spDevInfoDatas.Count);

        }

        [Fact]
        public void SetupDiGetClassDevs_shall_return_all_dev_info_data()
        {

            bool result = WinUsbApi.SetupDiGetClassDevs(null,
                null,
                null, SetupDiGetClassDevsFlags.DIGCF_ALLCLASSES,
                out Dictionary<uint, SpDevInfoData> spDevInfoDatas,
                out Dictionary<uint, DevPropKey> devPropKeys);

            Assert.True(result);
            Assert.NotNull(spDevInfoDatas);
            Assert.NotNull(devPropKeys);

            Assert.NotEmpty(devPropKeys);
            Assert.NotEmpty(spDevInfoDatas);
            Assert.Equal(devPropKeys.Count, spDevInfoDatas.Count);
        }

        [Fact]
        public void SetupDiGetClassDevs_shall_failed()
        {

            bool result = WinUsbApi.SetupDiGetClassDevs(null,
                null,
                null, SetupDiGetClassDevsFlags.DIGCF_PRESENT | SetupDiGetClassDevsFlags.DIGCF_DEVICEINTERFACE,
                out Dictionary<uint, SpDevInfoData> spDevInfoDatas,
                out Dictionary<uint, DevPropKey> devPropKeys);

            Assert.False(result);
            Assert.NotNull(spDevInfoDatas);
            Assert.NotNull(devPropKeys);

            Assert.Empty(devPropKeys);
            Assert.Empty(spDevInfoDatas);

        }

        [Fact]
        /// JTAGICE mkII shall be powered on.
        public void EnumerateAllDevicesWithGuid_shall_return_list_device()
        {
            bool result = WinUsbApi.EnumerateAllDevicesWithGuid(UsbConstants.GUID_DEVINTERFACE_USB_DEVICE, out Dictionary<uint, NodeUsbDeviceData> deviceList);

            Assert.True(result);
            Assert.NotNull(deviceList);
            Assert.NotEmpty(deviceList);

            List<KeyValuePair<uint, NodeUsbDeviceData>> a = deviceList.Where(e => e.Value.RegsiteryProperty.ContainsValue(deviceDescription)).ToList();
            KeyValuePair<uint, NodeUsbDeviceData>? ba = deviceList.FirstOrDefault(e => e.Value.RegsiteryProperty.ContainsValue(deviceDescription));

            Assert.NotEmpty(a);
            Assert.NotNull(ba);

            string resultData = a.First().Value.RegsiteryProperty[SpDiRegisteryProperty.SPDRP_DEVICEDESC];
            Assert.Equal(deviceDescription, resultData);
            Assert.Equal(deviceDescription, ba?.Value.RegsiteryProperty[SpDiRegisteryProperty.SPDRP_DEVICEDESC]);

            resultData = a.First().Value.RegsiteryProperty[SpDiRegisteryProperty.SPDRP_DRIVER];
            Assert.Equal(@"{c671678c-82c1-43f3-d700-0049433e9a4b}\0001", resultData);

        }

        [Fact]
        public void EnumerateAllDevices_shall_return_list_device()
        {
            bool result = WinUsbApi.EnumerateAllDevices(out Dictionary<uint, NodeUsbDeviceData> deviceList, out Dictionary<uint, NodeUsbDeviceData> hubList);

            Assert.True(result);
            Assert.NotNull(deviceList);
            Assert.NotEmpty(deviceList);
            Assert.NotNull(hubList);
            Assert.NotEmpty(hubList);
        }


        [Fact]
        public void GetDeviceInterfaces_shall_return_data()
        {
            var devInfoHandle = WinUsbApi.GetDeviceInfoListSafeHandle(UsbConstants.GUID_DEVINTERFACE_USB_DEVICE, null, null, SetupDiGetClassDevsFlags.DIGCF_PRESENT | SetupDiGetClassDevsFlags.DIGCF_DEVICEINTERFACE);

            Assert.NotNull(devInfoHandle);

            bool result = WinUsbApi.GetDeviceInterfaces(devInfoHandle, null, UsbConstants.GUID_DEVINTERFACE_USB_DEVICE, 0, out SetupDeviceInterfaceData deviceInterfaceData);

            Assert.True(result);
            Assert.NotNull(deviceInterfaceData);

        }

        [Fact]
        /// JTAGICE mkII shall be powered on.
        public void FindDevicePathForUsbDevice_shall_return_a_valid_device_path()
        {
            string result = WinUsbApi.FindDevicePathForUsbDevice(deviceDescription);

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            this.safeHandle = WinUsbApi.CreateFileHandle(result);
            Assert.NotNull(this.safeHandle);
            Assert.False(this.safeHandle.IsInvalid);
        }

        [Fact]
        public void FindDevicePathForUsbDevice_shall_return_a_null_device_path()
        {
            string requestDescription = deviceDescription + " error";
            string result = WinUsbApi.FindDevicePathForUsbDevice(requestDescription);

            Assert.Null(result);

            this.VerifyLogForError = false;
            LoggingEvent logevent = this.LoggedEvents.First(e => e.Level == Level.Error);

            Assert.Equal($"No device found with description: {requestDescription}", logevent.RenderedMessage);
        }
    }
}
