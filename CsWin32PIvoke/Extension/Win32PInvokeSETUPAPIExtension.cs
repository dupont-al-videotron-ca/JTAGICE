#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using CsWin32Api;
using Devices.DeviceAndDriverInstallation;
using Windows.Win32.Devices.DeviceAndDriverInstallation;
using winmdroot = global::Windows.Win32;

namespace Windows.Win32
{

    internal static partial class PInvoke
    {

        [SupportedOSPlatform("windows5.0")]
        internal static winmdroot.Foundation.BOOL SetupDiGetDeviceInterfaceDetail2(
            SafeHandle DeviceInfoSet,
            in winmdroot.Devices.DeviceAndDriverInstallation.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            in uint RequiquestedSize,
            out SP_DEVICE_INTERFACE_DETAIL_DATA_W2 DeviceInterfaceDetailData2,
            out uint RequiredSize)
        {
            bool DeviceInfoSetAddRef = false;
            uint DeviceInterfaceDetailDataLength = RequiquestedSize;
            string devivePath = string.Empty;

            IntPtr buffer = Marshal.AllocHGlobal((int)RequiquestedSize);
            try
            {
                unsafe
                {
                    SP_DEVICE_INTERFACE_DETAIL_DATA_W* t = (SP_DEVICE_INTERFACE_DETAIL_DATA_W*)buffer.ToPointer();
                    t->cbSize = (uint)sizeof(SP_DEVICE_INTERFACE_DETAIL_DATA_W);

                    fixed (uint* RequiredSizeLocal = &RequiredSize)
                    {
                        fixed (winmdroot.Devices.DeviceAndDriverInstallation.SP_DEVICE_INTERFACE_DATA* DeviceInterfaceDataLocal = &DeviceInterfaceData)
                        {
                            winmdroot.Devices.DeviceAndDriverInstallation.HDEVINFO DeviceInfoSetLocal;
                            if (DeviceInfoSet is object)
                            {
                                DeviceInfoSet.DangerousAddRef(ref DeviceInfoSetAddRef);
                                DeviceInfoSetLocal = (winmdroot.Devices.DeviceAndDriverInstallation.HDEVINFO)DeviceInfoSet.DangerousGetHandle();
                            }
                            else
                                throw new ArgumentNullException(nameof(DeviceInfoSet));

                            winmdroot.Foundation.BOOL __result = PInvoke.SetupDiGetDeviceInterfaceDetail(
                                DeviceInfoSetLocal,
                                DeviceInterfaceDataLocal,
                                t,
                                DeviceInterfaceDetailDataLength,
                                RequiredSizeLocal,
                                null);

                            return __result;
                        }
                    }
                }
            }
            finally
            {
                SP_DEVICE_INTERFACE_DETAIL_DATA_W temp = Marshal.PtrToStructure<SP_DEVICE_INTERFACE_DETAIL_DATA_W>(buffer); ;

                DeviceInterfaceDetailData2.cbSize = temp.cbSize;
                DeviceInterfaceDetailData2.DevicePathStr = new string(Marshal.PtrToStringUni(buffer + 4));

                Marshal.FreeHGlobal(buffer);

                if (DeviceInfoSetAddRef)
                    DeviceInfoSet.DangerousRelease();
            }
        }

        [SupportedOSPlatform("windows5.0")]
        internal static winmdroot.Foundation.BOOL SetupDiGetDeviceInterfaceDetail2(
            SafeHandle DeviceInfoSet,
            in winmdroot.Devices.DeviceAndDriverInstallation.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            in uint RequiquestedSize,
            out SetupDeviceInterfaceDataDetail DeviceInterfaceDetailData,
            out uint RequiredSize)
        {
            bool DeviceInfoSetAddRef = false;
            uint DeviceInterfaceDetailDataLength = RequiquestedSize;

            IntPtr buffer = Marshal.AllocHGlobal((int)RequiquestedSize);
            try
            {
                unsafe
                {
                    SP_DEVICE_INTERFACE_DETAIL_DATA_W* t = (SP_DEVICE_INTERFACE_DETAIL_DATA_W*)buffer.ToPointer();
                    t->cbSize = (uint)sizeof(SP_DEVICE_INTERFACE_DETAIL_DATA_W);

                    fixed (uint* RequiredSizeLocal = &RequiredSize)
                    {
                        fixed (winmdroot.Devices.DeviceAndDriverInstallation.SP_DEVICE_INTERFACE_DATA* DeviceInterfaceDataLocal = &DeviceInterfaceData)
                        {
                            winmdroot.Devices.DeviceAndDriverInstallation.HDEVINFO DeviceInfoSetLocal;
                            if (DeviceInfoSet is object)
                            {
                                DeviceInfoSet.DangerousAddRef(ref DeviceInfoSetAddRef);
                                DeviceInfoSetLocal = (winmdroot.Devices.DeviceAndDriverInstallation.HDEVINFO)DeviceInfoSet.DangerousGetHandle();
                            }
                            else
                                throw new ArgumentNullException(nameof(DeviceInfoSet));

                            winmdroot.Foundation.BOOL __result = PInvoke.SetupDiGetDeviceInterfaceDetail(
                                DeviceInfoSetLocal,
                                DeviceInterfaceDataLocal,
                                t,
                                DeviceInterfaceDetailDataLength,
                                RequiredSizeLocal,
                                null);

                            return __result;
                        }
                    }
                }
            }
            finally
            {
                SP_DEVICE_INTERFACE_DETAIL_DATA_W temp = Marshal.PtrToStructure<SP_DEVICE_INTERFACE_DETAIL_DATA_W>(buffer); ;

                DeviceInterfaceDetailData = new SetupDeviceInterfaceDataDetail(temp);
                DeviceInterfaceDetailData._src.DevicePathStr = new string(Marshal.PtrToStringUni(buffer + 4));

                Marshal.FreeHGlobal(buffer);

                if (DeviceInfoSetAddRef)
                    DeviceInfoSet.DangerousRelease();
            }
        }
    }
}