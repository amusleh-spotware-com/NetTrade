using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Enums;
using NetTrade.Abstractions;

namespace NetTrade.Implementations
{
    public class MarketOrder : Order
    {
        public MarketOrder(DateTimeOffset openTime, string comment, double entryPrice): base(OrderType.Market, openTime, comment)
        {
            EntryPrice = entryPrice;
        }

        public double EntryPrice { get; }
    }
}
