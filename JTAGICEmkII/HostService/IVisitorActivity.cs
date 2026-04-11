using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTAGICEmkII.HostService
{
    public interface IVisitorActivity
    {
        //        bool Visit(IAcitityElement element);
        bool Visit(ActivityInitial element);

        bool Visit(ActivityFinal element);

        bool Visit(ActivityAction element);

        bool Visit(TargetConnecting element);

        bool Visit(TargetConnected element);

    }
}
