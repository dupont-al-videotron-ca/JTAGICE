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
    public class ActivityGetAllParameterTest : ActivityBaseTest
    {

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------
        public ActivityGetAllParameterTest() : base()
        {
        }
        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void ActivityGetAllParameter_Constructor_Test()
        {
            //--- Setup

            //--- Expectations

            //--- Action
            var test = new ActivityGetAllParameter(
                _activityStructure, 
                null);


            //--- Verification
            Assert.Equal(MasterCommandEnum.CMND_GET_PARAMETER, test.CommandEnum);
            Assert.Equal(SlaveResponseEnum.RSP_PARAMETER, test.ResponseEnum);

        }

        [Fact]
        public void ActivityGetAllParameter_shall_Accept_visitorCommand()
        {
            //--- Setup
            var test = new ActivitySetDeviceDescriptor(_activityStructure, null);

            //--- Expectations
            _visitorCommandMoq.Setup(m => m.Visit(It.IsAny<ActivitySetDeviceDescriptor>())).Returns(true);

            //--- Action
            var result = test.Accept(_visitorCommandMoq.Object);

            //--- Verification
            Assert.True(result);
        }

        [Fact]
        public void ActivityGetAllParameter_shall_Accept_visitorActivity()
        {
            //--- Setup
            var test = new ActivityGetAllParameter(_activityStructure, null);
            //--- Expectations

            //--- Action
            Assert.Throws<NotImplementedException>(() => test.Accept(_visitorActivityMoq.Object));

            //--- Verification
        }

        [Fact]
        public void ActivityGetAllParameter_OnReceivedResponse_shall_be_success()
        {
            //--- Setup
            var parent = new ActivityClearEvents(_activityStructure, null);
            var test = new ActivityGetAllParameter(_activityStructure, parent);

            //--- Expectations

            //--- Action
            var result = test.OnReceivedResponse(new Response(SlaveResponseEnum.RSP_PARAMETER));

            //--- Verification
            Assert.True(result);
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
        }

        [Fact]
        public void ActivityGetAllParameter_OnReceivedResponse_shall_throw_not_implemented_exception()
        {
            //--- Setup
            var test = new ActivityGetAllParameter(_activityStructure, null);

            //--- Expectations

            //--- Action
            Assert.Throws<NotImplementedException>(() => test.OnReceivedResponse(new Response(SlaveResponseEnum.RSP_PC)));

            //--- Verification
        }

        [Fact]
        public void ActivityGetAllParameter_ActivityEntry_shall_return_true()
        {
            //--- Setup
            var test = new ActivityGetAllParameter(_activityStructure, null);

            //--- Expectations

            //--- Action
            var result =test.ActivityEntry();

            //--- Verification
            Assert.True(result);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
        }

        [Fact]
        public void ActivityGetAllParameter_ActivityEntry_shall_return_false()
        {
            //--- Setup
            var test = new ActivityGetAllParameter(_activityStructure, null);

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
