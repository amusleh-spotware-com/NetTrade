using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTrade.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Models;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;

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
                new AccountChange(1000, 100, DateTimeOffset.Now.AddDays(-10), string.Empty, AccountChangeType.Deposit),
                new AccountChange(1100, 100, DateTimeOffset.Now.AddDays(-9), string.Empty, AccountChangeType.Trading),
                new AccountChange(1200, 100, DateTimeOffset.Now.AddDays(-8), string.Empty, AccountChangeType.Trading),
                new AccountChange(1300, -400, DateTimeOffset.Now.AddDays(-7), string.Empty, AccountChangeType.Trading),
                new AccountChange(900, 100, DateTimeOffset.Now.AddDays(-6), string.Empty, AccountChangeType.Trading),
            };

            var actualDrawdown = Math.Round(MaxDrawdownCalculator.GetMaxDrawdown(changes), 1);
            var expectedDrawdown = -30.8;

            Assert.AreEqual(expectedDrawdown, actualDrawdown);
        }

        [TestMethod()]
        public void GetMaxDrawdownZeroTest()
        {
            var changes = new List<IAccountChange>
            {
                new AccountChange(1000, 100, DateTimeOffset.Now.AddDays(-10), string.Empty, AccountChangeType.Deposit),
                new AccountChange(1100, 100, DateTimeOffset.Now.AddDays(-9), string.Empty, AccountChangeType.Trading),
                new AccountChange(1200, 100, DateTimeOffset.Now.AddDays(-8), string.Empty, AccountChangeType.Trading),
            };

            var actualDrawdown = MaxDrawdownCalculator.GetMaxDrawdown(changes);
            var expectedDrawdown = 0;

            Assert.AreEqual(expectedDrawdown, actualDrawdown);
        }
    }
}