using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface ISymbolBacktestData: ICloneable
    {
        ISymbol Symbol { get; }

        IBar GetBar(DateTimeOffset time);
    }
}