using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbDeviceBase
{
    public class UsbInterfaceControl : UsbInterfaceBase
    {


        #region Constructors 
        public UsbInterfaceControl(Windows.Devices.Usb.UsbInterfaceDescriptor descriptor, IEnumerable<KeyValuePair<int, PipeOutBase>> outPipes, IEnumerable<KeyValuePair<int, PipeInBase>> inPipes) : base(descriptor, outPipes, inPipes)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 


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
