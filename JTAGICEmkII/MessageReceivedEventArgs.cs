using JTAGICEmkII.Slave;

namespace JTAGICEmkII
{
    public class MessageReceivedEventArgs: EventArgs
    {

        #region Constructors 
        public MessageReceivedEventArgs(Slave.ISlaveResponse response)
        {
            this.Response = response;
        }

        public ISlaveResponse Response { get; set; }

        #endregion

    }
}