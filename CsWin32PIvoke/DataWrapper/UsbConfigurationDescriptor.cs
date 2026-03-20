#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Windows.Win32.Devices.DeviceAndDriverInstallation;
using Windows.Win32.Devices.Usb;
using Windows.Win32.Foundation;
using winmdroot = global::Windows.Win32;

namespace CsWin32Api
{
    public class UsbConfigurationDescriptor : IDataWrapper<USB_CONFIGURATION_DESCRIPTOR>
    {

        public UsbConfigurationDescriptor() : this(new USB_CONFIGURATION_DESCRIPTOR())
        {
        }

        internal UsbConfigurationDescriptor(USB_CONFIGURATION_DESCRIPTOR src)
        {
            this._src = src;
            _src.bLength = (byte)Marshal.SizeOf<USB_CONFIGURATION_DESCRIPTOR>();
        }

        internal USB_CONFIGURATION_DESCRIPTOR _src;

        USB_CONFIGURATION_DESCRIPTOR IDataWrapper<USB_CONFIGURATION_DESCRIPTOR>._src { get => this._src; set => this._src = value; }

        public byte Length { get => _src.bLength; set => _src.bLength = value; }


        public byte DescriptorType { get => _src.bDescriptorType; set => _src.bDescriptorType = value; }

        public ushort TotalLength { get => _src.wTotalLength; set => _src.wTotalLength = value; }

        public byte NumInterfaces { get => _src.bNumInterfaces; set => _src.bNumInterfaces = value; }

        public byte ConfigurationValue{ get => _src.bConfigurationValue; set => _src.bConfigurationValue = value; }

        public byte Configuration { get => _src.iConfiguration; set => _src.iConfiguration = value; }

        public byte Attributes { get => _src.bmAttributes; set => _src.bmAttributes = value; }

        public byte MaxPower { get => _src.MaxPower; set => _src.MaxPower = value; }
    }
}
