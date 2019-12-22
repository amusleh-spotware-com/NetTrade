using System;

namespace NetTrade.Interfaces
{
    public interface IBacktestSettings
    {
        DateTimeOffset StartTime { get; }

        DateTimeOffset EndTime { get; }
    }
}