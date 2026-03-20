using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.Devices.DeviceAndDriverInstallation;
using Windows.Win32.Foundation;

namespace CsWin32Api
{
    /// <summary>
    /// Represents a device property key, which uniquely identifies a property in the Windows device property system.   
    /// </summary>
    /// <remarks>A device property key consists of a format identifier (FMTID) and a property identifier
    /// (PID). This class is typically used when interacting with device properties through Windows APIs, such as
    /// querying or setting property values. The property key must match the expected format and identifier for the
    /// property being accessed.</remarks>
    public class DevPropKey
    {
        /// <summary>
        /// Initializes a new instance of the DevPropKey class using the specified DEVPROPKEY value.    
        /// </summary>
        /// <param name="src">The DEVPROPKEY value to use for initializing the DevPropKey instance.</param>
        internal DevPropKey(DEVPROPKEY src)
        {
            this._src = src;
        }

        /// <summary>
        /// Initializes a new instance of the DevPropKey class. 
        /// </summary>
        /// <remarks>This constructor creates a DevPropKey object with its internal state set to a default
        /// DEVPROPKEY value. Use this constructor when you need an empty or default property key instance.</remarks>
        public DevPropKey()
        {
            this._src = new DEVPROPKEY();
        }

        // Member variables
        internal DEVPROPKEY _src;


        public Guid Fmtid { get => this._src.fmtid; set => this._src.fmtid = value;}

        public uint Pid { get => this._src.pid; set => this._src.pid = value; }

    }
}
