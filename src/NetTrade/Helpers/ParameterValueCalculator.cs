using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;

namespace NetTrade.Helpers
{
    public static class ParameterValueCalculator
    {
        public static List<object> GetAllParameterValues(IOptimizeParameter parameter)
        {
            var result = new List<object>();

            var step = (double)parameter.Step;
            var maxValue = (double)parameter.MaxValue;
            var minValue = (double)parameter.MinValue;

            for (var iValue = minValue; iValue <= maxValue; iValue += step)
            {
                result.Add(iValue);
            }

            return result;
        }

        public static long GetParameterRange(IOptimizeParameter parameter)
        {
            if (parameter.Step is int || parameter.Step is double || parameter.Step is long)
            {
                return GetNumericParameterRange(parameter);
            }

            throw new NotSupportedException("The parameter value type is not supported");
        }

        private static long GetNumericParameterRange(IOptimizeParameter parameter)
        {
            var step = (double)parameter.Step;
            var maxValue = (double)parameter.MaxValue;
            var minValue = (double)parameter.MinValue;

            long result = 0;

            for (var iValue = minValue; iValue <= maxValue; iValue += step)
            {
                result++;
            }

            return result;
        }
    }
}
