using NetTrade.Implementations;
using System;

namespace NetTrade.Interfaces
{
    public interface ITrade
    {
        MarketOrder Order { get; }

        DateTimeOffset ExitTime { get; }
    }
}