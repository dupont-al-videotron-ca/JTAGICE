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
    internal class TxFrameMoq : TxFrame
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

        protected internal override int SendBytes(byte[] values)
        {
            return SendBytesAsync(values, CancellationToken.None).GetAwaiter().GetResult();
        }

        protected internal override Task<int> SendBytesAsync(byte[] values, CancellationToken cancellationToken)
        {
            _buffer.AddRange(values);
            _position+= values.Length;
            return Task.FromResult(values.Length);
        }

        #endregion

        #region Private Methods 

        #endregion

        #region Private Classes / Enum 

        #endregion


    }
}
