using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using UsbDeviceBase;

namespace JTAGICEmkII.Adaptor
{
    public class TxUsbAdaptor : ITxFrameAdaptor
    {

        #region Constructors 
        public TxUsbAdaptor(IUsbPipeOut pipeOut) 
        {
            this._pipeOut = pipeOut ?? throw new ArgumentNullException(nameof(pipeOut));
        }
        #endregion


        #region Fields 

        private readonly IUsbPipeOut _pipeOut;


        #endregion


        #region Properties 

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        public int SendBytes(byte[] values)
        {
            if(_pipeOut == null)
            {
                throw new InvalidOperationException("Pipe is not initialized.");
            }

            return _pipeOut.Send(values);
        }

        public Task<int> SendBytesAsync(byte[] values, CancellationToken cancellationToken)
        {
            if(_pipeOut == null)
            {
                throw new InvalidOperationException("Pipe is not initialized.");
            }

            return _pipeOut.SendAsync(values);
        }

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
