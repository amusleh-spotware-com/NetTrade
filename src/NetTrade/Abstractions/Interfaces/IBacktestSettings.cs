using System;
using System.Collections.Generic;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IBacktestSettings
    {
        DateTimeOffset StartTime { get; }

        DateTimeOffset EndTime { get; }

        IReadOnlyList<ISymbolBacktestData> SymbolsData { get; }
    }
}