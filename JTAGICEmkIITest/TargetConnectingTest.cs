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
    public class TargetConnectingTest : HostServiceBaseTest
    {
        private Mock<IVisitorActivity> _visitorActivityMoq;

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------
        public TargetConnectingTest() : base()
        {
            _visitorActivityMoq = this.MockRepository.Create<IVisitorActivity>();
        }

        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void TargetConnecting_Constructor_Test()
        {
            //--- Setup
            this.CreateHostServiceMoq();
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
            this.CreateHostServiceMoq();
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
            var host = this.CreateHostService();
            var hostSession = host.HostSession;
            var test = hostSession.TargetConnecting;

            _activityStructure.CurrentActivity = test;

            //--- Expectations
            Assert.False(_activityStructure.TargetMcuState.IsStopped);

            //--- Action
            var result = RunTaskActivity(test, typeof(TargetStopped), null!, () => hostSession.IsSessionActive);

            //--- Verification
            Assert.True(result);
            Assert.True(hostSession.IsSessionActive);
            Assert.True(_activityStructure.CurrentActivity is TargetStopped);
            Assert.True(_activityStructure.TargetMcuState.IsStopped);
        }

        #endregion


        #region Private Methods -----------------------------------------------

        #endregion


        #region Private Classes -----------------------------------------------

        #endregion

    }
}

