using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTAGICEmkII.HostService;
using log4net;

using log4net.Repository.Hierarchy;

namespace JTAGICEmkIITest.Moq
{
    internal sealed class ActivityBaseCompMoq : ActivityBaseComp
    {

        #region Constructors 

        internal ActivityBaseCompMoq(StructureActivity activityStructure) : base(activityStructure)
        {
        }

        internal ActivityBaseCompMoq(StructureActivity activityStructure, IActivityElement? parent) : base(activityStructure, parent)
        {
        }

        internal ActivityBaseCompMoq(StructureActivity activityStructure, IActivityElement? parent, IEnumerable<IActivityElement> nextElements) : base(activityStructure, parent!, nextElements)
        {
        }

        #endregion


        #region Fields 

        #endregion


        #region Properties 

        public int NextIndex { get => _nextIndex; set => _nextIndex = value; }

        #endregion


        #region Delegates / Events 

        #endregion


        #region Public Methods 
        public override bool Accept(IVisitorActivity visitor) => true;

        #endregion
    }
}
