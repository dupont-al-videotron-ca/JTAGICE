using JTAGICEmkII.HostService;
using JTAGICEmkII.Slave;
using JTAGICEmkII.Master;

namespace JTAGICEmkII
{
    public class RequestEventArgs: EventArgs
    {

        #region Constructors 
        public RequestEventArgs(CommandRequest<IMasterCommand, ISlaveResponse> request)
        {
            this.Request = request;
        }

        public CommandRequest<IMasterCommand, ISlaveResponse> Request { get; private set; }

        #endregion

    }
}