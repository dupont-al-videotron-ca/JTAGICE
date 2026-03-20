#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8603, CS8604
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Win32.SafeHandles;
using Serilog;
using Windows.Win32;
using Windows.Win32.Devices.DeviceAndDriverInstallation;
using Windows.Win32.Devices.Properties;
using Windows.Win32.Devices.Usb;
using Windows.Win32.Foundation;
using static CsWin32Api.WinUsbApi;
using winmdroot = global::Windows.Win32;

namespace CsWin32Api
{
    public class UsbDeviceDescriptor : IDataWrapper<USB_DEVICE_DESCRIPTOR>
    {

        public UsbDeviceDescriptor():this(new USB_DEVICE_DESCRIPTOR())
        {
        }

        internal UsbDeviceDescriptor(USB_DEVICE_DESCRIPTOR src)
        {
            this._src = src;
            _src.bLength = (byte)Marshal.SizeOf<USB_DEVICE_DESCRIPTOR>();
        }

        internal USB_DEVICE_DESCRIPTOR _src;
        USB_DEVICE_DESCRIPTOR IDataWrapper<USB_DEVICE_DESCRIPTOR>._src { get => this._src; set => this._src = value; }

        public byte Length { get => _src.bLength; set => _src.bLength = value; }

        public byte DescriptorType { get => _src.bDescriptorType; set => _src.bDescriptorType = value; }

        public ushort bcdUSB { get => _src.bcdUSB; set => _src.bcdUSB = value; }

        public byte DeviceClass { get => _src.bDeviceClass; set => _src.bDeviceClass = value; }

        public byte DeviceSubClass { get => _src.bDeviceSubClass; set => _src.bDeviceSubClass = value; }

        public byte DeviceProtocol { get => _src.bDeviceProtocol; set => _src.bDeviceProtocol = value; }

        public byte MaxPacketSize0 { get => _src.bMaxPacketSize0; set => _src.bMaxPacketSize0 = value; }

        public ushort idVendor { get => _src.idVendor; set => _src.idVendor = value; }

        public ushort idProduct { get => _src.idProduct; set => _src.idProduct = value; }

        public ushort bcdDevice { get => _src.bcdDevice; set => _src.bcdDevice = value; }

        public byte Manufacturer { get => _src.iManufacturer; set => _src.iManufacturer = value; }

        public byte Product { get => _src.iProduct; set => _src.iProduct = value; }

        public byte SerialNumber { get => _src.iSerialNumber; set => _src.iSerialNumber = value; }

        public byte NumConfigurations { get => _src.bNumConfigurations; set => _src.bNumConfigurations = value; }
    }
}
