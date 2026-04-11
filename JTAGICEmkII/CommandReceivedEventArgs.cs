using JTAGICEmkII.Master;

namespace JTAGICEmkII
{
    public class CommandReceivedEventArgs : EventArgs
    {

        #region Constructors 
        public CommandReceivedEventArgs(IMasterCommand command)
        {
            this.Command = command;
        }

        public IMasterCommand Command { get; set; }

        #endregion

    }
}