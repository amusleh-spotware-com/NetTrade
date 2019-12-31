using System.Collections.Generic;

namespace NetTrade.Abstractions.Interfaces
{
    public interface ISeries<T> : IReadOnlyList<T>
    {
        T LastValue { get; }

        T Last(int index);
    }
}