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
            Assert.Equal(0x12, _activityStructure.Count());
            Assert.Same(parent, test.Parents[0]);
            Assert.Same(next, test.Nexts[0]);

            var expectedNext = test.Initial.Nexts[0];
            var expectedParent = test.Initial.Parent;
            Assert.IsType<ActivitySignOn>(expectedNext);
            Assert.Null(expectedParent);

            expectedParent = ((ActivitySignOn)expectedNext).Parent;
            expectedNext = ((ActivitySignOn)expectedNext).Nexts[0];
            Assert.IsType<ActivityReset>(expectedNext);
            Assert.IsType<ActivityInitial>(expectedParent);

            expectedParent = ((ActivityReset)expectedNext).Parent;
            expectedNext = ((ActivityReset)expectedNext).Nexts[0];
            Assert.IsType<ActivitySetDeviceDescriptor>(expectedNext);
            Assert.IsType<ActivitySignOn>(expectedParent);

            expectedParent = ((ActivitySetDeviceDescriptor)expectedNext).Parent;
            expectedNext = ((ActivitySetDeviceDescriptor)expectedNext).Nexts[0];
            Assert.IsType<ActivityClearEvents>(expectedNext);
            Assert.IsType<ActivityReset>(expectedParent);

            expectedParent = ((ActivityClearEvents)expectedNext).Parent;
            expectedNext = ((ActivityClearEvents)expectedNext).Nexts[0];
            Assert.IsType<ActivityGetParameter>(expectedNext);
            Assert.IsType<ActivitySetDeviceDescriptor>(expectedParent);

            expectedParent = ((ActivityGetParameter)expectedNext).Parent;
            expectedNext = ((ActivityGetParameter)expectedNext).Nexts[0];
            Assert.IsType<ActivityAction>(expectedNext);
            Assert.IsType<ActivityClearEvents>(expectedParent);

            expectedParent = ((ActivityAction)expectedNext).Parent;
            expectedNext = ((ActivityAction)expectedNext).Nexts[0];
            Assert.IsType<ActivitySetParameter>(expectedNext);
            Assert.IsType<ActivityGetParameter>(expectedParent);

            expectedParent = ((ActivitySetParameter)expectedNext).Parent;
            expectedNext = ((ActivitySetParameter)expectedNext).Nexts[0];
            Assert.IsType<ActivityFinal>(expectedNext);
            Assert.IsType<ActivityAction>(expectedParent);


            Assert.False(((ActivityFinal)expectedNext).HasChild);
            Assert.True(((ActivityFinal)expectedNext).HasParent);
            expectedParent = test.Final!.Parent;
            Assert.IsType<ActivitySetParameter>(expectedParent);


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

            //--- Expectations

            //--- Action
            var result = _activityStructure.RunActivity(test);

            //--- Verification
            Assert.True(result);
            Assert.NotNull(_activityStructure.CurrentActivity);
            Assert.Same(test.Initial.Nexts[0], _activityStructure.CurrentActivity );
        }

        #endregion


        #region Private Methods -----------------------------------------------

        #endregion


        #region Private Classes -----------------------------------------------

        #endregion

    }
}

