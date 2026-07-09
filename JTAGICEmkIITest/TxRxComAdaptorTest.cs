using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII;
using JTAGICEmkII.Adaptor;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using JTAGICEmkIITest.Moq;
using MyFramework;
using MyFramework.Queue;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JTAGICEmkIITest
{
    public class TxRxComAdaptorTest : XUnitTestBase
    {

        private TxRxComAdaptor _txRxComAdaptor;
        public TxRxComAdaptorTest() : base()
        {
            _txRxComAdaptor = null!;
        }

        override protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _txRxComAdaptor?.Dispose();
            }
            base.Dispose(disposing);
        }

        [Fact]
        public void TxRxComAdaptor_Shall_Open_and_Close_succesffuly()
        {
            var txFrameAdaptor = CreateTxRxComAdaptor();
            Assert.False(txFrameAdaptor.IsOpen);
            txFrameAdaptor.Open();
            Assert.True(txFrameAdaptor.IsOpen);
            txFrameAdaptor.Close();
            Assert.False(txFrameAdaptor.IsOpen);
        }

        [Fact]
        public void TxRxComAdaptor_Shall_Write_succesffuly()
        {
            var txFrameAdaptor = CreateTxRxComAdaptor(true);
            Assert.True(txFrameAdaptor.IsOpen);

            var sendBytes = new byte[] { 0x01, 0x02, 0x03 };
            var sendResult = txFrameAdaptor.SendBytes(sendBytes);
            Assert.Equal(sendBytes.Length, sendResult);
        }

        [Fact]
        public async Task TxRxComAdaptor_Shall_WriteAsync_succesffuly()
        {
            var txFrameAdaptor = CreateTxRxComAdaptor(true);
            Assert.True(txFrameAdaptor.IsOpen);

            var sendBytes = new byte[] { 0x01, 0x02, 0x03 };
            int sendResult = await txFrameAdaptor.SendBytesAsync(sendBytes, CancellationToken.None);

            Assert.Equal(sendBytes.Length, sendResult);
        }

        [Fact]
        public void TxRxComAdaptor_Shall_Read_failed()
        {
            var txFrameAdaptor = CreateTxRxComAdaptor(true);
            Assert.True(txFrameAdaptor.IsOpen);

            var sendBytes = new byte[] { 0x01, 0x02, 0x03 };
            var result = txFrameAdaptor.ReadBytes(out byte[]? expectedResult, (uint) sendBytes.Length, 1000);
            Assert.False(result);
            Assert.Null(expectedResult);
        }

        [Fact]
        public void TxRxComAdaptor_Shall_Read_Succefful()
        {
            var txFrameAdaptor = CreateTxRxComAdaptor(true);
            Assert.True(txFrameAdaptor.IsOpen);

            var sendBytes = new byte[] { 0x01, 0x02, 0x03 };
            txFrameAdaptor.SendBytes(sendBytes);

            var result = txFrameAdaptor.ReadBytes(out byte[]? expectedResult, (uint)sendBytes.Length, 10000);
            Assert.True(result);
            Assert.Equal(sendBytes, expectedResult);
        }

        [Fact]
        public void TxRxComAdaptor_Shall_Read_with_timeout()
        {
            var txFrameAdaptor = CreateTxRxComAdaptor(true);
            Assert.True(txFrameAdaptor.IsOpen);

            var sendBytes = new byte[] { 0x01, 0x02, 0x03 };
            var result = txFrameAdaptor.ReadBytes(out byte[]? expectedResult, (uint)sendBytes.Length, 1000);
            Assert.False(result);
            Assert.Null(expectedResult);
        }

        [Fact]
        public async Task TxRxComAdaptor_Shall_ReadAsync_succesffuly()
        {
            var txFrameAdaptor = CreateTxRxComAdaptor(true);
            Assert.True(txFrameAdaptor.IsOpen);

            var sendBytes = new byte[] { 0x01, 0x02, 0x03 };
            var result = await txFrameAdaptor.ReadBytesAsync(out byte[]? expectedResult, (uint)sendBytes.Length, CancellationToken.None, 1000);

            Assert.False(result);
            Assert.Null(expectedResult);
        }

        private TxRxComAdaptor CreateTxRxComAdaptor(bool open = false)
        {
            _txRxComAdaptor = new TxRxComAdaptor(new SerialConfig(), open);
            return _txRxComAdaptor;
        }

    }
}
