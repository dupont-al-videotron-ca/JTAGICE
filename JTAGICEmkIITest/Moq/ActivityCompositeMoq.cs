using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.HostService;
using log4net;

namespace JTAGICEmkIITest.Moq
{
    internal sealed class ActivityCompositeMoq : ActivityComposite
    {
        public ActivityCompositeMoq(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {
        }

        internal ActivityCompositeMoq(StructureActivity activityStructure) : base(activityStructure)
        {
        }

        public override bool Accept(IVisitorActivity visitor)
        {
            return true;
        }
    }
}
