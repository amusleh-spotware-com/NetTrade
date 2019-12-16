using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;

namespace NetTrade.Implementations
{
    public class Trade : ITrade
    {
        public Trade(IOrder order, double grossProfit, double netProfit, DateTimeOffset exitTime)
        {
            Order = order;

            GrossProfit = grossProfit;

            NetProfit = netProfit;

            ExitTime = exitTime;
        }

        public IOrder Order { get; }

        public double GrossProfit { get; }

        public double NetProfit { get; }

        public DateTimeOffset ExitTime { get; }
    }
}
