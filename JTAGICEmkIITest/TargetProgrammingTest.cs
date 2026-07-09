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
    public class TargetProgrammingTest : HostServiceBaseTest
    {
        private readonly Mock<IVisitorActivity> _visitorActivityMoq;

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------
        public TargetProgrammingTest() : base()
        {
            _visitorActivityMoq = this.MockRepository.Create<IVisitorActivity>();
        }

        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void TargetProgramming_Constructor_Test()
        {
            //--- Setup
            this.CreateHostServiceMoq();
            var parent = new ActivityInitial(_activityStructure);
            var next = new ActivityBaseMoq(_activityStructure);

            //--- Expectations

            //--- Action
            var test = new TargetProgramming(_activityStructure, parent);
            test.AddNext(next);

            //--- Verification
            Assert.Equal(0x03, _activityStructure.Count);
        }

        [Fact]
        public void TargetProgramming_shall_Accept_visitorActivity()
        {
            //--- Setup
            this.CreateHostServiceMoq();
            var test = new TargetProgramming(_activityStructure, null);
            //--- Expectations
            _visitorActivityMoq.Setup(m => m.Visit(It.IsAny<TargetProgramming>())).Returns(true);

            //--- Action
            var result = test.Accept(_visitorActivityMoq.Object);

            //--- Verification
            Assert.True(result);
        }

        // TODO: Find out the best way to programme memory.
        // Add more tests for TargetProgramming ActivityAction and its interactions with the HostService and TargetMcuState.

        //[Fact]
        //public void TargetProgramming_RunActivity_shall_change_TargetMcuState_to_Running()
        //{
        //    //--- Setup
        //    var host = this.CreateHostService();
        //    var hostSession = host.HostSession;
        //    var test = new TargetProgramming(_activityStructure);
        //    _activityStructure.CurrentActivity = test;
        //    Assert.False(_activityStructure.TargetMcuState.IsStopped);

        //    //--- Expectations

        //    //--- Action
        //    var result = RunTaskActivity(test, typeof(TargetProgramming), null!, () => _activityStructure.TargetMcuState.IsRunning);

        //    //--- Verification
        //    Assert.True(result);
        //    Assert.NotNull(_activityStructure.CurrentActivity);
        //    Assert.True(_activityStructure.TargetMcuState.IsRunning);

        //}

        //[Fact]
        //public void TargetProgramming_RunActivity_shall_change_activity_to_TargetProgramming_on_Reset()
        //{
        //    //--- Setup
        //    var host = this.CreateHostService();
        //    var hostSession = host.HostSession;
        //    var test = hostSession.TargetConnected.TargetProgramming;
        //    _activityStructure.TargetMcuState.GoRunning();
        //    _activityStructure.CurrentActivity = test;

        //    //--- Expectations
        //    Assert.True(_activityStructure.TargetMcuState.IsRunning);

        //    //--- Action
        //    var result = RunTaskActivity(test, typeof(TargetStopped), () => { return host.Reset(); });


        //    //--- Verification
        //    Assert.True(result);
        //    Assert.NotNull(_activityStructure.CurrentActivity);
        //    Assert.True(_activityStructure.CurrentActivity is TargetStopped);
        //    Assert.True(_activityStructure.TargetMcuState.IsStopped);

        //}

        //[Fact]
        //public void TargetProgramming_RunActivity_shall_change_activity_to_TargetProgramming_on_GetSync()
        //{
        //    //--- Setup
        //    var host = this.CreateHostService();
        //    var hostSession = host.HostSession;
        //    var test = hostSession.TargetConnected.TargetProgramming;
        //    _activityStructure.TargetMcuState.GoRunning();
        //    _activityStructure.CurrentActivity = test;

        //    //--- Expectations
        //    Assert.True(_activityStructure.TargetMcuState.IsRunning);

        //    //--- Action
        //    var result = RunTaskActivity(test, typeof(TargetStopped), () => { return host.GetSync(); });


        //    //--- Verification
        //    Assert.True(result);
        //    Assert.NotNull(_activityStructure.CurrentActivity);
        //    Assert.True(_activityStructure.CurrentActivity is TargetStopped);
        //    Assert.True(_activityStructure.TargetMcuState.IsStopped);

        //}

        //[Fact]
        //public void TargetProgramming_RunActivity_shall_change_activity_to_TargetProgramming_on_StopRunning()
        //{
        //    //--- Setup
        //    var host = this.CreateHostService();
        //    var hostSession = host.HostSession;
        //    var test = hostSession.TargetConnected.TargetProgramming;
        //    _activityStructure.TargetMcuState.GoRunning();
        //    _activityStructure.CurrentActivity = test;

        //    //--- Expectations
        //    Assert.True(_activityStructure.TargetMcuState.IsRunning);

        //    //--- Action
        //    var result = RunTaskActivity(test, typeof(TargetStopped), () => { return host.StopRunning(); });


        //    //--- Verification
        //    Assert.True(result);
        //    Assert.NotNull(_activityStructure.CurrentActivity);
        //    Assert.True(_activityStructure.CurrentActivity is TargetStopped);
        //    Assert.True(_activityStructure.TargetMcuState.IsStopped);

        //}

        //[Fact]
        //public void TargetProgramming_RunActivity_shall_change_activity_to_TargetProgramming_on_BreakEvent()
        //{
        //    //--- Setup
        //    var host = this.CreateHostService();
        //    var hostSession = host.HostSession;
        //    var test = hostSession.TargetConnected.TargetProgramming;
        //    _activityStructure.TargetMcuState.GoRunning();
        //    _activityStructure.CurrentActivity = test;

        //    //--- Expectations
        //    Assert.True(_activityStructure.TargetMcuState.IsRunning);

        //    //--- Action
        //    var result = RunTaskActivity(test, typeof(TargetStopped), () => {
        //        this.CreateAndSendEvent(SlaveResponseEnum.EVT_BREAK);
        //        return CommandResult.Success; });

        //    //--- Verification
        //    Assert.True(result);
        //    Assert.NotNull(_activityStructure.CurrentActivity);
        //    Assert.True(_activityStructure.CurrentActivity is TargetStopped);
        //    Assert.True(_activityStructure.TargetMcuState.IsStopped);

        //}

        #endregion

    }
}

