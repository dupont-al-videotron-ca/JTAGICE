using JTAGICEmkII.Slave;

namespace JTAGICEmkII
{
    public class ResponseReceivedEventArgs: EventArgs
    {

        #region Constructors 
        public ResponseReceivedEventArgs(Slave.ISlaveResponse response)
        {
            this.Response = response;
        }

        public ISlaveResponse Response { get; set; }

        #endregion

    }
}