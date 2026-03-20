#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8603
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using CsWin32Api;
using Devices.DeviceAndDriverInstallation;
using log4net;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
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
        /// Opens a file at the specified path for asynchronous read and write access and returns a safe file handle to
        /// the file.   
        /// </summary>
        /// <remarks>The returned handle allows both reading and writing to the file and supports
        /// asynchronous operations. The file is opened with shared read and write access. If the file does not exist,
        /// an exception will be thrown.</remarks>
        /// <param name="path">The path to the file to open. The path must refer to an existing file.</param>
        /// <returns>A SafeFileHandle representing the opened file. The handle provides asynchronous read and write access to the
        /// file.</returns>
        /// <summary>
        /// Creates the usb handle.
        /// </summary>
        /// <param name="path">The path such as \\?\usb#vid_03eb&pid_0001#001#{a5dcbf10-6530-11d2-901f-00c04fb951ed}.</param>
        /// <returns></returns>
        public static SafeFileHandle CreateFileHandle(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path);

            SafeFileHandle result = winmdroot.PInvoke.CreateFile(path,
                    (uint)(winmdroot.Foundation.GENERIC_ACCESS_RIGHTS.GENERIC_WRITE | winmdroot.Foundation.GENERIC_ACCESS_RIGHTS.GENERIC_READ),
                    winmdroot.Storage.FileSystem.FILE_SHARE_MODE.FILE_SHARE_WRITE | winmdroot.Storage.FileSystem.FILE_SHARE_MODE.FILE_SHARE_READ,
                    null,
                    winmdroot.Storage.FileSystem.FILE_CREATION_DISPOSITION.OPEN_EXISTING,
                    winmdroot.Storage.FileSystem.FILE_FLAGS_AND_ATTRIBUTES.FILE_ATTRIBUTE_NORMAL | winmdroot.Storage.FileSystem.FILE_FLAGS_AND_ATTRIBUTES.FILE_FLAG_OVERLAPPED,
                    null);

            return result;
        }

        /// <summary>
        /// Find device path using it's device decription.
        /// </summary>
        /// <param name="deviceDescription"></param>
        /// <returns></returns>
        public static string FindDevicePathForUsbDevice(string deviceDescription)
        {
            bool result = WinUsbApi.EnumerateAllDevicesWithGuid(UsbConstants.GUID_DEVINTERFACE_USB_DEVICE, out Dictionary<uint, NodeUsbDeviceData> deviceList);

            KeyValuePair<uint, NodeUsbDeviceData>? foundDevice = deviceList.SingleOrDefault(e => e.Value.RegsiteryProperty.ContainsValue(deviceDescription));
            if (foundDevice.Value.Value == null)
            {
                logger.Error("No device found with description: " + deviceDescription);
                return null;
            }
            else 
            {
                string path = foundDevice.Value.Value.DeviceInterfaceDataDetails.DevicePathStr;
                logger.Info($"Device found with device path: {path} " + deviceDescription);
                return path;
            }
        }

        /// <summary>
        /// Retrieves a handle to a device information set that contains device information elements matching the
        /// specified criteria, such as device class, enumerator, and installation flags.
        /// </summary>
        /// <remarks>The returned handle must be released using the appropriate method to avoid resource
        /// leaks. This method is typically used to enumerate devices for installation or configuration tasks.</remarks>
        /// <param name="classGuid">The GUID of the device setup class to filter devices by. Specify null to include all device classes.</param>
        /// <param name="enumerator">The name of the device enumerator to filter devices by, such as 'USB' or 'PCI'. Specify null to include all
        /// enumerators.</param>
        /// <param name="hwndParent">A handle to the parent window for user interface purposes. Specify null if no user interface is required.</param>
        /// <param name="flags">A combination of SetupDiGetClassDevsFlags values that control the scope and behavior of the device
        /// information set returned.</param>
        /// <returns>A SafeHandle representing the device information set. The caller is responsible for releasing the handle
        /// when it is no longer needed.</returns>
        public static bool SetupDiGetClassDevs(Guid? classGuid, string? enumerator, SafeHandle? hwndParent, SetupDiGetClassDevsFlags flags,
            out Dictionary<uint, SpDevInfoData> spDevInfoDatas,
            out Dictionary<uint, DevPropKey> devPropKeys)
        {
            spDevInfoDatas = new Dictionary<uint, SpDevInfoData>();
            devPropKeys = new Dictionary<uint, DevPropKey>();

            using (var deviceInfoSet = WinUsbApi.GetDeviceInfoListSafeHandle(classGuid, enumerator, hwndParent, flags))
            {
                if (deviceInfoSet.IsInvalid)
                {
                    return false;
                }

                uint deviceIndex = 0;

                SpDevInfoData deviceInfoData = new SpDevInfoData();
                while (winmdroot.PInvoke.SetupDiEnumDeviceInfo(
                    deviceInfoSet,
                    deviceIndex,
                    ref deviceInfoData._src))
                {
                    deviceIndex++;

                    DevPropType propType;
                    DevPropKey propertyKey = new DevPropKey();
                    Guid devGuid = Guid.Empty;
                    uint size = 0;
                    DEVPROPTYPE outPevPropType;

                    Span<byte> buffer = new Span<byte>(devGuid.ToByteArray());
                    if (!winmdroot.PInvoke.SetupDiGetDeviceProperty(
                        deviceInfoSet,
                        in deviceInfoData._src,
                        in propertyKey._src,
                        out outPevPropType,
                        buffer,
                        out size,
                        0) || outPevPropType != DEVPROPTYPE.DEVPROP_TYPE_GUID)
                    {

                        if ((WIN32_ERROR)Marshal.GetLastWin32Error() != WIN32_ERROR.ERROR_NOT_FOUND)
                        {
                            //
                            // This device has an unknown device setup class.
                            //
                            logger.Debug($"Error this device has not property or missing {DEVPROPTYPE.DEVPROP_TYPE_GUID.ToString()} . {Marshal.GetLastPInvokeErrorMessage()}");
                        }
                        else
                        {
                            propType = (DevPropType)outPevPropType;
                            spDevInfoDatas.Add(deviceIndex - 1, deviceInfoData);
                            devPropKeys.Add(deviceIndex - 1, propertyKey);

                            propertyKey = new DevPropKey();
                            deviceInfoData = new SpDevInfoData();
                        }
                    }
                }
            }

            return devPropKeys.Any() || spDevInfoDatas.Any();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceList"></param>
        /// <param name="hubList"></param>
        /// <returns></returns>
        public static bool EnumerateAllDevices(out Dictionary<uint, NodeUsbDeviceData> deviceList, out Dictionary<uint, NodeUsbDeviceData> hubList)
        {
            bool a = EnumerateAllDevicesWithGuid(UsbConstants.GUID_DEVINTERFACE_USB_DEVICE, out deviceList);
            bool b = EnumerateAllDevicesWithGuid(UsbConstants.GUID_DEVINTERFACE_USB_HUB, out hubList);

            return a && b;
        }


        /// <summary>
        /// Retrieves a safe handle to a device information set that contains devices matching the specified criteria.
        /// </summary>
        /// <remarks>The returned SafeHandle encapsulates a device information set that must be released
        /// by disposing the handle to avoid resource leaks. The device information set includes only currently present
        /// devices and device interfaces, as specified by the flags.</remarks>
        /// <param name="classGuid">The device setup class GUID used to filter the device information set. If null, all classes are included.</param>
        /// <param name="enumerator">The name of the device enumerator to filter devices. If null, all enumerators are included.</param>
        /// <param name="hwndParent">A safe handle to the parent window used for user interface prompts. If null, no parent window is specified.</param>
        /// <param name="flags">A combination of flags that specify how devices are filtered and which device information is included.</param>
        /// <returns>A SafeHandle representing the device information set. The caller is responsible for disposing the handle
        /// when it is no longer needed.</returns>
        public static SafeHandle GetDeviceInfoListSafeHandle(Guid? classGuid, string? enumerator, SafeHandle? hwndParent, SetupDiGetClassDevsFlags flags)
        {
            HWND parentHandle = new HWND(hwndParent?.DangerousGetHandle() ?? HWND.Null);

            Windows.Win32.SetupDiDestroyDeviceInfoListSafeHandle deviceInfoSet = winmdroot.PInvoke.SetupDiGetClassDevs(
                classGuid,
                enumerator,
                parentHandle,
                (SETUP_DI_GET_CLASS_DEVS_FLAGS)flags);

            return deviceInfoSet;
        }

        public static bool EnumerateAllDevicesWithGuid(Guid classGuid, out Dictionary<uint, NodeUsbDeviceData> resultList)
        {
            resultList = new Dictionary<uint, NodeUsbDeviceData>();

            using (var deviceInfoSet = GetDeviceInfoListSafeHandle(classGuid,
                null,
                null,
                SetupDiGetClassDevsFlags.DIGCF_PRESENT | SetupDiGetClassDevsFlags.DIGCF_DEVICEINTERFACE))
            {
                if (deviceInfoSet.IsInvalid)
                {
                    return false;
                }
                else
                {
                    uint deviceIndex = 0;

                    SpDevInfoData deviceInfoData = new SpDevInfoData();
                    while (winmdroot.PInvoke.SetupDiEnumDeviceInfo(deviceInfoSet,
                        deviceIndex,
                        ref deviceInfoData._src))
                    {

                        NodeUsbDeviceData nodeUsbDeviceData = new NodeUsbDeviceData(deviceInfoSet);
                        nodeUsbDeviceData.SpDevInfoData = deviceInfoData;

                        deviceIndex++;

                        string propertyValue;
                        bool result = GetDeviceProperty(deviceInfoSet,
                                                    nodeUsbDeviceData.SpDevInfoData,
                                                    SpDiRegisteryProperty.SPDRP_DEVICEDESC,
                                                    out propertyValue);


                        if (result)
                        {
                            logger.Info($"{nodeUsbDeviceData.SpDevInfoData.DevInst}, {SpDiRegisteryProperty.SPDRP_DEVICEDESC}, {propertyValue}");
                            nodeUsbDeviceData.RegsiteryProperty.Add(SpDiRegisteryProperty.SPDRP_DEVICEDESC, propertyValue);
                        }
                        else
                        {
                            logger.Error($"Index = {deviceIndex - 1}, {Marshal.GetLastPInvokeErrorMessage()}");
                            continue;
                        }


                        result = GetDeviceProperty(deviceInfoSet,
                                                        nodeUsbDeviceData.SpDevInfoData,
                                                        SpDiRegisteryProperty.SPDRP_DRIVER,
                                                        out propertyValue);
                        if (result)
                        {
                            logger.Info($"{nodeUsbDeviceData.SpDevInfoData.DevInst}, {SpDiRegisteryProperty.SPDRP_DRIVER}, {propertyValue}");
                            nodeUsbDeviceData.RegsiteryProperty.Add(SpDiRegisteryProperty.SPDRP_DRIVER, propertyValue);
                        }
                        else
                        {
                            logger.Error($"Index = {deviceIndex - 1}, {Marshal.GetLastPInvokeErrorMessage()}");
                            continue;
                        }

                        if (GetDeviceInterfaces(deviceInfoSet,
                                        nodeUsbDeviceData.SpDevInfoData,
                                        classGuid,
                                        deviceIndex - 1,
                                        out SetupDeviceInterfaceData interfaceData))
                        {
                            logger.Info($"{nodeUsbDeviceData.SpDevInfoData.DevInst}, {interfaceData}");
                            nodeUsbDeviceData.DeviceInterfaceData = interfaceData;
                        }
                        else
                        {
                            continue;
                        }

                        if (GetDeviceInterfaceDetail(deviceInfoSet,
                                    nodeUsbDeviceData.SpDevInfoData,
                                    nodeUsbDeviceData.DeviceInterfaceData,
                                    out SetupDeviceInterfaceDataDetail deviceInterfaceDataDetail))
                        {
                            logger.Info($"{nodeUsbDeviceData.SpDevInfoData.DevInst}, {deviceInterfaceDataDetail}");
                            nodeUsbDeviceData.DeviceInterfaceDataDetails = deviceInterfaceDataDetail;
                        }
                        else
                        {
                            continue;
                        }

                        resultList.Add(deviceIndex - 1, nodeUsbDeviceData);
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Enumerates device interfaces for a specified device information set and device, retrieving information about
        /// a particular interface class instance.
        /// </summary>
        /// <remarks>This method wraps the SetupDiEnumDeviceInterfaces Windows API and is typically used
        /// to enumerate device interfaces for hardware devices. If the specified member index is out of range, the
        /// method returns false. The caller should check the return value to determine whether a valid device interface
        /// was found.</remarks>
        /// <param name="devInfoHandle">A handle to the device information set containing the device interfaces to enumerate. Must be valid and
        /// open.</param>
        /// <param name="SpDevInfoData">An optional device information data structure identifying the device for which interfaces are to be
        /// enumerated. If null, interfaces for all devices in the set are considered.</param>
        /// <param name="interfaceClassGuid">The GUID of the interface class to enumerate. Specifies the type of device interface to retrieve.</param>
        /// <param name="memberIndex">The zero-based index of the device interface to retrieve within the specified device information set and
        /// class.</param>
        /// <param name="deviceInterfaceData">When the method returns, contains information about the enumerated device interface.</param>
        /// <returns>true if the device interface was successfully retrieved; otherwise, false.</returns>
        public static bool GetDeviceInterfaces(SafeHandle devInfoHandle, SpDevInfoData? SpDevInfoData, Guid interfaceClassGuid, uint memberIndex, out SetupDeviceInterfaceData deviceInterfaceData)
        {
            deviceInterfaceData = new SetupDeviceInterfaceData();
            SP_DEVINFO_DATA? sdd = SpDevInfoData?._src;

            bool retval = winmdroot.PInvoke.SetupDiEnumDeviceInterfaces(devInfoHandle,
                    null,
                    interfaceClassGuid,
                    memberIndex,
                    ref deviceInterfaceData._src);

            if (!retval)
                logger.Error($"Index = {memberIndex}, {Marshal.GetLastPInvokeErrorMessage()}");

            return retval;
        }


        /// <summary>
        /// Gets the device interface detail.
        /// </summary>
        /// <param name="devInfoHandle">The dev information handle.</param>
        /// <param name="deviceInterfaceData">The device interface data.</param>
        /// <param name="devicePath">The device path.</param>
        /// <returns></returns>
        internal static bool GetDeviceInterfaceDetail(SafeHandle devInfoHandle, SpDevInfoData spDevInfoData, SetupDeviceInterfaceData deviceInterfaceData, out SetupDeviceInterfaceDataDetail deviceInterfaceDataDetail)
        {
            deviceInterfaceDataDetail = new SetupDeviceInterfaceDataDetail();

            uint requiredLength;

            // get requiredLength
            winmdroot.PInvoke.SetupDiGetDeviceInterfaceDetail(devInfoHandle,
                                                      deviceInterfaceData._src,
                                                      null,
                                                      out requiredLength,
                                                      ref spDevInfoData._src);

            Int32 errorCode = Marshal.GetLastPInvokeError();
            if (requiredLength == 0 || errorCode != (Int32)WIN32_ERROR.ERROR_INSUFFICIENT_BUFFER)
            {
                logger.Error($"Error code = {errorCode}, {Marshal.GetLastPInvokeErrorMessage()}");
                return false;
            }
            else
            {

                requiredLength = (uint)SetupDeviceInterfaceDataDetail.SizeOf((int)requiredLength);

                bool success = winmdroot.PInvoke.SetupDiGetDeviceInterfaceDetail2(devInfoHandle,
                                                          deviceInterfaceData._src,
                                                          requiredLength,
                                                          out SetupDeviceInterfaceDataDetail buffer,
                                                          out uint requiredLength2);

                deviceInterfaceDataDetail = buffer;

                errorCode = Marshal.GetLastPInvokeError();
                if (!success)
                {
                    logger.Error($"Error code = {errorCode}, {Marshal.GetLastPInvokeErrorMessage()}");
                    return false;
                }
                else
                {
                    //deviceInterfaceDataDetail._src = MemoryMarshal.AsRef<SP_DEVICE_INTERFACE_DETAIL_DATA_W>(detailData);
                    return true;
                }
            }
        }

        private static bool GetDeviceProperty(SafeHandle DeviceInfoSet, SpDevInfoData DeviceInfoData, SpDiRegisteryProperty Property, out string description)
        {
            bool result;
            description = string.Empty;

            winmdroot.PInvoke.SetupDiGetDeviceRegistryProperty(DeviceInfoSet,
                                                        DeviceInfoData._src,
                                                        (SETUP_DI_REGISTRY_PROPERTY)Property,
                                                        out uint propertyRegDataTypeLocal,
                                                        null,
                                                        out uint requiredLength);

            RegistryValueKind propertyRegDataType = (RegistryValueKind)propertyRegDataTypeLocal;

            if (requiredLength == 0)
            {
                logger.Error($"{Marshal.GetLastWin32Error()} ,{Marshal.GetLastPInvokeErrorMessage()}");
                return false;
            }

            byte[] buffer = new byte[requiredLength];
            result = winmdroot.PInvoke.SetupDiGetDeviceRegistryProperty(DeviceInfoSet,
                                                        DeviceInfoData._src,
                                                        (SETUP_DI_REGISTRY_PROPERTY)Property,
                                                        out propertyRegDataTypeLocal,
                                                        buffer,
                                                        out requiredLength);
            if (result)
            {
                switch (propertyRegDataType)
                {
                    case RegistryValueKind.Binary:
                        break;
                    case RegistryValueKind.String:
                    case RegistryValueKind.ExpandString:
                        if (Marshal.SystemDefaultCharSize == 2)
                        {
                            unsafe
                            {
                                fixed (byte* bufferLocal = buffer)
                                {
                                    string? myManagedString = Marshal.PtrToStringUni((IntPtr)(char*)bufferLocal);

                                    if (myManagedString != null)
                                        description = myManagedString;
                                }
                            }
                        }
                        else
                        {
                            unsafe
                            {
                                fixed (byte* bufferLocal = buffer)
                                {
                                    string? myManagedString = Marshal.PtrToStringUni((IntPtr)(byte*)bufferLocal);

                                    if (myManagedString != null)
                                        description = myManagedString;
                                }
                            }
                        }

                        break;
                    case RegistryValueKind.MultiString:
                        logger.Error($"{propertyRegDataType.ToString()} is not implemented.");
                        break;
                    case RegistryValueKind.DWord:
                        UInt16 dWord = UInt16.Parse(buffer);
                        description = dWord.ToString();
                        break;
                    case RegistryValueKind.QWord:
                        UInt32 qWord = UInt16.Parse(buffer);
                        description = qWord.ToString();
                        break;

                    case RegistryValueKind.None:
                    case RegistryValueKind.Unknown:
                    default:
                        break;
                }
                return true;
            }
            else
            {
                logger.Error($"{Marshal.GetLastPInvokeErrorMessage()}");
            }

            return false;
        }


    }

}




