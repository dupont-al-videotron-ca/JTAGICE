using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.HostService;
using JTAGICEmkIITest.Moq;
using Moq;
using Xunit;
using Common.Test.Xunit;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkIITest
{
    public class CommandRequestTest : XUnitTestBase
    {
        private readonly Command _command;
        private readonly Response _response;

        #region Declarations --------------------------------------------------

        #endregion


        #region Constructors --------------------------------------------------

        public CommandRequestTest() : base()
        {
            _command = new Command(MasterCommandEnum.CMND_SIGN_OFF);
            _response = new Response(SlaveResponseEnum.RSP_OK);
        }

        #endregion


        #region Tests ---------------------------------------------------------

        [Fact]
        public void CommandRequest_Constructor_Command_Response()
        {
            //--- Setup

            //--- Expectations

            //--- Action
            var test = new CommandRequest<Command, Response>(_command, TimeSpan.FromSeconds(45));

            //--- Verification
            Assert.NotNull(test.Command);
            Assert.Same(_command, test.Command);
            Assert.Null(test.Response);
            Assert.Equal(3, test.RetryCount);
        }

        [Fact]
        public void CommandRequest_Constructor_Response_Command()
        {
            //--- Setup

            //--- Expectations

            //--- Action
            var test = new CommandRequest<Response, Command>(_response, TimeSpan.FromSeconds(45));

            //--- Verification
            Assert.NotNull(test.Command);
            Assert.Same(_response, test.Command);
            Assert.Null(test.Response);
            Assert.Equal(3, test.RetryCount);
        }

        [Fact]
        public void CommandRequest_Constructor_shall_throw_exception_when_Command_is_null()
        {
            //--- Setup

            //--- Expectations

            //--- Action
            Assert.Throws<ArgumentNullException>(() => new CommandRequest<Response, Command>(null!, TimeSpan.FromSeconds(45)));

            //--- Verification
        }

        [Fact]
        public void CommandRequest_ReceivedResponse_shall_set_Response()
        {
            //--- Setup
            var test = new CommandRequest<Command, Response>(_command, TimeSpan.FromSeconds(45));

            //--- Expectations

            //--- Action
            test.ReceivedResponse(_response);

            //--- Verification
            Assert.NotNull(test.Command);
            Assert.NotNull(test.Response);
            Assert.Same(_command, test.Command);
            Assert.Same(_response, test.Response);
            Assert.Equal(3, test.RetryCount);
        }

        [Fact]
        public void CommandRequest_ReceivedResponse_shall_WaitForResponse()
        {
            //--- Setup
            var test = new CommandRequest<Command, Response>(_command, TimeSpan.FromMilliseconds(100));
            var expectedRetryCount = 6;
            test.RetryCount = expectedRetryCount;
            Assert.Equal(expectedRetryCount, test.RetryCount);

            //--- Expectations

            //--- Action
            var result = test.WaitForResponse();

            //--- Verification
            Assert.False(result);
            Assert.NotNull(test.Command);
            Assert.Null(test.Response);
            Assert.Same(_command, test.Command);
            Assert.Equal(expectedRetryCount-1, test.RetryCount);
        }
        #endregion


        #region Private Methods -----------------------------------------------

        #endregion


        #region Private Classes -----------------------------------------------

        #endregion


    }
}
