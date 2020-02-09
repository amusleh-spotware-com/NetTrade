using NetTrade.Abstractions;
using NetTrade.Attributes;
using NetTrade.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace NetTrade.Helpers
{
    public static class RobotParameterTools
    {
        public static IEnumerable<ParameterAttribute> GetParameters(Type robotType)
        {
            var properties = robotType.GetProperties().Where(iProperty => iProperty.GetCustomAttributes(true).Any());

            var parameters = new List<ParameterAttribute>();

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                var parameterAttribute = property.GetCustomAttribute<ParameterAttribute>();

                parameterAttribute.Type = GetParameterType(property);
                parameterAttribute.Property = property;

                parameters.Add(parameterAttribute);
            }

            return parameters;
        }

        public static void SetParameterValuesToDefault(Robot robot)
        {
            var robotParameters = GetParameters(robot.GetType());

            if (!robotParameters.Any())
            {
                return;
            }

            foreach (var robotParamter in robotParameters)
            {
                if (!robotParamter.Property.CanWrite || robotParamter.DefaultValue == null)
                {
                    continue;
                }

                object defaultValue = null;

                switch (robotParamter.Type)
                {
                    case ParameterType.DateTime:
                        defaultValue = DateTimeOffset.ParseExact(robotParamter.DefaultValue.ToString(), "o", CultureInfo.InvariantCulture);
                        break;

                    case ParameterType.Time:
                        defaultValue = TimeSpan.Parse(robotParamter.DefaultValue.ToString(), CultureInfo.InvariantCulture);
                        break;

                    case ParameterType.Boolean:
                    case ParameterType.Double:
                    case ParameterType.Int:
                    case ParameterType.Long:
                    case ParameterType.Enum:
                    case ParameterType.String:
                    case ParameterType.Other:
                        defaultValue = robotParamter.DefaultValue;
                        break;
                }

                robotParamter.Property.SetValue(robot, defaultValue);
            }
        }

        private static ParameterType GetParameterType(PropertyInfo property)
        {
            if (property.PropertyType == typeof(int))
            {
                return ParameterType.Int;
            }
            else if (property.PropertyType == typeof(long))
            {
                return ParameterType.Long;
            }
            else if (property.PropertyType == typeof(double))
            {
                return ParameterType.Double;
            }
            else if (property.PropertyType == typeof(bool))
            {
                return ParameterType.Boolean;
            }
            else if (property.PropertyType == typeof(TimeSpan))
            {
                return ParameterType.Time;
            }
            else if (property.PropertyType == typeof(DateTimeOffset))
            {
                return ParameterType.DateTime;
            }
            else if (property.PropertyType == typeof(string))
            {
                return ParameterType.String;
            }
            else if (property.PropertyType.IsEnum)
            {
                return ParameterType.Enum;
            }
            else
            {
                return ParameterType.Other;
            }
        }
    }
}