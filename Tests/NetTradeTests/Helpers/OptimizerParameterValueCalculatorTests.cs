using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTrade.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Models;
using System.Linq;

namespace NetTrade.Helpers.Tests
{
    [TestClass()]
    public class OptimizerParameterValueCalculatorTests
    {
        private readonly OptimizeParameter firstParameter = new OptimizeParameter("First Parameter", -10, 100, 100);
        private readonly OptimizeParameter secondParameter = new OptimizeParameter("Second Parameter", "Test");

        private readonly List<int> firstParameterExpectedValues = new List<int> { -10, 90 };
        private readonly List<string> secondParameterExpectedValues = new List<string> { "Test" };

        [TestMethod()]
        public void GetParameterRangeTest()
        {
            var firstParameterRange = OptimizerParameterValueCalculator.GetParameterRange(firstParameter);
            var secondParameterRange = OptimizerParameterValueCalculator.GetParameterRange(secondParameter);

            Assert.AreEqual(firstParameterExpectedValues.Count, firstParameterRange);
            Assert.AreEqual(secondParameterExpectedValues.Count, secondParameterRange);
        }

        [TestMethod()]
        public void GetParameterAllValuesTest()
        {
            var firstParameterValues = OptimizerParameterValueCalculator.GetParameterAllValues(firstParameter);
            var secondParameterValues = OptimizerParameterValueCalculator.GetParameterAllValues(secondParameter);

            bool result = firstParameterValues.All(value => firstParameterExpectedValues.Contains((int)value));
            Assert.IsTrue(result);
            Assert.IsTrue(secondParameterValues.All(value => secondParameterExpectedValues.Contains(value.ToString())));
        }
    }
}