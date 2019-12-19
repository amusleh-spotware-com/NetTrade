using NetTrade.Abstractions;
using NetTrade.Enums;
using NetTrade.Interfaces;

namespace NetTrade.Implementations
{
    public class MarketOrderParameters : OrderParameters
    {
        public MarketOrderParameters(ISymbol symbol) : base(OrderType.Market, symbol)
        {
        }

        public double EntryPrice { get; set; }
    }
}