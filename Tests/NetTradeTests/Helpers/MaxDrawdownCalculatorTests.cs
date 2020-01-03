using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTrade.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Models;
using NetTrade.Abstractions.Interfaces;

namespace NetTrade.Helpers.Tests
{
    [TestClass()]
    public class MaxDrawdownCalculatorTests
    {
        [TestMethod()]
        public void GetMaxDrawdownTest()
        {
            var changes = new List<IAccountChange>
            {
                new AccountChange(1000, 100, DateTimeOffset.Now.AddDays(-10), string.Empty),
                new AccountChange(1100, 100, DateTimeOffset.Now.AddDays(-9), string.Empty),
                new AccountChange(1200, 100, DateTimeOffset.Now.AddDays(-8), string.Empty),
                new AccountChange(1300, -400, DateTimeOffset.Now.AddDays(-7), string.Empty),
                new AccountChange(900, 100, DateTimeOffset.Now.AddDays(-6), string.Empty),
            };

            var actualDrawdown = MaxDrawdownCalculator.GetMaxDrawdown(changes);
            var expectedDrawdown = -30.8;

            Assert.AreEqual(expectedDrawdown, actualDrawdown);
        }
    }
}