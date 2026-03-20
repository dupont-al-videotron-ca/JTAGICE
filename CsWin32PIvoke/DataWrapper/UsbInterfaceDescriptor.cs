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
using Windows.Win32.Devices.Usb;
using Windows.Win32.Foundation;
using winmdroot = global::Windows.Win32;

namespace CsWin32Api
{
    /// <summary>
    /// usb interface descriptor class.
    /// </summary>
    public class UsbInterfaceDescriptor : IDataWrapper<USB_INTERFACE_DESCRIPTOR>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="UsbInterfaceDescriptor"/> class.
        /// </summary>
        /// <param name="src">The source.</param>
        internal UsbInterfaceDescriptor(USB_INTERFACE_DESCRIPTOR src)
        {
            this._src = src;         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsbInterfaceDescriptor"/> class.
        /// </summary>
        public UsbInterfaceDescriptor()
        {
            this._src = new USB_INTERFACE_DESCRIPTOR();
        }

        // Member variables
        internal USB_INTERFACE_DESCRIPTOR _src;

        USB_INTERFACE_DESCRIPTOR IDataWrapper<USB_INTERFACE_DESCRIPTOR>._src { get => this._src; set => this._src = value; }


        // properties
        public byte Length { get => this._src.bLength; set => this._src.bLength = value; }
        public byte DescriptorType { get => this._src.bDescriptorType; set => this._src.bDescriptorType = value; }
        public byte InterfaceNumber { get => this._src.bInterfaceNumber; set => this._src.bInterfaceNumber = value; }
        public byte AlternateSetting { get => this._src.bAlternateSetting; set => this._src.bAlternateSetting = value; }
        public byte NumEndpoints { get => this._src.bNumEndpoints; set => this._src.bNumEndpoints = value; }
        public byte InterfaceClass { get => this._src.bInterfaceClass; set => this._src.bInterfaceClass = value; }
        public byte InterfaceSubClass { get => this._src.bInterfaceSubClass; set => this._src.bInterfaceSubClass = value; }
        public byte InterfaceProtocol { get => this._src.bInterfaceProtocol; set => this._src.bInterfaceProtocol = value; }
        public byte Interface { get => this._src.iInterface; set => this._src.iInterface = value; }

    }
}
