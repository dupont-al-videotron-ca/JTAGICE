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
    public class TargetStoppedTest : HostServiceBaseTest
    {
        private Mock<IVisitorActivity> _visitorActivityMoq;

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------
        public TargetStoppedTest() : base()
        {
            _visitorActivityMoq = this.MockRepository.Create<IVisitorActivity>();
        }

        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void TargetStopped_Constructor_Test()
        {
            //--- Setup
            this.CreateHostServiceMoq();
            var parent = new ActivityInitial(_activityStructure);
            var next = new ActivityBaseMoq(_activityStructure);

            //--- Expectations

            //--- Action
            var test = new TargetStopped(_activityStructure, parent);
            test.AddNext(next);

            //--- Verification
            Assert.Equal(0x03, _activityStructure.Count());
        }

        [Fact]
        public void TargetStopped_shall_Accept_visitorActivity()
        {
            //--- Setup
            this.CreateHostServiceMoq();
            var test = new TargetStopped(_activityStructure, null);
            //--- Expectations
            _visitorActivityMoq.Setup(m => m.Visit(It.IsAny<TargetStopped>())).Returns(true);

            //--- Action
            var result = test.Accept(_visitorActivityMoq.Object);

            //--- Verification
            Assert.True(result);
        }

        [Fact]
        public void TargetStopped_RunActivity_shall_change_TargetMcuState_to_Stopped()
        {
            //--- Setup
            var host = this.CreateHostService();
            var hostSession = host.HostSession;
            var initial = new ActivityInitial(_activityStructure);
            var test = new TargetStopped(_activityStructure);
            initial.AddNext(test);
            _activityStructure.CurrentActivity = initial;
            Assert.False(_activityStructure.TargetMcuState.IsStopped);

            //--- Expectations

            //--- Action
            var t = RunTaskActivity(initial, typeof(TargetStopped), null!);

            //--- Verification
            Assert.True(t);
            Assert.NotNull(_activityStructure.CurrentActivity);
            Assert.IsType<TargetStopped>(_activityStructure.CurrentActivity);
            Assert.True(_activityStructure.TargetMcuState.IsStopped);

        }

        [Fact]
        public void TargetStopped_RunActivity_shall_change_activity_to_TargetRunning_on_StartRunning()
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
            var t = RunTaskActivity(test, typeof(TargetRunning), () => { return host.StartRunning(); });


            //--- Verification
            Assert.True(t);
            Assert.NotNull(_activityStructure.CurrentActivity);
            Assert.True(_activityStructure.CurrentActivity is TargetRunning);
            Assert.True(_activityStructure.TargetMcuState.IsRunning);

        }

        [Fact]
        public void TargetStopped_RunActivity_shall_change_activity_to_TargetRunning_on_StartRunningUntil()
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
            var t = RunTaskActivity(test, typeof(TargetRunning), () => { return host.StartRunningUntil(0x12345678); });


            //--- Verification
            Assert.True(t);
            Assert.NotNull(_activityStructure.CurrentActivity);
            Assert.True(_activityStructure.CurrentActivity is TargetRunning);
            Assert.True(_activityStructure.TargetMcuState.IsRunning);

        }

        [Fact]
        public void TargetStopped_RunActivity_shall_change_activity_to_TargetRunning_on_SetBreakpoint()
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
            var t = RunTaskActivity(test, typeof(TargetStopped), () => { return host.SetBreakpoint(0,0x12345678,1,1); });


            //--- Verification
            Assert.True(t);
            Assert.NotNull(_activityStructure.CurrentActivity);
            Assert.True(_activityStructure.CurrentActivity is TargetStopped);
            Assert.True(_activityStructure.TargetMcuState.IsStopped);

        }

        [Fact]
        public void TargetStopped_RunActivity_shall_change_activity_to_TargetRunning_on_SingleStepIntoAsm()
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
            var t = RunTaskActivity(test, typeof(TargetRunning), () => { return host.SingleStepIntoAsm(); });


            //--- Verification
            Assert.True(t);
            Assert.NotNull(_activityStructure.CurrentActivity);
            Assert.True(_activityStructure.CurrentActivity is TargetRunning);
            Assert.True(_activityStructure.TargetMcuState.IsRunning);

        }

        [Fact]
        public void TargetStopped_RunActivity_shall_change_activity_to_TargetRunning_on_SingleStepOutAsm()
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
            var t = RunTaskActivity(test, typeof(TargetRunning), () => { return host.SingleStepOutAsm(); });


            //--- Verification
            Assert.True(t);
            Assert.NotNull(_activityStructure.CurrentActivity);
            Assert.True(_activityStructure.CurrentActivity is TargetRunning);
            Assert.True(_activityStructure.TargetMcuState.IsRunning);

        }


        [Fact]
        public void TargetStopped_RunActivity_shall_change_activity_to_TargetRunning_on_SingleStepOverAsm()
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
            var t = RunTaskActivity(test, typeof(TargetRunning), () => { return host.SingleStepOverAsm(); });


            //--- Verification
            Assert.True(t);
            Assert.NotNull(_activityStructure.CurrentActivity);
            Assert.True(_activityStructure.CurrentActivity is TargetRunning);
            Assert.True(_activityStructure.TargetMcuState.IsRunning);

        }

        [Fact (Skip = "Fix race condition with TargetStopped and TargetProgramming")]
        public void TargetStopped_RunActivity_shall_change_activity_to_TargetRunning_on_EnterPrograming()
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
            var t = RunTaskActivity(test, typeof(TargetProgramming), () => { return host.EnterPrograming(); });


            //--- Verification
            Assert.True(t);
            Assert.NotNull(_activityStructure.CurrentActivity);
            Assert.True(_activityStructure.CurrentActivity is TargetProgramming);
            Assert.True(_activityStructure.TargetMcuState.IsProgramming);

        }

        [Fact(Skip = "Fix race condition with HostSession and TargetDisonnecting")]
        public void TargetStopped_RunActivity_shall_change_activity_to_TargetRunning_on_RestoreTarget()
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
            var t = RunTaskActivity(test, typeof(TargetDisonnecting), () => { return host.RestoreTarget(); });

            //--- Verification
            Assert.True(t);
            Assert.NotNull(_activityStructure.CurrentActivity);
            Assert.True(_activityStructure.CurrentActivity is TargetDisonnecting);
            Assert.True(_activityStructure.TargetMcuState.IsReset);

        }
        #endregion

    }
}

