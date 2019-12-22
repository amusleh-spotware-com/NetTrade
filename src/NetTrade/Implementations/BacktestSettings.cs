using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;

namespace NetTrade.Implementations
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
