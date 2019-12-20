using NetTrade.Abstractions;
using System;

namespace NetTrade.Implementations
{
    public class TimeBasedBars : Bars
    {
        public TimeBasedBars(TimeSpan timeFrame)
        {
            TimeFrame = timeFrame;
        }

        public TimeSpan TimeFrame { get; }
    }
}