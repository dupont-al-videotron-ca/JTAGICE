using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII.HostService;
using JTAGICEmkIITest.Moq;
using Moq;
using Xunit;

namespace JTAGICEmkIITest
{
    public class ActivityInitialTest : ActivityBaseTest
    {
        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------

        public ActivityInitialTest() : base()
        {
        }

        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void ActivityInitial_Shall_Constructor_with_no_parent()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);

            //--- Expectations

            //--- Action
            var test = new ActivityInitial(activityStructure);

            //--- Verification
            Assert.Null(test.Parent);
            Assert.Empty(test.Parents);
        }

        [Fact]
        public void ActivityInitial_Shall_Constructor_with_parent()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityInitial(activityStructure);
            //--- Expectations

            //--- Action
            var test = new ActivityInitial(activityStructure, parent);

            //--- Verification
            Assert.NotNull(test.Parent);
            Assert.Single(test.Parents);
            Assert.Same(parent, test.Parent);
        }

        [Fact]
        public void ActivityInitial_Shall_call_the_visitor()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityInitial(activityStructure);

            //--- Expectations
            _visitorActivityMoq.Setup(m => m.Visit(It.IsAny<ActivityInitial>())).Returns(true);

            //--- Action
            var result = test.Accept(_visitorActivityMoq.Object);

            //--- Verification
            Assert.True(result);
        }

        [Fact]
        public void ActivityInitial_call_ActivityEntry_shall_return_true()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityInitial(activityStructure);
            var child = new ActivityBaseMoq(activityStructure);
            test.AddNext(child);
            test._nextIndex = 0;

            //--- Expectations

            //--- Action
            var result = test.ActivityEntry();

            //--- Verification
            Assert.True(result);
            Assert.Same(child, activityStructure.CurrentActivity);
        }

        [Fact]
        public void ActivityInitial_call_ActivityEntry_shall_throw_exception_when_no_child()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityInitial(activityStructure);
            var child = new ActivityBaseMoq(activityStructure);

            //--- Expectations

            //--- Action
            Assert.Throws<InvalidOperationException>(() => test.ActivityEntry());

            //--- Verification
            this.VerifyLogForError = false;
            var eventLog = this.LoggedEvents.FirstOrDefault(e => e.Level == log4net.Core.Level.Error);
            Assert.NotNull(eventLog);
            Assert.Equal("Activity JTAGICEmkII.HostService.ActivityInitial has no child activity to enter.", eventLog.RenderedMessage);

        }

        [Fact]
        public void ActivityInitial_call_ActivityEntry_shall_throw_exception_when_too_many_children()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityInitial(activityStructure);
            var child = new ActivityBaseMoq(activityStructure);
            test.AddNext(child);
            test.AddNext(child);
            //--- Expectations

            //--- Action
            Assert.Throws<NotImplementedException>(() => test.ActivityEntry());

            //--- Verification
            this.VerifyLogForError = false;
            var eventLog = this.LoggedEvents.FirstOrDefault(e => e.Level == log4net.Core.Level.Error);
            Assert.NotNull(eventLog);
            Assert.Equal("Activity JTAGICEmkII.HostService.ActivityInitial has multiple child activities, which is not supported yet.", eventLog.RenderedMessage);

        }

        [Fact]
        public void ActivityInitial_call_ActivityExit_shall_return_true()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityInitial(activityStructure);
            var child = new ActivityBaseMoq(activityStructure);
            test.AddNext(child);
            test._nextIndex = 0;

            //--- Expectations

            //--- Action
            var result = test.ActivityExit(true);

            //--- Verification
            Assert.True(result);
            Assert.Same(child, activityStructure.CurrentActivity);

        }
        
        #endregion

    }
}
