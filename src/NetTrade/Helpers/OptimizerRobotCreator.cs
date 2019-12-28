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
        public static IRobot GetRobot<TRobot>(Dictionary<string, object> parameterSet, IOptimizerSettings settings)
            where TRobot: Robot
        {
            var robot = Activator.CreateInstance(typeof(TRobot), settings) as IRobot;

            if (parameterSet.Any())
            {
                SetRobotParameters(robot, parameterSet);
            }

            return robot as IRobot;
        }

        private static IRobotSettings GetRobotSettings(IOptimizerSettings settings)
        {
            Activator.CreateInstance(settings.RobotSettingsType);
        }


        private static void SetRobotParameters<TRobot>(TRobot robot, Dictionary<string, object> parameterSet) where TRobot: IRobot
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

                robotParamter.SetValue(robot, parameterValue);
            }
        }
    }
}
