using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;
using NetTrade.Abstractions;
using System.Linq;
using NetTrade.Implementations;
using System.Reflection;

namespace NetTrade.Helpers
{
    public static class OptimizerRobotCreator
    {
        public static Robot GetRobot<TRobot>(Dictionary<string, object> parameterSet, IOptimizer optimizer)
            where TRobot: Robot
        {
            var robotSettings = optimizer.GetRobotSettings();

            var robot = Activator.CreateInstance(typeof(TRobot), robotSettings) as Robot;

            if (parameterSet.Any())
            {
                SetRobotParameters(robot, parameterSet);
            }

            return robot;
        }


        private static void SetRobotParameters<TRobot>(TRobot robot, Dictionary<string, object> parameterSet)
            where TRobot: IRobot
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

                var parameterValue = parameterSet.FirstOrDefault(iParameter => iParameter.Key.Equals(parameterAttribute.Name,
                    StringComparison.InvariantCultureIgnoreCase)).Value;

                if (parameterValue != null)
                {
                    robotParamter.SetValue(robot, parameterValue);
                }
            }
        }
    }
}
