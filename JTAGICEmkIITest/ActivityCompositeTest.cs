using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.HostService;
using JTAGICEmkIITest.Moq;
using Moq;
using Xunit;

namespace JTAGICEmkIITest
{
    public class ActivityCompositeTest : ActivityBaseTest
    {
        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------

        public ActivityCompositeTest() : base()
        {
        }

        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void ActivityComposite_Shall_Constructor()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);

            //--- Expectations

            //--- Action
            var test = new ActivityCompositeMoq(activityStructure);

            //--- Verification
            Assert.NotNull(test.Initial);
        }

        [Fact]
        public void ActivityComposite_Shall_Constructor_with_parent()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var initial = new ActivityInitial(activityStructure);
            var parent = new ActivityBaseMoq(activityStructure);
            //--- Expectations

            //--- Action
            var test = new ActivityCompositeMoq(activityStructure, parent);

            //--- Verification
            Assert.NotNull(test.Parent);
            Assert.Single(test.Parents);
            Assert.Same(parent, test.Parent);
        }

        [Fact]
        public void ActivityComposite_Shall_call_the_visitor()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityCompositeMoq(activityStructure);

            //--- Expectations

            //--- Action
            var result = test.Accept(_visitorActivityMoq.Object);

            //--- Verification
            Assert.True(result);
        }

        [Fact(Skip = "Skipped To find ActivityEntry not implemented.")]
        public void ActivityComposite_call_ActivityEntry_shall_return_true()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityCompositeMoq(activityStructure);
            var child = new ActivityBaseMoq(activityStructure);
            test.Initial.AddNext(child);

            //--- Expectations

            //--- Action
            var result = test.ActivityEntry();

            //--- Verification
            Assert.Null(test.Initial.Parent);
            Assert.True(result);
            Assert.Same(child, activityStructure.CurrentActivity);
        }

        [Fact(Skip = "Skipped To find ActivityEntry not implemented.")]
        public void ActivityComposite_call_ActivityEntry_shall_throw_exception_when_Initial_has_no_child()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityCompositeMoq(activityStructure);

            //--- Expectations

            //--- Action
            Assert.Throws<InvalidOperationException>(() => test.ActivityEntry());

            //--- Verification
            this.VerifyLogForError = false;
            var eventLog = this.LoggedEvents.FirstOrDefault(e => e.Level == log4net.Core.Level.Error);
            Assert.NotNull(eventLog);
            Assert.Equal("Activity JTAGICEmkIITest.Moq.ActivityCompositeMoq has an initial activity that has no child activities, which is not supported yet.", eventLog.RenderedMessage);

        }

        [Fact(Skip = "Skipped To find ActivityEntry not implemented.")]
        public void ActivityComposite_call_ActivityEntry_shall_throw_exception_when_Initial_is_null()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityCompositeMoq(activityStructure);
            test.Initial = null!;

            //--- Expectations

            //--- Action
            Assert.Throws<InvalidOperationException>(() => test.ActivityEntry());

            //--- Verification
            this.VerifyLogForError = false;
            var eventLog = this.LoggedEvents.FirstOrDefault(e => e.Level == log4net.Core.Level.Error);
            Assert.NotNull(eventLog);
            Assert.Equal("Activity JTAGICEmkIITest.Moq.ActivityCompositeMoq has no initial activity to enter.", eventLog.RenderedMessage);

        }

        [Fact(Skip = "Skipped To find ActivityEntry not implemented.")]
        public void ActivityComposite_call_ActivityEntry_shall_throw_exception_when_too_many_children()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityCompositeMoq(activityStructure);
            var child = new ActivityBaseMoq(activityStructure);
            test.Initial.AddNext(child);
            test.Initial.AddNext(child);
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
        public void ActivityComposite_call_ActivityExit_shall_return_true()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityCompositeMoq(activityStructure);
            var child = new ActivityBaseCompMoq(activityStructure);
            var other = new ActivityBaseMoq(activityStructure);
            test.Initial.AddNext(child);
            test.AddNext(other);
            test.NextActivity = other;

            //--- Expectations

            //--- Action
            var result = test.ActivityExit(true);

            //--- Verification
            Assert.True(result);
            Assert.Same(other, test.NextActivity);

        }

        [Fact]
        public void ActivityComposite_call_ActivityExit_shall_not_exit_with_exitPoint()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityCompositeMoq(activityStructure);
            var child = new ActivityBaseMoq(activityStructure);
            var parent = new ActivityBaseMoq(activityStructure);
            test.Initial.AddNext(child);

            //--- Expectations
            test.NextActivity = child;

            //--- Action
            bool result = test.ActivityExit(true);

            //--- Verification
            Assert.True(result);
            Assert.Same(child, test.NextActivity);
        }

        #endregion

    }
}
