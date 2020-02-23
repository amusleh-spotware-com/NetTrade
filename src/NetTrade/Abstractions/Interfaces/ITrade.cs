using NetTrade.Models;
using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface ITrade
    {
        MarketOrder Order { get; }

        DateTimeOffset ExitTime { get; }

        double ExitPrice { get; }

        double Equity { get; }

        double Balance { get; }

        double SharpeRatio { get; }

        double SortinoRatio { get; }

        TimeSpan Duration { get; }

        double MaxDrawDown { get; }
    }
}