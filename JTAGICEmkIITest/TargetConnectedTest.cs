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
    public class TargetConnectedTest : HostServiceBaseTest
    {
        private readonly Mock<IVisitorActivity> _visitorActivityMoq;

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------
        public TargetConnectedTest() : base()
        {
            _visitorActivityMoq = this.MockRepository.Create<IVisitorActivity>();
        }

        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void TargetConnected_Constructor_Test()
        {
            //--- Setup
            this.CreateHostServiceMoq();
            var parent = new ActivityInitial(_activityStructure);
            var next = new ActivityBaseMoq(_activityStructure);

            //--- Expectations

            //--- Action
            var test = new TargetConnected(_activityStructure, parent);
            test.AddNext(next);

            //--- Verification
            Assert.Equal(0x06, _activityStructure.Count);
        }

        [Fact]
        public void TargetConnected_shall_Accept_visitorActivity()
        {
            //--- Setup
            this.CreateHostServiceMoq();
            var test = new TargetConnected(_activityStructure, null);
            //--- Expectations
            _visitorActivityMoq.Setup(m => m.Visit(It.IsAny<TargetConnected>())).Returns(true);

            //--- Action
            var result = test.Accept(_visitorActivityMoq.Object);

            //--- Verification
            Assert.True(result);
        }

        [Fact]
        public void TargetConnected_RunActivity_shall_change_next_activity_to_TargetStopped()
        {
            //--- Setup
            var host = this.CreateHostService();
            var hostSession = host.HostSession;
            var test = hostSession.TargetConnected;

            _activityStructure.CurrentActivity = test;

            //--- Expectations
            Assert.False(_activityStructure.TargetMcuState.IsStopped);

            //--- Action
            var t = RunTaskActivity(test, typeof(TargetStopped), null!);

            //--- Verification
            Assert.True(t);
            Assert.True(_activityStructure.CurrentActivity is TargetStopped);
            Assert.True(_activityStructure.TargetMcuState.IsStopped);

        }

        [Fact]
        public void TargetConnected_RunActivity_shall_change_activity_to_TargetRuning()
        {
            //--- Setup
            var host = this.CreateHostService();
            var hostSession = host.HostSession;
            var test = hostSession.TargetConnected.TargetStopped;
            _activityStructure.TargetMcuState.GoStopped();
            _activityStructure.CurrentActivity = test;

            //--- Expectations
            Assert.True(_activityStructure.TargetMcuState.IsStopped);

            //--- Action
            var t = RunTaskActivity(test, typeof(TargetRunning), () => { return host.StartRunning();});


            //--- Verification
            Assert.True(t);
            Assert.NotNull(_activityStructure.CurrentActivity);
            Assert.True(_activityStructure.CurrentActivity is TargetRunning);
            Assert.True(_activityStructure.TargetMcuState.IsRunning);

        }

        #endregion


        #region Private Methods -----------------------------------------------

        #endregion


        #region Private Classes -----------------------------------------------

        #endregion

    }
}

