using NetTrade.Interfaces;
using System;

namespace NetTrade.Implementations
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