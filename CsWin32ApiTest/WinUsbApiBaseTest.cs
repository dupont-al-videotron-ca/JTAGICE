#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8603, CS8604, CS8618
#define AT90USBKEY
//#define JTAGICE 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using CsWin32Api;
using Microsoft.Win32.SafeHandles;

namespace CsWin32ApiTest
{
    public class WinUsbApiBaseTest : XUnitTestBase
    {
#if AT90USBKEY
        // AT90USBKEY
        internal const string deviceDescription = "WinUsb Device";
        internal const string deviceConnectionString = @"\\?\usb#vid_03eb&pid_0001#001#{a5dcbf10-6530-11d2-901f-00c04fb951ed}";
        internal const string expectedSpdrpRegistryDriverInformation = @"{88bae032-5a81-49f0-bc3d-a4ff138216d6}\0003";
#endif
#if JTAGICE 
        // JTAGICE mkII
        internal const string deviceDescription = "JTAGICE mkII";
        internal const string deviceConnectionString = @"\\?\usb#vid_03eb&pid_2103#00b0000006b4#{a5dcbf10-6530-11d2-901f-00c04fb951ed}";
        internal const string expectedSpdrpRegistryDriverInformation = @"{deb97e2c-8b0f-446f-b280-7cfac41c3bd9}\0000";
#endif
        internal WinUsbDeviceApi _winUsbDevice;
        internal SafeFileHandle? _fileHandle;
        internal SafeHandle? _deviceInfoHandle;

        public WinUsbApiBaseTest() : base(false)
        {

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_winUsbDevice != null)
                {
                    if (!_winUsbDevice.IsClosed)
                    {
                        _winUsbDevice.Close();
                    }

                    _winUsbDevice = null;
                }

                if (_fileHandle != null)
                {
                    if (!_fileHandle.IsClosed)
                    {
                        _fileHandle.Close();
                    }

                    _fileHandle = null;
                }

                if (_deviceInfoHandle != null)
                {
                    if (!_deviceInfoHandle.IsClosed)
                    {
                        _deviceInfoHandle.Close();
                    }

                    _deviceInfoHandle = null;
                }
            }

            base.Dispose(disposing);
        }

        internal void CreateWinUSBDeviceHandle(string deviceDescription)
        {
            var connectionPath = WinUsbApi.FindDevicePathForUsbDevice(deviceDescription);
            this._fileHandle = CsWin32Api.WinUsbApi.CreateFileHandle(connectionPath);
            this._winUsbDevice = WinUsbApi.WinUsbDeviceApiFactory(_fileHandle); ;
            Assert.NotNull(this._winUsbDevice);
        }
    }
}
