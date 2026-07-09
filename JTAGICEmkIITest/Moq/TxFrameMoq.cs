using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json.Linq;
using Windows.Foundation.Collections;


namespace JTAGICEmkIITest.Moq
{
    internal class TxFrameMoq : ITxFrameAdaptorBuffer
    {

        #region Constructors 
        public TxFrameMoq() : base()
        {
            _buffer = new List<byte>();
            _position = 0;
        }

        public TxFrameMoq(byte[] initialBuffer) : base()
        {
            _buffer = new List<byte>(initialBuffer);
            _position = 0;
        }

        #endregion


        #region Fields 

        private List<Byte> _buffer;
        private int _position;

        #endregion


        #region Properties 

        public byte[] Buffer => _buffer.ToArray();

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 

        #endregion


        #region Protected Methods 

        public int SendBytes(byte[] values)
        {
            return SendBytesAsync(values, CancellationToken.None).GetAwaiter().GetResult();
        }

        public Task<int> SendBytesAsync(byte[] values, CancellationToken cancellationToken)
        {
            _buffer.AddRange(values);
            _position += values.Length;
            return Task.FromResult(values.Length);
        }

        int ITxFrameAdaptor.SendBytes(byte[] values)
            => this.SendBytes(values);

        Task<int> ITxFrameAdaptor.SendBytesAsync(byte[] values, CancellationToken cancellationToken)
            => this.SendBytesAsync(values, cancellationToken);

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion


    }
}
