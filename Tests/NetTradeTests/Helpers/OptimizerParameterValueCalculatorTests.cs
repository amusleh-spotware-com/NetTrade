using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTrade.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Models;

namespace NetTrade.Helpers.Tests
{
    [TestClass()]
    public class OptimizerParameterValueCalculatorTests
    {
        [TestMethod()]
        public void GetParameterRangeTest()
        {
            var firstParameter = new OptimizeParameter("First Parameter", -10, 100, 100);
            var secondParameter = new OptimizeParameter("Second Parameter", "Test");

            var firstParameterRange = OptimizerParameterValueCalculator.GetParameterRange(firstParameter);
            var secondParameterRange = OptimizerParameterValueCalculator.GetParameterRange(secondParameter);

            Assert.AreEqual(2, firstParameterRange);
            Assert.AreEqual(1, secondParameterRange);
        }
    }
}