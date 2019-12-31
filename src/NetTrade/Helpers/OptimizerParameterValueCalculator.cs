using NetTrade.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NetTrade.Helpers
{
    public static class OptimizerParameterValueCalculator
    {
        public static List<object> GetAllParameterValues(IOptimizeParameter parameter)
        {
            if (parameter.Step is int)
            {
                return GetAllIntParameterValues(parameter);
            }
            else if (parameter.Step is long)
            {
                return GetAllLongParameterValues(parameter);
            }
            else if (parameter.Step is double)
            {
                return GetAllDoubleParameterValues(parameter);
            }

            throw new ArgumentException($"The parameter value type ({parameter.Step.GetType()}) is not supported by this method");
        }

        public static List<object> GetAllDoubleParameterValues(IOptimizeParameter parameter)
        {
            var result = new List<object>();

            var step = (double)parameter.Step;

            var stepString = step.ToString(CultureInfo.InvariantCulture);
            var stepIntString = ((int)step).ToString(CultureInfo.InvariantCulture);

            var stepDecimalPoints = stepString.IndexOf('.') >= 0 ? stepString.Length - (stepIntString.Length + 1) : 0;

            var maxValue = (double)parameter.MaxValue;
            var minValue = (double)parameter.MinValue;

            for (var iValue = minValue; iValue <= maxValue; iValue += step)
            {
                var valueRounded = Math.Round(iValue, stepDecimalPoints);

                result.Add(valueRounded);
            }

            return result;
        }

        public static List<object> GetAllIntParameterValues(IOptimizeParameter parameter)
        {
            var result = new List<object>();

            var step = (int)parameter.Step;

            var maxValue = (int)parameter.MaxValue;
            var minValue = (int)parameter.MinValue;

            for (var iValue = minValue; iValue <= maxValue; iValue += step)
            {
                result.Add(iValue);
            }

            return result;
        }

        public static List<object> GetAllLongParameterValues(IOptimizeParameter parameter)
        {
            var result = new List<object>();

            var step = (long)parameter.Step;

            var maxValue = (long)parameter.MaxValue;
            var minValue = (long)parameter.MinValue;

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