using NetTrade.Abstractions;
using System;

namespace NetTrade.Models
{
    public class MarketOrder : Order
    {
        public MarketOrder(double entryPrice, MarketOrderParameters parameters, DateTimeOffset openTime) :
            base(parameters, openTime)
        {
            EntryPrice = entryPrice;
        }

        public double EntryPrice { get; }

        public double Commission { get; set; }

        public double GrossProfit { get; set; }

        public double NetProfit { get; set; }

        public double MarginUsed { get; set; }
    }
}