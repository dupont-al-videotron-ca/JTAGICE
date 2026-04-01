using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Test.Xunit;
using log4net;
using Xunit;

namespace JTAGICEmkIITest
{
    public class JTAGICEmkIITest : XUnitTestBase
    {
        public JTAGICEmkIITest() : base()
        {
        }

        [Fact]
        public void TestMethod1()
        {
            Assert.True(true);
        }
    }
}
