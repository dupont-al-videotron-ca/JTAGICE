using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using MyFramework;
using Xunit;

namespace MyFrameworkTest
{
    public class FifoBufferTest : XUnitTestBase
    {
        private int[] expectedData;

        public FifoBufferTest() : base()
        {
            expectedData = new int[] { 1, 2, 3 };
        }


        [Fact]
        public void FifoBuffer_Should_initialize_with_given_item()
        {
            FifoBuffer<int> fifo = new FifoBuffer<int>(5);
            Assert.Equal(4, fifo.Capacity);
            Assert.False(fifo.IsEmpty);
            Assert.Equal(5, fifo.ToArray()[0]);
        }

        [Fact]
        public void FifoBuffer_Should_initialize_with_given_items()
        {
            FifoBuffer<int> fifo = new FifoBuffer<int>(expectedData);
            Assert.Equal(expectedData.Length+1, fifo.Capacity);
            Assert.False(fifo.IsEmpty);
            Assert.Equal(expectedData, fifo.ToArray());
        }

        [Fact]
        public void FifoBuffer_In_should_add_items_to_buffer()
        {
            FifoBuffer<int> fifo = new FifoBuffer<int>();
            int nb = fifo.In(expectedData);
            Assert.Equal(expectedData.Length, nb);
            Assert.Equal(expectedData.Length, fifo.Count);
            Assert.Equal(expectedData, fifo.ToArray());
        }

        [Fact]
        public void FifoBuffer_In_should_add_item_to_buffer()
        {
            FifoBuffer<int> fifo = new FifoBuffer<int>();
            int nb = fifo.In(expectedData[0]);
            Assert.Equal(1, nb);
            Assert.NotEmpty(fifo);
            Assert.Equal(new int[] { expectedData[0] }, fifo.ToArray());
        }

        [Fact]
        public void FifoBuffer_Out_should_remove_items_from_buffer()
        {
            FifoBuffer<int> fifo = new FifoBuffer<int>(expectedData);            

            var result = fifo.Out(out int[] output , expectedData.Length);
            Assert.True(result);
            Assert.Equal(expectedData.Length, output.Length);
            Assert.Equal(expectedData, output);
        }

        [Fact]
        public void FifoBuffer_Out_should_timeout()
        {
            FifoBuffer<int> fifo = new FifoBuffer<int>();

            var result = fifo.Out(out int[] output, expectedData.Length, 100);
            Assert.False(result);
            Assert.Null(output);
            
            result = fifo.Out(out int output2, 100);
            Assert.False(result);
            Assert.Equal(0, output2);
        }

        [Fact]
        public void FifoBuffer_Out_should_timeout2()
        {
            FifoBuffer<int> fifo = new FifoBuffer<int>();

            var result = fifo.Out(out int output, 100);
            Assert.False(result);
            Assert.Equal(0, output);
        }
        [Fact]
        public void FifoBuffer_Out_should_remove_item_from_buffer()
        {
            FifoBuffer<int> fifo = new FifoBuffer<int>(expectedData);

            var result = fifo.Out(out int output);
            Assert.True(result);
            Assert.Equal(expectedData[0], output);
            Assert.False(fifo.IsEmpty);
        }

        [Fact]
        public void FifoBuffer_CanRead_should_return_true()
        {
            FifoBuffer<int> fifo = new FifoBuffer<int>(5);
            Assert.True(fifo.CanRead);
        }

        [Fact]
        public void FifoBuffer_CanSeek_should_return_false()
        {
            FifoBuffer<int> fifo = new FifoBuffer<int>(5);
            Assert.False(fifo.CanSeek);
        }

        [Fact]
        public void FifoBuffer_CanWrite_should_return_true()
        {
            FifoBuffer<int> fifo = new FifoBuffer<int>(5);
            Assert.True(fifo.CanWrite);
        }

    }
}
