using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII.HostService;
using JTAGICEmkII.Master;
using Moq;
using Xunit;

namespace JTAGICEmkIITest
{
    public class ParametersTest : XUnitTestBase
    {


        #region Constructors --------------------------------------------------

        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void ParametersTest_Constructor_Test()
        {
            //--- Setup
            
            //--- Expectations

            //--- Action
            var test = new Parameters();

            //--- Verification
            Assert.Equal(39, test.Count);
        }

        [Theory]
        [InlineData(ParameterEnum.PARAM_HWD_VERSION, true)]
        [InlineData(ParameterEnum.PARAM_BAUD_RATE, true)]
        [InlineData(ParameterEnum.PARAM_PROGRAM_ENTRY_POINT, false)]
        [InlineData(ParameterEnum.PARAM_BREAK_ADDR1, true)]
        public void ParametersTest_GetIsRead_shall_return_expected_value(ParameterEnum paramId, bool expectedIsRead)
        {
            //--- Setup
            var test = new Parameters();

            //--- Expectations

            //--- Action
            var result = test.GetIsRead(paramId);

            //--- Verification
            Assert.Equal(expectedIsRead, result);
        }

        [Theory]
        [InlineData(ParameterEnum.PARAM_HWD_VERSION, true)]
        [InlineData(ParameterEnum.PARAM_BAUD_RATE, true)]
        [InlineData(ParameterEnum.PARAM_PROGRAM_ENTRY_POINT, true)]
        [InlineData(ParameterEnum.PARAM_BREAK_ADDR1, false)]
        public void ParametersTest_IsUsed_shall_return_expected_value(ParameterEnum paramId, bool expectedIsUsed)
        {
            //--- Setup
            var test = new Parameters();

            //--- Expectations

            //--- Action
            var result = test.GetIsUsed(paramId);

            //--- Verification
            Assert.Equal(expectedIsUsed, result);
        }

        [Theory]
        [InlineData(ParameterEnum.PARAM_HWD_VERSION, false)]
        [InlineData(ParameterEnum.PARAM_BAUD_RATE, true)]
        [InlineData(ParameterEnum.PARAM_PROGRAM_ENTRY_POINT, true)]
        [InlineData(ParameterEnum.PARAM_BREAK_ADDR1, true)]
        public void ParametersTest_GetIsWrite_shall_return_expected_value(ParameterEnum paramId, bool expectedIsWrite)
        {
            //--- Setup
            var test = new Parameters();

            //--- Expectations

            //--- Action
            var result = test.GetIsWrite(paramId);

            //--- Verification
            Assert.Equal(expectedIsWrite, result);
        }

        [Fact]
        public void ParametersTest_GetAllWrite_shall_return_values()
        {
            //--- Setup
            var test = new Parameters();

            //--- Expectations

            //--- Action
            var result = test.GetAllWrite();

            //--- Verification
            Assert.NotEmpty(result);
            Assert.Equal(26, result.Count);
        }

        [Fact]
        public void ParametersTest_GetAllRead_shall_return_values()
        {
            //--- Setup
            var test = new Parameters();

            //--- Expectations

            //--- Action
            var result = test.GetAllRead();

            //--- Verification
            Assert.NotEmpty(result);
            Assert.Equal(36, result.Count);
        }
        #endregion


        #region Private Methods -----------------------------------------------

        #endregion


        #region Private Classes -----------------------------------------------

        #endregion

    }
}
