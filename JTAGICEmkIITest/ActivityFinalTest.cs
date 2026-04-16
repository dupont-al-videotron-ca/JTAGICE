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
    public class ActivityFinalTest : ActivityBaseTest
    {

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------

        public ActivityFinalTest() : base()
        {
        }

        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void ActivityFinal_Constructor_with_null_parent_shall_throw_ArgumentNullException()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);

            //--- Expectations

            //--- Action
            Assert.Throws<ArgumentNullException>(() => new ActivityFinal(activityStructure, null!));

            //--- Verification
        }

        [Fact]
        public void ActivityFinal_Shall_Constructor_with_parent()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityBaseMoq(activityStructure);
            //--- Expectations

            //--- Action
            var test = new ActivityFinal(activityStructure, parent);

            //--- Verification
            Assert.NotNull(test.Parent);
            Assert.Single(test.Parents);
            Assert.Same(parent, test.Parent);
            Assert.False(test.IsExitPoint);
        }

        [Fact]
        public void ActivityFinal_Shall_Constructor_with_parent_and_exit_point()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityBaseMoq(activityStructure);
            //--- Expectations

            //--- Action
            var test = new ActivityFinal(activityStructure, parent, true);

            //--- Verification
            Assert.NotNull(test.Parent);
            Assert.Single(test.Parents);
            Assert.Same(parent, test.Parent);
            Assert.True(test.IsExitPoint);
        }

        [Fact]
        public void ActivityFinal_Shall_call_the_visitor()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityBaseMoq(activityStructure);
            var test = new ActivityFinal(activityStructure, parent);

            //--- Expectations
            _visitorActivityMoq.Setup(m => m.Visit(It.IsAny<ActivityFinal>())).Returns(true);

            //--- Action
            var result = test.Accept(_visitorActivityMoq.Object);

            //--- Verification
            Assert.True(result);
        }

        [Fact]
        public void ActivityFinal_call_ActivityEntry_shall_return_true()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityBaseMoq(activityStructure);
            var test = new ActivityFinal(activityStructure, parent);
            activityStructure.PushActivity(test);

            //--- Expectations

            //--- Action
            var result = test.ActivityEntry();

            //--- Verification
            Assert.True(result);
            Assert.NotNull(activityStructure.CurrentActivity);
            Assert.Same(test, activityStructure.CurrentActivity);
        }

        [Fact]
        public void ActivityFinal_call_ActivityExit_shall_return_true()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityBaseMoq(activityStructure);
            var test = new ActivityFinal(activityStructure, parent);
            activityStructure.PushActivity(test);

            //--- Expectations

            //--- Action
            var result = test.ActivityExit(true);

            //--- Verification
            Assert.True(result);
            Assert.Same(test, activityStructure.CurrentActivity);

        }

        [Fact]
        public void ActivityFinal_call_ActivityExit_shall_throw_exception_when_adding_child()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityBaseMoq(activityStructure);
            var test = new ActivityFinal(activityStructure, parent);
            activityStructure.PushActivity(test);
            var child = new ActivityBaseMoq(activityStructure);

            //--- Expectations

            //--- Action
            Assert.Throws<InvalidOperationException>(() => test.AddNext(child));

            //--- Verification
            Assert.Same(test, activityStructure.CurrentActivity);
        }

        [Fact]
        public void ActivityFinal_call_ActivityExit_shall_throw_exception_when_adding_children()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityBaseMoq(activityStructure);
            var test = new ActivityFinal(activityStructure, parent);
            activityStructure.PushActivity(test);
            var child = new ActivityBaseMoq(activityStructure);

            //--- Expectations

            //--- Action
            Assert.Throws<InvalidOperationException>(() => test.AddNextRange(new[] { child }));

            //--- Verification
            Assert.Same(test, activityStructure.CurrentActivity);
        }

        [Fact]
        public void ActivityFinal_call_ActivityExit_shall_return_true_with_exit_point()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityBaseMoq(activityStructure);
            var test = new ActivityFinal(activityStructure, parent, true);
            activityStructure.PushActivity(test);
            var child = new ActivityBaseMoq(activityStructure);

            //--- Expectations

            //--- Action
            var result = test.ActivityExit(true);

            //--- Verification
            Assert.True(result);
            Assert.Same(test, activityStructure.CurrentActivity);

        }


        #endregion

    }
}
