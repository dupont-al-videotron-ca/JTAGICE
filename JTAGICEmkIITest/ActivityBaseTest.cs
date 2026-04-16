using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Test.Xunit;
using JTAGICEmkII.HostService;
using JTAGICEmkIITest.Moq;
using Moq;
using Xunit;

namespace JTAGICEmkIITest
{
    public class ActivityBaseTest : XUnitTestBase
    {

        #region Declarations --------------------------------------------------

        protected HostDeviceMoq _hostService;
        protected Mock<IVisitorActivity> _visitorActivityMoq;
        protected Mock<IVisitorCommand> _visitorCommandMoq;
        protected StructureActivity _activityStructure;

        #endregion


        #region Constructors --------------------------------------------------
        public ActivityBaseTest() : base()
        {
            _hostService = new HostDeviceMoq();

            _visitorActivityMoq = this.MockRepository.Create<IVisitorActivity>();
            _visitorCommandMoq = this.MockRepository.Create<IVisitorCommand>();
            _activityStructure = new StructureActivity(_hostService);   

        }

        #endregion


        #region Private Methods -----------------------------------------------

        #endregion
    }
}
