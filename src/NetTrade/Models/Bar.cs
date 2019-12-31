using NetTrade.Abstractions.Interfaces;
using System;

namespace NetTrade.Models
{
    public class Bar : IBar
    {
        public Bar(DateTimeOffset time, double open, double high, double low, double close, long volume)
        {
            Time = time;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public DateTimeOffset Time { get; }

        public double Open { get; }

        public double High { get; }

        public double Low { get; }

        public double Close { get; }

        public long Volume { get; }
    }
}