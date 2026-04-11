using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII;

namespace JTAGICEmkII.HostService
{
    internal static class StructureActivityBuilder
    {

        public static void Build(StructureActivity activityStructure)
        {
            ArgumentNullException.ThrowIfNull(activityStructure);

            var targetConnecting = new TargetConnecting(activityStructure);
            activityStructure.AddActivity(targetConnecting);

            var targetConnected = new TargetConnected(activityStructure, targetConnecting);
            activityStructure.AddActivity(targetConnected);
            targetConnecting.AddNext(targetConnected);

            var signOff = new ActivitySignOff(activityStructure, targetConnected);
            var targetFinal = new ActivityFinal(activityStructure, signOff);
            signOff.AddNext(targetFinal);


             activityStructure.CurrentActivity = targetConnecting;
        }


    }
}
