#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8603, CS8604
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Win32.SafeHandles;
using Windows.Win32.Devices.DeviceAndDriverInstallation;
using Windows.Win32.Devices.Properties;
using Windows.Win32.Devices.Usb;
using Windows.Win32.Foundation;
using static CsWin32Api.WinUsbApi;
using winmdroot = global::Windows.Win32;

namespace CsWin32Api
{
    public static partial class WinUsbApi
    {

        private static ILog logger = LogManager.GetLogger(typeof(WinUsbApi));

        /// <summary>
        /// Initializes the usb handle.
        /// </summary>
        /// <param name="deviceFileHandle">The usb handle.</param>
        /// <param name="usbDeviceHandle">The win interface handle.</param>
        /// <returns></returns>
        public static WinUsbDeviceApi WinUsbDeviceApiFactory(SafeFileHandle deviceFileHandle)
        {
            try
            {
                bool retval = winmdroot.PInvoke.WinUsb_Initialize(deviceFileHandle, out winmdroot.WinUsb_FreeSafeHandle handle);

                if (!retval)
                {
                    logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
                    return null;
                }
                else
                {
                    return new WinUsbDeviceApi(handle);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Exception in {nameof(WinUsbDeviceApiFactory)}: {ex}");
                return null;
            }
        }
    }
}
