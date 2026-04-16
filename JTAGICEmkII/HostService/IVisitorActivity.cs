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

        bool Visit(ActivityAction element);

        bool Visit(TargetConnecting element);

        bool Visit(TargetDisonnecting element);

        bool Visit(TargetConnected element);

        bool Visit(TargetStopped element);

        bool Visit(TargetRunning element);

        bool Visit(TargetProgramming element);
        
        bool Visit(HostSession element);

        

    }
}
