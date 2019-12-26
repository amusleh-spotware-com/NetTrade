using NetTrade.Interfaces;

namespace NetTrade.Implementations
{
    public class OptimizeParameter : IOptimizeParameter
    {
        public OptimizeParameter(string name, object minValue, object maxValue, object step)
        {
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
            Step = step;
        }

        public string Name { get; }

        public object MinValue { get; }

        public object MaxValue { get; }

        public object Step { get; }
    }
}