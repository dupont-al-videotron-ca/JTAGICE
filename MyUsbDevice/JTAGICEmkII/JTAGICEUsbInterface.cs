using System;
using System.Collections.Generic;
using System.Linq;
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
    public class JTAGICEUsbInterface : UsbInterfaceBase
    {
        #region Constructors 

        public JTAGICEUsbInterface(UsbInterfaceDescriptor descriptor, IEnumerable<KeyValuePair<int, PipeOutBase>> outPipes, IEnumerable<KeyValuePair<int, PipeInBase>> inPipes) : base(descriptor, outPipes, inPipes)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public PipeOutBase? OutPipe => this.OutPipes.TryGetValue(2, out var pipe) ? pipe : null;

        public PipeInBase? InPipe => this.InPipes.TryGetValue(2, out var pipe) ? pipe : null;


        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        #endregion


        #region Protected Methods 

        #endregion

        #region Provate Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
