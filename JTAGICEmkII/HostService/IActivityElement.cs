using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;
using JTAGICEmkII.Master;
using JTAGICEmkII.Slave;

namespace JTAGICEmkII.HostService
{
    public interface IActivityElement
    {
        bool Accept(IVisitorActivity visitor);

        bool ActivityExit(bool lastRequest);

        bool ActivityEntry();

        bool ActivityAction();

        bool EventReceived(ISlaveResponse response);

        bool RequestTimeout(CommandRequestBase<IMasterCommand, ISlaveResponse> request);

        bool RequestCompleted(HostService.CommandRequestBase<Master.IMasterCommand, Slave.ISlaveResponse> request);

        IActivityElement? NextActivity { get; }
    }
}
