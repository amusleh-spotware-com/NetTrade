using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Enums;
using NetTrade.Abstractions;
using NetTrade.Interfaces;

namespace NetTrade.Implementations
{
    public class MarketOrder : Order
    {
        public MarketOrder(MarketOrderParameters parameters): base(parameters)
        {
            EntryPrice = parameters.Symbol.GetPrice(parameters.TradeType);
        }

        public double EntryPrice { get; }
    }
}
