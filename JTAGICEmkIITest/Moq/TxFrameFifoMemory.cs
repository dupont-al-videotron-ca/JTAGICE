using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using MyFramework;

namespace JTAGICEmkIITest.Moq
{
    internal class TxFrameFifoMemory : ITxFrameAdaptor
    {

        #region Constructors 
        public TxFrameFifoMemory(FifoBuffer<byte> FifoBuffer) : base()
        {
            this._fifoBuffer = FifoBuffer;
        }


        #endregion

        #region Fields 

        private FifoBuffer<byte> _fifoBuffer;
        public byte[] Buffer => _fifoBuffer.ToArray();

        #endregion

        #region Protected Methods 

        internal int SendBytes(byte[] values)
        {
            return SendBytesAsync(values, CancellationToken.None).GetAwaiter().GetResult();
        }

        internal Task<int> SendBytesAsync(byte[] values, CancellationToken cancellationToken)
        {
            Task<int> task = Task.Run(() => _fifoBuffer.In(values), cancellationToken);
            task.Wait();
            return Task.FromResult(values.Length);
        }

        int ITxFrameAdaptor.SendBytes(byte[] values) 
            => this.SendBytes(values);

        Task<int> ITxFrameAdaptor.SendBytesAsync(byte[] values, CancellationToken cancellationToken) 
            => this.SendBytesAsync(values, cancellationToken);

        #endregion

    }
}
