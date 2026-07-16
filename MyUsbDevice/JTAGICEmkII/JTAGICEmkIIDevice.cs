#pragma warning disable CS1591,CS1573,CS0465,CS0649,CS8019,CS1570,CS1584,CS1658,CS0436,CS8981,SYSLIB1092, CS8625, CS8618, CS8603, CS8604, CA1416
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using Windows.Foundation.Metadata;
using Windows.Storage.Streams;
using UsbDeviceBase;
using Windows.Devices.Usb;

namespace MyUsbDevice.JTAGICEmkII
{
    public class JTAGICEmkIIDevice : UsbDeviceBase.UsbDeviceBase
    {
        public override ushort VendorId => 0x03eb;

        public override ushort ProductId => 0x2103;

        public override string DeviceId => "\\\\?\\usb#vid_03eb&pid_2103#00b0000006b4#{a5dcbf10-6530-11d2-901f-00c04fb951ed}";

        public override string DeviceName => "JTAGICE mkII";

        protected override UsbInterfaceBase CreateInterface(UsbInterfaceDescriptor descriptor, IEnumerable<KeyValuePair<int, PipeOutBase>> outPipes, IEnumerable<KeyValuePair<int, PipeInBase>> inPipes)
        {
            return new JTAGICEUsbInterface(descriptor, outPipes, inPipes);
        }

        public JTAGICEUsbInterface JTAGICEInterface 
        {
            get
            {
                if (this.ImplInterfaces.TryGetValue(0, out var iface))
                {
                    return iface as JTAGICEUsbInterface;
                }
                return null!;
            }
        }   
    }
}
