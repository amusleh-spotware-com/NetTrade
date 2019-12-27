using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTrade.Implementations;
using System.Collections.Generic;

namespace NetTrade.Helpers.Tests
{
    [TestClass()]
    public class OptimizerParameterSetsCalculatorTests
    {
        [TestMethod()]
        public void GetAllParameterSetsTest()
        {
            var parameters = new List<OptimizeParameter>
            {
                new OptimizeParameter("First Parameter", 1, 10, 1),
                new OptimizeParameter("Second Parameter", -10, 100, 100),
                new OptimizeParameter("Third Parameter", 0, 10, 0.1),
                new OptimizeParameter("Fourth Parameter", 1)
            };

            int totalParameterSetsNumber = 0;

            foreach (var parameter in parameters)
            {
                if (totalParameterSetsNumber == 0)
                {
                    totalParameterSetsNumber = parameter.Values.Count;
                }
                else
                {
                    totalParameterSetsNumber *= parameter.Values.Count;
                }
            }

            var sets = OptimizerParameterSetsCalculator.GetAllParameterSets(parameters);

            Assert.AreEqual(totalParameterSetsNumber, sets.Count);
        }
    }
}