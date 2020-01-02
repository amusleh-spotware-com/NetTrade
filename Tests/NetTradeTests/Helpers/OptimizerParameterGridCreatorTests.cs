using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTrade.Models;
using System.Collections.Generic;

namespace NetTrade.Helpers.Tests
{
    [TestClass()]
    public class OptimizerParameterGridCreatorTests
    {
        [TestMethod()]
        public void GetParameterGridTest()
        {
            var parameters = new List<OptimizeParameter>
            {
                new OptimizeParameter("First Parameter", 1, 10, 1),
                new OptimizeParameter("Second Parameter", -10, 100, 100),
                new OptimizeParameter("Third Parameter", 0, 10, 0.1),
                new OptimizeParameter("Fourth Parameter", "Test")
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

            var parametersGrid = OptimizerParameterGridCreator.GetParameterGrid(parameters);

            Assert.AreEqual(totalParametersGridNumber, parametersGrid.Count);
        }
    }
}