using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTrade.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Helpers.Tests
{
    [TestClass()]
    public class SampleDataGeneratorTests
    {
        [TestMethod()]
        public void GetSampleDataTest()
        {
            var data = SampleDataGenerator.GetSampleData(100, DateTimeOffset.Now.AddDays(-19), DateTimeOffset.Now,
                TimeSpan.FromDays(1));

            Assert.IsTrue(data.Count == 20);
            Assert.IsTrue(data[0].Close == data[1].Open);
        }
    }
}