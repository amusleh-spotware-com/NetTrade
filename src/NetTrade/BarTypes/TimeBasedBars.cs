using NetTrade.Abstractions;
using System;

namespace NetTrade.BarTypes
{
    public class TimeBasedBars : Bars
    {
        public TimeBasedBars(TimeSpan timeFrame)
        {
            TimeFrame = timeFrame;
        }

        public TimeSpan TimeFrame { get; }

        public override object Clone()
        {
            var clone = new TimeBasedBars(TimeFrame);

            return clone;
        }
    }
}