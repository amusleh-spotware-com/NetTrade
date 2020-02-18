using NetTrade.Enums;
using System.Collections.Generic;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IOptimizeParameter
    {
        string Name { get; }

        object MinValue { get; }

        object MaxValue { get; }

        object Step { get; }

        IReadOnlyList<object> Values { get; }

        ParameterType Type { get; }
    }
}