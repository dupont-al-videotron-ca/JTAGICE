using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;

namespace JTAGICEmkII.HostService
{
    public interface IActivityComElement
    {
        bool Accept(IVisitorCommand visitor);

        bool CanSendCommand(Master.IMasterCommand command);
        bool OnReceivedResponse(Slave.ISlaveResponse response);


        bool CanSendResponse(Slave.ISlaveResponse command);

        bool OnReceivedCommand(Master.IMasterCommand response);

        bool CommandSent();

    }
}
