using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public class CommandRequest : CommandRequestBase<IMasterCommand, ISlaveResponse>, ICommandRequest<IMasterCommand, ISlaveResponse>
    {
        public CommandRequest(IMasterCommand command, IActivityComElement activityElement) : base(command, activityElement)
        {
        }

        public CommandRequest(IMasterCommand command, IActivityComElement activityElement, TimeSpan timeout) : base(command, activityElement, timeout)
        {
        }
    }
}
