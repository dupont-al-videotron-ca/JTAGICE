using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.HostService;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using JTAGICEmkIITest.Moq;
using Moq;
using Xunit;

namespace JTAGICEmkIITest
{
    public class ActivitySetAllParameterTest : ActivityBaseTest
    {

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------
        public ActivitySetAllParameterTest() : base()
        {
        }
        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void ActivitySetAllParameter_Constructor_Test()
        {
            //--- Setup

            //--- Expectations

            //--- Action
            var test = new ActivitySetAllParameter(
                _activityStructure, 
                null);


            //--- Verification
            Assert.Equal(MasterCommandEnum.CMND_SET_PARAMETER, test.CommandEnum);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.ResponseEnum);

        }

        [Fact]
        public void ActivitySetAllParameter_shall_Accept_visitorCommand()
        {
            //--- Setup
            var test = new ActivitySetAllParameter(_activityStructure, null);

            //--- Expectations
            _visitorCommandMoq.Setup(m => m.Visit(It.IsAny<ActivitySetParameter  >())).Returns(true);

            //--- Action
            var result = test.Accept(_visitorCommandMoq.Object);

            //--- Verification
            Assert.True(result);
        }

        [Fact]
        public void ActivitySetAllParameter_shall_Accept_visitorActivity()
        {
            //--- Setup
            var test = new ActivitySetAllParameter(_activityStructure, null);
            //--- Expectations

            //--- Action
            Assert.Throws<NotImplementedException>(() => test.Accept(_visitorActivityMoq.Object));

            //--- Verification
        }

        [Fact]
        public void ActivitySetAllParameter_OnReceivedResponse_shall_be_success()
        {
            //--- Setup
            var parent = new ActivityAction(_activityStructure, null!, () => { return true; });
            var test = new ActivitySetAllParameter(_activityStructure, parent);
            var next = new ActivityBaseMoq(_activityStructure);
            test.AddNext(next);

            //--- Expectations

            //--- Action
            var result = test.OnReceivedResponse(new Response(SlaveResponseEnum.RSP_OK));

            //--- Verification
            Assert.True(result);
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
        }

        [Fact]
        public void ActivitySetAllParameter_OnReceivedResponse_shall_throw_exception_with_child()
        {
            //--- Setup
            var parent = new ActivityAction(_activityStructure, null!, () => { return true; });
            var test = new ActivitySetAllParameter(_activityStructure, parent);

            //--- Expectations

            //--- Action
            Assert.Throws<InvalidOperationException>(() => test.OnReceivedResponse(new Response(SlaveResponseEnum.RSP_OK)));

            //--- Verification
            
        }

        [Fact]
        public void ActivitySetAllParameter_OnReceivedResponse_shall_throw_not_implemented_exception()
        {
            //--- Setup
            var test = new ActivitySetAllParameter(_activityStructure, null);

            //--- Expectations

            //--- Action
            Assert.Throws<NotImplementedException>(() => test.OnReceivedResponse(new Response(SlaveResponseEnum.RSP_PC)));

            //--- Verification
        }

        [Fact]
        public void ActivitySetAllParameter_ActivityEntry_shall_return_true()
        {
            //--- Setup
            var test = new ActivitySetAllParameter(_activityStructure, null);

            //--- Expectations

            //--- Action
            var result =test.ActivityEntry();

            //--- Verification
            Assert.True(result);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
        }

        [Fact]
        public void ActivitySetAllParameter_ActivityEntry_shall_return_false()
        {
            //--- Setup
            var test = new ActivitySetAllParameter(_activityStructure, null);

            //--- Expectations
            this._hostService.ForceSuccess = false;

            //--- Action
            var result = test.ActivityEntry();

            //--- Verification
            Assert.False(result);
            Assert.Null(_hostService.SignOnResponse);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
        }

        #endregion


    }
}
