using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII.HostService;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;
using JTAGICEmkIITest.Moq;
using Moq;
using Xunit;

namespace JTAGICEmkIITest
{
    public class CommandRequestTest : ActivityBaseTest
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
            var test = CommandRequestFactory.CreateRequest(this._activityStructure, _command.MessageId);

            //--- Verification
            Assert.NotNull(test.Command);
            Assert.Equal(_command.MessageId, test.Command.MessageId);
            Assert.Null(test.Response);
            Assert.Equal(3, test.RetryCount);
        }

        [Fact(Skip = ("The RespopnceRequestFactory does not exist."))]
        public void CommandRequest_Constructor_Response_Command()
        {
            //--- Setup

            //--- Expectations

            //--- Action
            //var test = new CommandRequestBase<Response, Command>(_response, TimeSpan.FromSeconds(45));

            ////--- Verification
            //Assert.NotNull(test.Command);
            //Assert.Same(_response, test.Command);
            //Assert.Null(test.Response);
            //Assert.Equal(3, test.RetryCount);
        }

        [Fact(Skip = ("The RespopnceRequestFactory does not exist."))]
        public void CommandRequest_Constructor_shall_throw_exception_when_Command_is_null()
        {
            //--- Setup

            //--- Expectations

            //--- Action
            //Assert.Throws<ArgumentNullException>(() => new CommandRequestBase<Response, Command>(null!, TimeSpan.FromSeconds(45)));

            //--- Verification
        }

        [Fact]
        public void CommandRequest_ReceivedResponse_shall_set_Response()
        {
            //--- Setup
            var test = CommandRequestFactory.CreateRequest(this._activityStructure, _command.MessageId);

            //--- Expectations

            //--- Action
            test.ReceivedResponse(_response);

            //--- Verification
            Assert.NotNull(test.Command);
            Assert.NotNull(test.Response);
            Assert.Equal(_command.MessageId, test.Command.MessageId);
            Assert.Equal(_response.ResponseId, test.Response.ResponseId);
            Assert.Equal(3, test.RetryCount);
        }

        [Fact]
        public void CommandRequest_ReceivedResponse_shall_WaitForResponse()
        {
            //--- Setup
            var test = CommandRequestFactory.CreateRequest(this._activityStructure, _command.MessageId, TimeSpan.FromMicroseconds(100));
            var expectedRetryCount = 6;
            test.RetryCount = expectedRetryCount;

            //--- Expectations

            //--- Action
            var result = test.WaitForResponse();

            //--- Verification
            Assert.False(result);
            Assert.NotNull(test.Command);
            Assert.Null(test.Response);
            Assert.Equal(_command.MessageId, test.Command.MessageId);
            Assert.Equal(expectedRetryCount - 1, test.RetryCount);
        }
        #endregion


        #region Private Methods -----------------------------------------------

        #endregion


        #region Private Classes -----------------------------------------------

        #endregion


    }
}
