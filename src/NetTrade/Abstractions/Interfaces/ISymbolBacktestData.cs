using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface ISymbolBacktestData
    {
        ISymbol Symbol { get; }

        IBar GetBar(DateTimeOffset time);
    }
}