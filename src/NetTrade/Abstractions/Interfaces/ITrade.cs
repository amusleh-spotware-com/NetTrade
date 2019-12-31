using NetTrade.Models;
using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface ITrade
    {
        MarketOrder Order { get; }

        DateTimeOffset ExitTime { get; }
    }
}