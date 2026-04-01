using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Test.Xunit;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Moq;
using Xunit;
using MyFramework;


namespace MyFrameWorkTest
{
    public class Crc16Test : XUnitTestBase
    {

        public Crc16Test() : base()
        {
        }

        [Fact]
        public void ComputeCrc_should_return_expected_CRC16()
        {
            byte[] data = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x09 };
            UInt16 crc = Crc16.ComputeCrc(data);
            Assert.Equal(0xa55E, crc);
        }

        [Fact]
        public void AppendCrc_should_return_append_crc()
        {
            byte[] data = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x09, 0x00, 0x00 };
            Crc16.AppendCrc(data);
            Assert.True(Crc16.ValidateCrc(data));
        }

        [Fact]
        public void ValidateCrc_should_return_true_for_valid_crc()
        {
            byte[] data = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x09 };
            UInt16 expectedCrc = Crc16.ComputeCrc(data);
            byte[] expectedData = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x09, (byte)(expectedCrc & 0xff), (byte)(expectedCrc >> 8 & 0xff) };

            var result = Crc16.ValidateCrc(expectedData);
            Assert.True(result);
        }

    }
}
