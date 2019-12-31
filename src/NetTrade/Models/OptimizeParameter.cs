using NetTrade.Abstractions.Interfaces;
using NetTrade.Helpers;
using System.Collections.Generic;

namespace NetTrade.Models
{
    public class OptimizeParameter : IOptimizeParameter
    {
        public OptimizeParameter(string name, object minValue, object maxValue, object step)
        {
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
            Step = step;

            Values = OptimizerParameterValueCalculator.GetAllParameterValues(this);
        }

        public OptimizeParameter(string name, object defaultValue)
        {
            Name = name;
            DefaultValue = defaultValue;

            Optimize = false;

            Values = new List<object> { defaultValue };
        }

        public string Name { get; }

        public object MinValue { get; }

        public object MaxValue { get; }

        public object Step { get; }

        public bool Optimize { get; }

        public object DefaultValue { get; }

        public IReadOnlyList<object> Values { get; }
    }
}