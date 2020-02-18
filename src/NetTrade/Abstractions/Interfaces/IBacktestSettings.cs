using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IBacktestSettings
    {
        DateTimeOffset StartTime { get; }

        DateTimeOffset EndTime { get; }
    }
}