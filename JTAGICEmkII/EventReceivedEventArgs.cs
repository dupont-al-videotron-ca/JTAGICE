using JTAGICEmkII.Slave;

namespace JTAGICEmkII
{
    public class EventReceivedEventArgs: EventArgs
    {

        #region Constructors 
        public EventReceivedEventArgs(Slave.ISlaveResponse response)
        {
            this.Event = response;
        }

        public ISlaveResponse Event { get; set; }

        #endregion

    }
}