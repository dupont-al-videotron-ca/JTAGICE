using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.HostService;
using JTAGICEmkII.Slave;
using JTAGICEmkIITest.Moq;
using Moq;
using Xunit;

namespace JTAGICEmkIITest
{
    public class TargetConnectingTest : ActivityBaseTest
    {

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------
        public TargetConnectingTest() : base()
        {

        }

        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void TargetConnecting_Constructor_Test()
        {
            //--- Setup
            var parent = new ActivityInitial(_activityStructure);
            var next = new ActivityBaseMoq(_activityStructure);

            //--- Expectations

            //--- Action
            var test = new TargetConnecting(_activityStructure, parent);
            test.AddNext(next);

            //--- Verification
            Assert.Equal(0x03, _activityStructure.Count());
        }

        [Fact]
        public void TargetConnecting_shall_Accept_visitorActivity()
        {
            //--- Setup
            var test = new TargetConnecting(_activityStructure, null);
            //--- Expectations
            _visitorActivityMoq.Setup(m => m.Visit(It.IsAny<TargetConnecting>())).Returns(true);

            //--- Action
            var result = test.Accept(_visitorActivityMoq.Object);

            //--- Verification
            Assert.True(result);
        }

        [Fact]
        public void TargetConnecting_RunActivity_shall_return_true()
        {
            //--- Setup
            var test = new TargetConnecting(_activityStructure, null);
            var host = new HostSession(_activityStructure);
            var tc = new TargetConnected(_activityStructure);

            _activityStructure.CurrentActivity = test;

            //--- Expectations

            //--- Action
            var result = _activityStructure.RunActivity(test);

            //--- Verification
            Assert.True(result);
            Assert.True(host.IsSessionActive);
            Assert.NotNull(test.NextActivity);
            Assert.True(test.NextActivity is TargetConnected);
        }

        #endregion


        #region Private Methods -----------------------------------------------

        #endregion


        #region Private Classes -----------------------------------------------

        #endregion

    }
}

