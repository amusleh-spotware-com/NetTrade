using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetTrade.Helpers
{
    public static class OptimizerRobotCreator
    {
        public static Robot GetRobot<TRobot>(Dictionary<string, object> parameterGrid, IOptimizer optimizer)
            where TRobot : Robot
        {
            var robotSettings = optimizer.GetRobotSettings();

            var robot = Activator.CreateInstance(typeof(TRobot), robotSettings) as Robot;

            if (parameterGrid.Any())
            {
                SetRobotParameters(robot, parameterGrid);
            }

            return robot;
        }

        private static void SetRobotParameters<TRobot>(TRobot robot, Dictionary<string, object> parameterGrid)
            where TRobot : IRobot
        {
            var robotParameters = typeof(IRobot).GetProperties()
            .Where(iProperty => iProperty.GetCustomAttributes(true).Any(iAttribute => iAttribute is ParameterAttribute));

            if (!robotParameters.Any())
            {
                return;
            }

            foreach (var robotParamter in robotParameters)
            {
                var parameterAttribute = robotParamter.GetCustomAttribute<ParameterAttribute>();

                var parameterValue = parameterGrid.FirstOrDefault(iParameter => iParameter.Key.Equals(parameterAttribute.Name,
                    StringComparison.InvariantCultureIgnoreCase)).Value;

                if (parameterValue != null)
                {
                    robotParamter.SetValue(robot, parameterValue);
                }
            }
        }
    }
}