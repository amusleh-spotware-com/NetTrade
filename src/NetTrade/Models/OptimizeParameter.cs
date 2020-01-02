using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Helpers;
using System.Collections.Generic;

namespace NetTrade.Models
{
    public class OptimizeParameter : IOptimizeParameter
    {
        public OptimizeParameter(string name, int minValue, int maxValue, int step) 
        {
            Type = ParameterType.Int;

            SetParameterValues(name, minValue, maxValue, step);
        }

        public OptimizeParameter(string name, long minValue, long maxValue, long step)
        {
            Type = ParameterType.Long;

            SetParameterValues(name, minValue, maxValue, step);
        }

        public OptimizeParameter(string name, double minValue, double maxValue, double step)
        {
            Type = ParameterType.Double;

            SetParameterValues(name, minValue, maxValue, step);
        }

        public OptimizeParameter(string name, object defaultValue)
        {
            Name = name;
            DefaultValue = defaultValue;

            Optimize = false;

            Values = new List<object> { defaultValue };

            Type = ParameterType.Other;
        }

        public string Name { get; private set; }

        public object MinValue { get; private set; }

        public object MaxValue { get; private set; }

        public object Step { get; private set; }

        public bool Optimize { get; }

        public object DefaultValue { get; }

        public IReadOnlyList<object> Values { get; private set; }

        public ParameterType Type { get; }

        private void SetParameterValues(string name, object minValue, object maxValue, object step)
        {
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
            Step = step;

            Values = OptimizerParameterValueCalculator.GetAllParameterValues(this);
        }
    }
}