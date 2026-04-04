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
    internal class TxFrameMoq : ITxComAdaptor
    {

        #region Constructors 
        public TxFrameMoq() : base()
        {
            _buffer = new List<byte>();
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

        internal int SendBytes(byte[] values)
        {
            return SendBytesAsync(values, CancellationToken.None).GetAwaiter().GetResult();
        }

        internal Task<int> SendBytesAsync(byte[] values, CancellationToken cancellationToken)
        {
            _buffer.AddRange(values);
            _position+= values.Length;
            return Task.FromResult(values.Length);
        }

        int ITxComAdaptor.SendBytes(byte[] values) 
            => this.SendBytes(values);

        Task<int> ITxComAdaptor.SendBytesAsync(byte[] values, CancellationToken cancellationToken) 
            => this.SendBytesAsync(values, cancellationToken);

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion


    }
}
