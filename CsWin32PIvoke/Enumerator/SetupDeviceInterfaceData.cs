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
    /// <summary>
    /// 
    /// </summary>
    public class SetupDeviceInterfaceData
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupDeviceInterfaceData"/> class.
        /// </summary>
        /// <param name="src">The source.</param>
        internal SetupDeviceInterfaceData(SP_DEVICE_INTERFACE_DATA src)
        {
            this._src = src;
            this._src.cbSize = (uint)Marshal.SizeOf<SP_DEVICE_INTERFACE_DATA>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupDeviceInterfaceData"/> class.
        /// </summary>
        public SetupDeviceInterfaceData() : this(new SP_DEVICE_INTERFACE_DATA())
        {
        }

        // Member variables
        internal SP_DEVICE_INTERFACE_DATA _src;


        // properties
        /// <summary>
        /// Gets the size of the class structure.
        /// </summary>
        /// <value>
        /// The size of the cb.
        /// </value>
        public uint CbSize { get => this._src.cbSize;}

        /// <summary>The GUID for the class to which the device interface belongs.</summary>
        public Guid InterfaceClassGuid { get => this._src.InterfaceClassGuid; set => this._src.InterfaceClassGuid = value; }

        /// <summary>Can be one or more of the following:</summary>
        public uint Flags { get => this._src.Flags; set => this._src.Flags = value; }

        /// <summary>Reserved. Do not use.</summary>
        public nuint Reserved { get => this._src.Reserved; set => this._src.Reserved = value; }

        public override string ToString()
        {
            return $"CbSize = {CbSize}, InterfaceClassGuid = {InterfaceClassGuid}, Flags = {Flags}";
        }

    }

}
