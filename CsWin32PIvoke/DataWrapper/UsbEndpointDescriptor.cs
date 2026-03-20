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
    public class UsbEndpointDescriptor : IDataWrapper<USB_ENDPOINT_DESCRIPTOR>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="UsbInterfaceDescriptor"/> class.
        /// </summary>
        /// <param name="src">The source.</param>
        internal UsbEndpointDescriptor(USB_ENDPOINT_DESCRIPTOR src)
        {
            this._src = src;         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsbInterfaceDescriptor"/> class.
        /// </summary>
        public UsbEndpointDescriptor()
        {
            this._src = new USB_ENDPOINT_DESCRIPTOR();
        }

        // Member variables
        internal USB_ENDPOINT_DESCRIPTOR _src;

        USB_ENDPOINT_DESCRIPTOR IDataWrapper<USB_ENDPOINT_DESCRIPTOR>._src { get => this._src; set => this._src = value; }

        // properties
        public byte Length { get => this._src.bLength; set => this._src.bLength = value; }
        public byte DescriptorType { get => this._src.bDescriptorType; set => this._src.bDescriptorType = value; }

        public byte EndpointAddress{ get => _src.bEndpointAddress; set => _src.bEndpointAddress = value; }

        public byte mAttributes{ get => _src.bmAttributes; set => _src.bmAttributes = value; }

        public ushort MaxPacketSize{ get => _src.wMaxPacketSize; set => _src.wMaxPacketSize = value; }     

        public byte Interval{ get => _src.bInterval; set => _src.bInterval = value; }

    }
}
