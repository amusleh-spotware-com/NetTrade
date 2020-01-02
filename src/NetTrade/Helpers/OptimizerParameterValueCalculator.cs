﻿using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NetTrade.Helpers
{
    public static class OptimizerParameterValueCalculator
    {
        public static List<object> GetAllParameterValues(IOptimizeParameter parameter)
        {
            switch (parameter.Type)
            {
                case ParameterType.Int:
                case ParameterType.Long:
                case ParameterType.Double:
                    return GetAllNumericParameterValues(parameter);

                case ParameterType.Other:
                    return new List<object> { parameter.DefaultValue };

                default:
                    throw new ArgumentException($"The parameter type ({parameter.Type}) is not supported by this method");
            }
        }

        public static long GetParameterRange(IOptimizeParameter parameter)
        {
            switch (parameter.Type)
            {
                case ParameterType.Int:
                case ParameterType.Long:
                case ParameterType.Double:
                    return GetNumericParameterRange(parameter);

                case ParameterType.Other:
                    return 1;

                default:
                    throw new ArgumentException($"The parameter type ({parameter.Type}) is not supported by this method");
            }
        }

        private static List<object> GetAllNumericParameterValues(IOptimizeParameter parameter)
        {
            var result = new List<object>();

            var step = Convert.ToDouble(parameter.Step);

            var stepString = step.ToString(CultureInfo.InvariantCulture);
            var stepIntString = ((int)step).ToString(CultureInfo.InvariantCulture);

            var stepDecimalPoints = stepString.IndexOf('.') >= 0 ? stepString.Length - (stepIntString.Length + 1) : 0;

            var maxValue = Convert.ToDouble(parameter.MaxValue);
            var minValue = Convert.ToDouble(parameter.MinValue);

            for (var iValue = minValue; iValue <= maxValue; iValue += step)
            {
                var valueRounded = Math.Round(iValue, stepDecimalPoints);

                result.Add(valueRounded);
            }

            return result;
        }

        private static long GetNumericParameterRange(IOptimizeParameter parameter)
        {
            var step = Convert.ToDouble(parameter.Step);
            var maxValue = Convert.ToDouble(parameter.MaxValue);
            var minValue = Convert.ToDouble(parameter.MinValue);

            long result = 0;

            for (var iValue = minValue; iValue <= maxValue; iValue += step)
            {
                result++;
            }

            return result;
        }
    }
}