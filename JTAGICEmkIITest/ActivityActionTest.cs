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
    public class ActivityActionTest : ActivityBaseTest
    {

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------
        public ActivityActionTest() : base() 
        { 
        }
        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void VisitorActivityActionTest_Constructor()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityBaseCompMoq(activityStructure);
            bool isActionExecuted = false;

            //--- Expectations

            //--- Action
            var test = new ActivityAction(activityStructure, parent, () => 
            {
                isActionExecuted = true;
                return true;
            });

            //--- Verification
            Assert.False(isActionExecuted);
        }

        [Fact]
        public void VisitorActivityActionTest_ActivityEntry_shall_return_true()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityBaseCompMoq(activityStructure);
            var next = new ActivityBaseCompMoq(activityStructure);
            bool isActionExecuted = false;
            var test = new ActivityAction(activityStructure, parent, () =>
            {
                isActionExecuted = true;
                return true;
            });
            test.AddNext(next);

            //--- Expectations

            //--- Action
            var result = test.ActivityEntry();

            //--- Verification
            Assert.True(isActionExecuted);
        }

        [Fact]
        public void VisitorActivityActionTest_Accept_shall_return_true()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var parent = new ActivityBaseCompMoq(activityStructure);
            bool isActionExecuted = false;
            var test = new ActivityAction(activityStructure, parent, () =>
            {
                isActionExecuted = true;
                return true;
            });

            //--- Expectations
            _visitorActivityMoq.Setup(m => m.Visit(It.IsAny<ActivityAction>())).Returns(true);

            //--- Action
            var result = test.Accept(_visitorActivityMoq.Object);

            //--- Verification
            Assert.True(result);
            Assert.False(isActionExecuted);
        }

        #endregion

    }
}
