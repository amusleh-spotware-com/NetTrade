using System;

namespace NetTrade.Interfaces
{
    public interface IBar
    {
        DateTimeOffset Time { get; }

        double Open { get; }

        double High { get; }

        double Low { get; }

        double Close { get; }

        long Volume { get; }
    }
}