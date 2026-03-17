#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

        /// <summary>
        /// Initializes the usb handle.
        /// </summary>
        /// <param name="deviceFileHandle">The usb handle.</param>
        /// <param name="usbDeviceHandle">The win interface handle.</param>
        /// <returns></returns>
        public static bool InitializeUsbHandle(SafeFileHandle deviceFileHandle, out SafeHandle usbDeviceHandle)
        {
            bool retval = winmdroot.PInvoke.WinUsb_Initialize(deviceFileHandle, out winmdroot.WinUsb_FreeSafeHandle handle);
            usbDeviceHandle = handle;

            if (!retval)
            {
                logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
            }

            return retval;
        }

        /// <summary>
        /// Queries the interface settings.
        /// </summary>
        /// <param name="usbHandle">The usb handle.</param>
        /// <param name="alternateInterfaceNumber">The alternate interface number.</param>
        /// <param name="usbInterfaceDescriptor">The usb interface descriptor.</param>
        /// <returns></returns>
        public static bool QueryInterfaceSettings(SafeHandle usbHandle, byte alternateInterfaceNumber, out UsbInterfaceDescriptor usbInterfaceDescriptor)
        {
            USB_INTERFACE_DESCRIPTOR usbAltInterfaceDescriptor;

            bool retval = winmdroot.PInvoke.WinUsb_QueryInterfaceSettings(usbHandle,
                    alternateInterfaceNumber,
                    out usbAltInterfaceDescriptor);

            if (!retval)
            {
                logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
            }

            usbInterfaceDescriptor = new UsbInterfaceDescriptor(usbAltInterfaceDescriptor);

            return retval;
        }

        /// <summary>
        /// Queries the pipe.
        /// </summary>
        /// <param name="usbHandle">The usb handle.</param>
        /// <param name="alternateInterfaceNumber">The alternate interface number.</param>
        /// <param name="pipeIndex">Index of the pipe.</param>
        /// <param name="usbPipeInformation">The usb pipe information.</param>
        /// <returns></returns>
        public static bool QueryPipe(SafeHandle usbHandle, byte alternateInterfaceNumber, byte pipeIndex, out UsbPipeInformation usbPipeInformation)
        {
            WINUSB_PIPE_INFORMATION usbAltInterfaceDescriptor;
            bool retval = winmdroot.PInvoke.WinUsb_QueryPipe(usbHandle,
                    alternateInterfaceNumber,
                    pipeIndex,
                    out usbAltInterfaceDescriptor);

            usbPipeInformation = new UsbPipeInformation(usbAltInterfaceDescriptor);
            return retval;
        }

        /// <summary>
        /// Sets the pipe policy.
        /// </summary>
        /// <param name="usbHandle">The usb handle.</param>
        /// <param name="pipeID">The pipe identifier.</param>
        /// <param name="policyType">Type of the policy.</param>
        /// <param name="valueLength">Length of the value.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool SetPipePolicy(SafeHandle usbHandle, byte pipeID, USBPipePolicyEnum policyType, byte data)
        {
            // TODO : revise
            var refData = MemoryMarshal.CreateReadOnlySpan(ref data, 1);
            bool retval = winmdroot.PInvoke.WinUsb_SetPipePolicy(usbHandle, pipeID, (winmdroot.Devices.Usb.WINUSB_PIPE_POLICY)policyType, refData);

            return retval;
        }

        /// <summary>
        /// Gets the pipe policy.
        /// </summary>
        /// <param name="usbHandle">The usb handle.</param>
        /// <param name="pipeID">The pipe identifier.</param>
        /// <param name="policyType">Type of the policy.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static bool GetPipePolicy(SafeHandle usbHandle, byte pipeID, USBPipePolicyEnum policyType, byte data)
        {
            // TODO : revise
            var refData = MemoryMarshal.CreateSpan(ref data, 1);
            uint len = (uint)refData.Length;

            return winmdroot.PInvoke.WinUsb_GetPipePolicy(usbHandle, pipeID, (winmdroot.Devices.Usb.WINUSB_PIPE_POLICY)policyType, ref len, refData);
        }

    }

}
