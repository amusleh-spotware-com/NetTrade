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

            int totalParametersGridNumber = 0;

            foreach (var parameter in parameters)
            {
                if (totalParametersGridNumber == 0)
                {
                    totalParametersGridNumber = parameter.Values.Count;
                }
                else
                {
                    totalParametersGridNumber *= parameter.Values.Count;
                }
            }

            var parametersGrid = GridOptimizerParametersCalculator.GetParametersGrid(parameters);

            Assert.AreEqual(totalParametersGridNumber, parametersGrid.Count);
        }
    }
}