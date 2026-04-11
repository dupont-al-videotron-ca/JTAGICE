using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using JTAGICEmkIITest.Moq;
using Xunit;
using MyFramework;

namespace JTAGICEmkIITest
{
    public class RxFrameFifoMemoryTest : RxFrameMoqTest
    {
        public RxFrameFifoMemoryTest() : base()
        {

        }


        protected override RxFrame CreateFrameForTest(byte[] buffer, int timeout = -1)
        {
            FifoBuffer<byte> memory = new FifoBuffer<byte>(buffer);
            RxFrameFifoMemory rxFrameMoq = new RxFrameFifoMemory(memory, timeout);
            RxFrame rxFrame = new RxFrame(rxFrameMoq);
            rxFrameMoq.Attach(rxFrame);

            if (timeout != -1)
                rxFrameMoq.WaitForTimeout = true;
            return rxFrame;
        }
    }
}
