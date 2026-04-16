using JTAGICEmkII.HostService;
using JTAGICEmkII.Slave;
using JTAGICEmkII.Master;

namespace JTAGICEmkII
{
    public class RequestEventArgs: EventArgs
    {

        #region Constructors 
        public RequestEventArgs(CommandRequestBase<IMasterCommand, ISlaveResponse> request)
        {
            this.Request = request;
        }

        public CommandRequestBase<IMasterCommand, ISlaveResponse> Request { get; private set; }

        #endregion

    }
}