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
            var symbolPrice = parameters.Symbol.GetPrice(parameters.TradeType);
            var symbolSlippageInPrice = parameters.Symbol.Slippage * parameters.Symbol.TickSize;

            if (TradeType == TradeType.Buy)
            {
                EntryPrice = symbolPrice + symbolSlippageInPrice;
            }
            else
            {
                EntryPrice = symbolPrice - symbolSlippageInPrice;
            }
        }

        public double EntryPrice { get; }
    }
}
