using NetTrade.Abstractions.Interfaces;
using System;

namespace NetTrade.Models
{
    public class Trade : ITrade
    {
        public Trade(MarketOrder order, DateTimeOffset exitTime)
        {
            Order = order;

            ExitTime = exitTime;
        }

        public MarketOrder Order { get; }

        public DateTimeOffset ExitTime { get; }
    }
}