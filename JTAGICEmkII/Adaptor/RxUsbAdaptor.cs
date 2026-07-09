using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using UsbDeviceBase;

namespace JTAGICEmkII.Adaptor
{
    public class RxUsbAdaptor : IRxFrameAdaptor
    {

        #region Constructors 
        public RxUsbAdaptor(IUsbPipeIn pipeIn) 
        {
            this._pipeIn = pipeIn ?? throw new ArgumentNullException(nameof(pipeIn));
        }
        #endregion


        #region Fields 

        private readonly IUsbPipeIn _pipeIn;



        #endregion

        #region Properties
        public bool IsByteToRead => _pipeIn.IsByteToRead;

        #endregion


        #region Public Methods 

        public bool ReadBytes(out byte[]? values, uint length, int timeout = -1)
            => _pipeIn.ReadBytes(out values, length, timeout);

        public Task<bool> ReadBytesAsync(out byte[]? values, uint length, CancellationToken cancellationToken, int timeout = -1)
            => _pipeIn.ReadBytesAsync(out values, length, cancellationToken, timeout);

        #endregion


        #region Protected Methods 

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion

    }
}
