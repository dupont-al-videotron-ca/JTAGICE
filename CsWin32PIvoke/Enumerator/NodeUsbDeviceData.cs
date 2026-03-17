#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Win32.SafeHandles;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.Win32.Devices.DeviceAndDriverInstallation;
using Windows.Win32.Devices.Properties;
using Windows.Win32.Devices.Usb;
using Windows.Win32.Foundation;
using static System.Net.WebRequestMethods;
using static CsWin32Api.WinUsbApi;
using winmdroot = global::Windows.Win32;

namespace CsWin32Api
{
    public class NodeUsbDeviceData
    {
        private Dictionary<SpDiRegisteryProperty, string> regsiteryProperty = new Dictionary<SpDiRegisteryProperty, string>();

        public NodeUsbDeviceData(SafeHandle handle)
        {
            ArgumentNullException.ThrowIfNull(handle);

            DevInfoHandle = handle;
            SpDevInfoData = new SpDevInfoData();
            DevPropKey = new DevPropKey();
            DeviceInterfaceData = new SetupDeviceInterfaceData();
            DeviceInterfaceDataDetails = new SetupDeviceInterfaceDataDetail();
        }

        public Dictionary<SpDiRegisteryProperty, string> RegsiteryProperty => regsiteryProperty;

        public SetupDeviceInterfaceData DeviceInterfaceData { get; set; }
        public SetupDeviceInterfaceDataDetail DeviceInterfaceDataDetails { get; set; }

        public SafeHandle DevInfoHandle { get; private set; }

        public SpDevInfoData SpDevInfoData { get; set; }

        public DevPropKey DevPropKey { get; set; }
    }
}
