using System.Collections.Generic;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IOptimizeParameter
    {
        string Name { get; }

        object MinValue { get; }

        object MaxValue { get; }

        object Step { get; }

        bool Optimize { get; }

        object DefaultValue { get; }

        IReadOnlyList<object> Values { get; }
    }
}