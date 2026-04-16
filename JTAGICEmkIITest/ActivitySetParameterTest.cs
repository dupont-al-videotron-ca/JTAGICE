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
    public class ActivitySetParameterTest : ActivityBaseTest
    {

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------
        public ActivitySetParameterTest() : base()
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
            var test = new ActivitySetParameter(_activityStructure);


            //--- Verification
            Assert.Equal(MasterCommandEnum.CMND_SET_PARAMETER, test.CommandEnum);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.ResponseEnum);

        }

        [Fact]
        public void ActivitySetAllParameter_shall_Accept_visitorCommand()
        {
            //--- Setup
            var test = new ActivitySetParameter(_activityStructure);

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
            var test = new ActivitySetParameter(_activityStructure);
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
            var test = new ActivitySetParameter(_activityStructure);
            var next = new ActivityBaseMoq(_activityStructure);

            //--- Expectations

            //--- Action
            var result = test.OnReceivedResponse(new Response(SlaveResponseEnum.RSP_OK));

            //--- Verification
            Assert.True(result);
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
        }

        [Fact]
        public void ActivitySetAllParameter_OnReceivedResponse_shall_throw_not_implemented_exception()
        {
            //--- Setup
            var test = new ActivitySetParameter(_activityStructure);

            //--- Expectations

            //--- Action
            Assert.Throws<NotImplementedException>(() => test.OnReceivedResponse(new Response(SlaveResponseEnum.RSP_PC)));

            //--- Verification
        }

        #endregion


    }
}
