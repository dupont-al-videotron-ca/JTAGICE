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
    public class SpDevInfoData
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SpDevInfoData"/> class.
        /// </summary>
        /// <param name="src">The source.</param>
        internal SpDevInfoData(SP_DEVINFO_DATA src)
        {
            this._src = src;
            this._src.cbSize = (uint)Marshal.SizeOf<SP_DEVINFO_DATA>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpDevInfoData"/> class.
        /// </summary>
        public SpDevInfoData() :this(new SP_DEVINFO_DATA())
        {
        }

        // Member variables
        internal SP_DEVINFO_DATA _src;

        // properties
        /// <summary>
        /// Gets the size of the class structure.
        /// </summary>
        /// <value>
        /// The size of the cb.
        /// </value>
        public uint CbSize { get => this._src.cbSize;}

        /// <summary>The GUID for the class to which the device interface belongs.</summary>
        public Guid ClassGuid { get => this._src.ClassGuid; set => this._src.ClassGuid = value; }

        /// <summary>Can be one or more of the following:</summary>
        public uint DevInst { get => this._src.DevInst; set => this._src.DevInst = value; }

        /// <summary>Reserved. Do not use.</summary>
        public nuint Reserved { get => this._src.Reserved; set => this._src.Reserved = value; } 

    }

}
