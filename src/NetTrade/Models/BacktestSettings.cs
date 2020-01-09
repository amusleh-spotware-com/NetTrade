using NetTrade.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Models
{
    public class BacktestSettings : IBacktestSettings
    {
        public BacktestSettings(DateTimeOffset startTime, DateTimeOffset endTime, IEnumerable<ISymbolBacktestData> symbolsData)
        {
            StartTime = startTime;
            EndTime = endTime;

            SymbolsData = symbolsData.ToList();
        }

        public DateTimeOffset StartTime { get; }

        public DateTimeOffset EndTime { get; }

        public IReadOnlyList<ISymbolBacktestData> SymbolsData { get; }
    }
}