using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;

namespace JTAGICEmkII.HostService
{
    public interface IActivityElement
    {
        bool Accept(IVisitorActivity visitor);

        bool ActivityExit(bool lastRequest);

        bool ActivityEntry();

        bool ActivityAction();

    }
}
