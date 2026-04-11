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

        #endregion


        #region Constructors --------------------------------------------------
        public ActivityBaseTest() : base()
        {
            _hostService = new HostDeviceMoq();

            _visitorActivityMoq = this.MockRepository.Create<IVisitorActivity>();

        }

        #endregion


        #region Private Methods -----------------------------------------------

        #endregion
    }
}
