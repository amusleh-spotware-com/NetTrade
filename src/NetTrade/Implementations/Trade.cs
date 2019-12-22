using NetTrade.Interfaces;
using System;

namespace NetTrade.Implementations
{
    public class Trade : ITrade
    {
        public Trade(IOrder order, DateTimeOffset exitTime)
        {
            Order = order;

            ExitTime = exitTime;
        }

        public IOrder Order { get; }

        public DateTimeOffset ExitTime { get; }
    }
}