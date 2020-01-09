using NetTrade.Abstractions.Interfaces;
using System;

namespace NetTrade.Models
{
    public class Trade : ITrade
    {
        public Trade(MarketOrder order, DateTimeOffset exitTime, double exitPrice)
        {
            Order = order;

            ExitTime = exitTime;

            ExitPrice = exitPrice;
        }

        public MarketOrder Order { get; }

        public DateTimeOffset ExitTime { get; }

        public double ExitPrice { get; }
    }
}