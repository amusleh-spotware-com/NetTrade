using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Abstractions;
using NetTrade.Enums;
using NetTrade.Interfaces;
using NetTrade.Models;

namespace NetTrade.Implementations
{
    public class MarketOrderParameters: OrderParameters
    {
        public MarketOrderParameters(ISymbol symbol): base(OrderType.Market, symbol)
        {
        }

        public override TradeResult Execute(ITradeEngine tradeEngine)
        {
            var order = new MarketOrder(this);

            tradeEngine.AddOrder(order);

            var result = new TradeResult(order);

            return result;
        }
    }
}
