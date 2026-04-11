using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using JTAGICEmkIITest.Moq;
using MyFramework;
using MyFramework.Queue;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JTAGICEmkIITest
{
    public class TxFrameFifoMemoryTest : TxFrameMoqTest
    {

        public TxFrameFifoMemoryTest() : base()
        {
        }

        protected override TxFrame CreateFrameForTest()
        {
            FifoBuffer<byte> memory = new FifoBuffer<byte>();
            TxFrameFifoMemory txFrameMoq = new TxFrameFifoMemory(memory);
            TxFrame txFrame = new TxFrame(txFrameMoq);
            return txFrame;
        }

    }
}
