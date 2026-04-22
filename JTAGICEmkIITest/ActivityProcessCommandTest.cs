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
    public class ActivityProcessCommandTest : ActivityBaseTest
    {

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------

        public ActivityProcessCommandTest() : base()
        {
        }

        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void ActivityProcessCommand_Constructor_Test()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);

            //--- Expectations

            //--- Action
            var test = new ActivityProcessCommandMoq(activityStructure);

            //--- Verification
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
            Assert.True(test.IsCanSendCommand);
            Assert.Equal(MasterCommandEnum.CMND_RESET, test.CommandEnum);

        }

        [Fact]
        public void ActivityProcessCommand_OnReceivedCommand_shall_return_success()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityProcessCommandMoq(activityStructure);

            //--- Expectations

            //--- Action
            var result = test.OnReceivedCommand(new Command(MasterCommandEnum.CMND_RESET));

            //--- Verification
            Assert.True(result);
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
            Assert.True(test.IsCanSendCommand);
            Assert.Equal(MasterCommandEnum.CMND_RESET, test.CommandEnum);

        }

        [Fact]
        public void ActivityProcessCommand_OnReceivedResponse_shall_return_success()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityProcessCommandMoq(activityStructure);

            //--- Expectations

            //--- Action
            var result = test.OnReceivedResponse(new Response(SlaveResponseEnum.RSP_OK));

            //--- Verification
            Assert.True(result);
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
            Assert.True(test.IsCanSendCommand);
            Assert.Equal(MasterCommandEnum.CMND_RESET, test.CommandEnum);

        }

        [Theory]
        [InlineData(SlaveResponseEnum.RSP_FAILED)]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_PARAMETER)]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_MEMORY_TYPE)]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_MEMORY_RANGE)]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_EMULATOR_MODE)]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_MCU_STATE)]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_COMMAND)]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_VALUE)]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_BREAKPOINT)]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_JTAG_ID)]
        [InlineData(SlaveResponseEnum.RSP_NO_TARGET_POWER)]
        [InlineData(SlaveResponseEnum.RSP_DEBUGWIRE_SYNC_FAILED)]
        [InlineData(SlaveResponseEnum.RSP_ILLEGAL_POWER_STATE)]
        public void ActivityProcessCommand_OnReceivedResponse_shall_set_lastError(SlaveResponseEnum expectedError)
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityProcessCommandMoq(activityStructure);

            //--- Expectations

            //--- Action
            ISlaveResponse? response = null;
            if (expectedError == SlaveResponseEnum.RSP_ILLEGAL_EMULATOR_MODE)
            {
                response = new ResponseEmulatorMode(expectedError);
            }
            else if (expectedError == SlaveResponseEnum.RSP_ILLEGAL_MCU_STATE)
            {
                response = new ResponseMcuState(expectedError);
            }
            else
            {
                response = new Response(expectedError);
            }

            var result = test.OnReceivedResponse(response);

            //--- Verification
            Assert.True(result);
            Assert.True(test.HasError);
            Assert.Equal(expectedError, test.LastError);
            Assert.True(test.IsCanSendCommand);
            Assert.Equal(MasterCommandEnum.CMND_RESET, test.CommandEnum);

        }

        [Theory]
        [InlineData(SlaveResponseEnum.EVT_TARGET_POWER_ON)]
        [InlineData(SlaveResponseEnum.EVT_TARGET_POWER_OFF)]
        [InlineData(SlaveResponseEnum.EVT_EXTERNAL_RESET)]
        [InlineData(SlaveResponseEnum.EVT_TARGET_SLEEP)]
        [InlineData(SlaveResponseEnum.EVT_TARGET_WAKEUP)]
        [InlineData(SlaveResponseEnum.EVT_ICE_POWER_ERROR_STATE)]
        [InlineData(SlaveResponseEnum.EVT_ICE_POWER_OK)]
        [InlineData(SlaveResponseEnum.EVT_IDR_DIRTY)]
        [InlineData(SlaveResponseEnum.EVT_PROGRAM_BREAK)]
        [InlineData(SlaveResponseEnum.EVT_PDSB_BREAK)]
        [InlineData(SlaveResponseEnum.EVT_PDSMB_BREAK)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_FORCE_BREAK_TIMEOUT)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_RELEASE_BREAK_TIMEOUT)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_MAX_BIT_LENGHT_DIFF)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_TIMEOUT_BAUD)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_OUT_OF_RANGE)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_SYNC_WAIT_TIMEOUT)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_TIMEOUT)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_RECEIVE_BREAK)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVE_TIMEOUT)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_OPT_RECEIVED_BREAK)]
        [InlineData(SlaveResponseEnum.EVT_ERROR_PHY_NO_ACTIVITY)]
        [InlineData(SlaveResponseEnum.EVT_BREAK)]
        [InlineData(SlaveResponseEnum.EVT_RUN)]
        [InlineData(SlaveResponseEnum.EVT_DEBUG)]
        public void ActivityProcessCommand_OnReceivedResponse_shall_receive_event(SlaveResponseEnum expectedEvent)
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityProcessCommandMoq(activityStructure);

            //--- Expectations

            //--- Action
            var result = test.OnReceivedResponse(new Response(expectedEvent));

            //--- Verification
            Assert.True(result);
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
            Assert.True(test.IsCanSendCommand);
            Assert.Equal(MasterCommandEnum.CMND_RESET, test.CommandEnum);

        }

        [Fact]
        public void ActivityProcessCommand_CommandSent_shall_return_success()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityProcessCommandMoq(activityStructure);

            //--- Expectations

            //--- Action
            var result = test.CommandSent();

            //--- Verification
            Assert.True(result);
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
            Assert.True(test.IsCanSendCommand);
            Assert.Equal(MasterCommandEnum.CMND_RESET, test.CommandEnum);

        }

        [Fact]
        public void ActivityProcessCommand_CanSendResponse_shall_return_success()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityProcessCommandMoq(activityStructure);

            //--- Expectations

            //--- Action

            var result = test.CanSendResponse(new Response(SlaveResponseEnum.RSP_OK));

            //--- Verification
            Assert.True(result);
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
            Assert.True(test.IsCanSendCommand);
            Assert.Equal(MasterCommandEnum.CMND_RESET, test.CommandEnum);

        }

        [Fact]
        public void ActivityProcessCommand_CanSendResponse_shall_return_fail()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityProcessCommandMoq(activityStructure);

            //--- Expectations

            //--- Action

            var result = test.CanSendResponse(new Response((SlaveResponseEnum)(-1)));

            //--- Verification
            Assert.False(result);
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
            Assert.True(test.IsCanSendCommand);
            Assert.Equal(MasterCommandEnum.CMND_RESET, test.CommandEnum);

        }


        [Fact]
        public void ActivityProcessCommand_CanSendCommand_shall_return_success()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityProcessCommandMoq(activityStructure);

            //--- Expectations

            //--- Action
            var result = test.CanSendCommand(new Command(MasterCommandEnum.CMND_RESET));

            //--- Verification
            Assert.True(result);
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
            Assert.True(test.IsCanSendCommand);
            Assert.Equal(MasterCommandEnum.CMND_RESET, test.CommandEnum);

        }

        [Fact]
        public void ActivityProcessCommand_CanSendCommand_shall_return_fail()
        {
            //--- Setup
            var activityStructure = new JTAGICEmkII.HostService.StructureActivity(_hostService);
            var test = new ActivityProcessCommandMoq(activityStructure);

            //--- Expectations

            //--- Action
            var result = test.CanSendCommand(new Command((MasterCommandEnum)(-1)));

            //--- Verification
            Assert.False(result);
            Assert.False(test.HasError);
            Assert.Equal(SlaveResponseEnum.RSP_OK, test.LastError);
            Assert.True(test.IsCanSendCommand);
            Assert.Equal(MasterCommandEnum.CMND_RESET, test.CommandEnum);

        }

        #endregion

    }
}
