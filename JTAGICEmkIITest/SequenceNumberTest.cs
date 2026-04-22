using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test;
using Common.Test.Xunit;
using Xunit;

namespace JTAGICEmkIITest
{
    public class SequenceNumberTest : XUnitTestBase
    {


        [Fact]
        public void SequenceNumber_default_constructeur()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber();
            Assert.True(sequenceNumber.IsInitial);
            Assert.Equal(-1, sequenceNumber.NumberValue);
            Assert.Equal(0xFFFF, sequenceNumber.SequenceNumberModulo);
            Assert.Equal(0xFFFE, sequenceNumber.SequenceNumberWrap);
        }

        [Fact]
        public void SequenceNumber_startValue_constructeur()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(5);
            Assert.False(sequenceNumber.IsInitial);
            Assert.Equal(5, sequenceNumber.NumberValue);
            Assert.Equal(0xFFFF, sequenceNumber.SequenceNumberModulo);
            Assert.Equal(0xFFFE, sequenceNumber.SequenceNumberWrap);
        }

        [Fact]
        public void SequenceNumber_minusOne_constructeur()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(-1);
            Assert.True(sequenceNumber.IsInitial);
            Assert.Equal(-1, sequenceNumber.NumberValue);
            Assert.Equal(0xFFFF, sequenceNumber.SequenceNumberModulo);
            Assert.Equal(0xFFFE, sequenceNumber.SequenceNumberWrap);
        }

        [Fact]
        public void SequenceNumber_copy_constructeur()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(5);
            var copy = new JTAGICEmkII.Frame.SequenceNumber(sequenceNumber);
            Assert.False(copy.IsInitial);
            Assert.Equal(5, copy.NumberValue);
            Assert.Equal(0xFFFF, copy.SequenceNumberModulo);
            Assert.Equal(0xFFFE, copy.SequenceNumberWrap);
        }

        [Fact]
        public void SequenceNumber_shall_return_GetUInt16LittleEndian()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(5);
            var result = sequenceNumber.GetUInt16LittleEndian();
            Assert.Equal(2, result.Length);
            Assert.Equal(5, result[0]);
            Assert.Equal(0, result[1]);
        }

        [Fact]
        public void SequenceNumber_shall_return_Difference()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(5);
            var result = sequenceNumber.Difference(3);
            Assert.Equal(2, result);

            result = sequenceNumber.Difference(7);
            Assert.Equal(-2, result);

            result = sequenceNumber.Difference(5);
            Assert.Equal(0, result);
        }

        [Fact]
        public void SequenceNumber_Equals_shall_return_true()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(5);
            var sequenceNumber2 = new JTAGICEmkII.Frame.SequenceNumber(5);

            Assert.True(sequenceNumber.Equals(sequenceNumber2));

            Object obj = sequenceNumber;
            Object obj2 = sequenceNumber2;
            Assert.True(obj.Equals(obj2));

        }

        [Fact]
        public void SequenceNumber_Equals_shall_return_false()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(5);
            var sequenceNumber2 = new JTAGICEmkII.Frame.SequenceNumber(6);

            Assert.False(sequenceNumber.Equals(sequenceNumber2));

            Object obj = sequenceNumber;
            Object obj2 = sequenceNumber2;
            Assert.False(obj.Equals(obj2));

        }

        [Fact]
        public void SequenceNumber_Equals_shall_return_false_for_null()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(5);

            Assert.False(sequenceNumber.Equals(null));

            Object obj = sequenceNumber;
            Assert.False(obj.Equals(null));

        }

        [Fact]
        public void SequenceNumber_GetHashCode_shall_return_value()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(5);

            Assert.Equal(5, sequenceNumber.GetHashCode());

        }

        [Theory]
        [InlineData(2, 2, 0)]
        [InlineData(2, 3, -1)]
        [InlineData(3, 2, 1)]
        public void SequenceNumber_CompareTo_shall_return_expectingResult(int a, int b, int expectingResult)
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(a);
            var sequenceNumber2 = new JTAGICEmkII.Frame.SequenceNumber(b);

            Assert.Equal(expectingResult, sequenceNumber.CompareTo(sequenceNumber2));

        }

        [Theory]
        [InlineData(2, 2, false)]
        [InlineData(2, 3, false)]
        [InlineData(3, 2, true)]
        public void SequenceNumber_GreaterThan_shall_return_expectingResult(int a, int b, bool expectingResult)
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(a);
            var sequenceNumber2 = new JTAGICEmkII.Frame.SequenceNumber(b);

            Assert.Equal(expectingResult, sequenceNumber > sequenceNumber2);

        }

        [Theory]
        [InlineData(2, 2, true)]
        [InlineData(2, 3, false)]
        [InlineData(3, 2, true)]
        public void SequenceNumber_GreaterOrEqualThan_shall_return_expectingResult(int a, int b, bool expectingResult)
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(a);
            var sequenceNumber2 = new JTAGICEmkII.Frame.SequenceNumber(b);

            Assert.Equal(expectingResult, sequenceNumber >= sequenceNumber2);

        }

        [Theory]
        [InlineData(2, 2, false)]
        [InlineData(2, 3, true)]
        [InlineData(3, 2, false)]
        public void SequenceNumber_LessThan_shall_return_expectingResult(int a, int b, bool expectingResult)
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(a);
            var sequenceNumber2 = new JTAGICEmkII.Frame.SequenceNumber(b);

            Assert.Equal(expectingResult, sequenceNumber < sequenceNumber2);

        }

        [Theory]
        [InlineData(2, 2, true)]
        [InlineData(2, 3, true)]
        [InlineData(3, 2, false)]
        public void SequenceNumber_LessOrEqualThan_shall_return_expectingResult(int a, int b, bool expectingResult)
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(a);
            var sequenceNumber2 = new JTAGICEmkII.Frame.SequenceNumber(b);

            Assert.Equal(expectingResult, sequenceNumber <= sequenceNumber2);

        }

        [Theory]
        [InlineData(2, 2, true)]
        [InlineData(2, 3, false)]
        [InlineData(3, 2, false)]
        public void SequenceNumber_EqualThan_shall_return_expectingResult(int a, int b, bool expectingResult)
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(a);
            var sequenceNumber2 = new JTAGICEmkII.Frame.SequenceNumber(b);

            Assert.Equal(expectingResult, sequenceNumber == sequenceNumber2);

        }

        [Theory]
        [InlineData(2, 2, false)]
        [InlineData(2, 3, true)]
        [InlineData(3, 2, true)]
        public void SequenceNumber_NotEqualThan_shall_return_expectingResult(int a, int b, bool expectingResult)
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(a);
            var sequenceNumber2 = new JTAGICEmkII.Frame.SequenceNumber(b);

            Assert.Equal(expectingResult, sequenceNumber != sequenceNumber2);
        }


        [Fact]
        public void SequenceNumber_post_increment_shall_return_expectingResult()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(2);
            var sequenceNumber2 = sequenceNumber++;

            Assert.NotSame(sequenceNumber, sequenceNumber2);
            Assert.Equal(3, sequenceNumber.NumberValue);
            Assert.Equal(2, sequenceNumber2.NumberValue);
        }

        [Fact]
        public void SequenceNumber_post_decrement_shall_return_expectingResult()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(2);
            var sequenceNumber2 = sequenceNumber--;

            Assert.NotSame(sequenceNumber, sequenceNumber2);
            Assert.Equal(2, sequenceNumber2.NumberValue);
            Assert.Equal(1, sequenceNumber.NumberValue);

            sequenceNumber--;
            Assert.Equal(0, sequenceNumber.NumberValue);

        }

        [Fact]
        public void SequenceNumber_pre_increment_shall_return_expectingResult()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(2);
            var sequenceNumber2 = ++sequenceNumber;

            Assert.Same(sequenceNumber, sequenceNumber2);
            Assert.Equal(3, sequenceNumber2.NumberValue);
            Assert.Equal(3, sequenceNumber.NumberValue);

            ++sequenceNumber;
            Assert.Equal(4, sequenceNumber.NumberValue);

        }

        [Fact]
        public void SequenceNumber_pre_decrement_shall_return_expectingResult()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(2);
            var sequenceNumber2 = --sequenceNumber;

            Assert.Same(sequenceNumber, sequenceNumber2);
            Assert.Equal(1, sequenceNumber2.NumberValue);
            Assert.Equal(1, sequenceNumber.NumberValue);

            --sequenceNumber;
            Assert.Equal(0, sequenceNumber.NumberValue);

        }

        [Fact]
        public void SequenceNumber_implicit_operator_SequenceNumber_shall_work()
        {
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(2);

            UInt16 result = sequenceNumber;
            Assert.Equal(2, result);
        }

        [Fact]
        public void SequenceNumber_explicit_operator_UInt16_shall_work()
        {
            UInt16 i = 2;
            var sequenceNumber = (JTAGICEmkII.Frame.SequenceNumber)i;
            Assert.Equal(2, sequenceNumber.NumberValue);
        }

        [Theory]
        [InlineData(0x0000, 0x0001, 0x0000 - 0x0001)]
        [InlineData(0xFFFE, 0x0000, 0xFFFE - 0x0000)]
        [InlineData(0xFFFD, 0xFFFE, 0xFFFD - 0xFFFE)]
        [InlineData(0x0000, 0x0002, 0x0000 - 0x0002)]
        [InlineData(0x0005, 0x0004, 0x0005 - 0x0004)]
        [InlineData(0x0005, 0x0005, 0x0005 - 0x0005)]
        [InlineData(0x0005, 0xFFFE, 0x0005 - 0xFFFE)]
        public void SequenceNumber_Difference_shall_return_expectingResult(int previousSequence, UInt16 frameSequence, int expectedSequence)
        {

            short expectedSequenceShort = (short)expectedSequence;
            var sequenceNumber = new JTAGICEmkII.Frame.SequenceNumber(previousSequence);

            short result = sequenceNumber.Difference(frameSequence);

            Assert.Equal(expectedSequenceShort, result);
        }

    }
}
