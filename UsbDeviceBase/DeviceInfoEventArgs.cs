#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8618, CS8603, CS8604, CA1416
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using MemoryPack;
using Windows.Devices.Enumeration;
using Windows.Devices.Usb;

using Windows.Foundation.Metadata;
using Windows.Storage.Streams;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UsbDeviceBase
{
    public class DeviceInfoEventArgs : EventArgs
    {
        public DeviceInformation DeviceInfo { get; }

        public DeviceInfoEventArgs(DeviceInformation deviceInfo)
        {
            DeviceInfo = deviceInfo;
        }
    }
}
