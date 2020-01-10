using NetTrade.Abstractions.Interfaces;
using System;

namespace NetTrade.Models
{
    public class BacktestSettings : IBacktestSettings
    {
        public BacktestSettings(DateTimeOffset startTime, DateTimeOffset endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public DateTimeOffset StartTime { get; }

        public DateTimeOffset EndTime { get; }
    }
}