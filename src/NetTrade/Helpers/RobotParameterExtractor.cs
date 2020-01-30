using NetTrade.Abstractions;
using NetTrade.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetTrade.Enums;

namespace NetTrade.Helpers
{
    public static class RobotParameterExtractor
    {
        public static IEnumerable<ParameterAttribute> GetParameters(Robot robot)
        {
            var properties = robot.GetType().GetProperties().Where(iProperty => iProperty.GetCustomAttributes(true).Any());

            var parameters = new List<ParameterAttribute>();

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                var parameterAttribute = property.GetCustomAttribute<ParameterAttribute>();

                parameterAttribute.Type = GetParameterType(property);

                parameters.Add(parameterAttribute);
            }

            return parameters;
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