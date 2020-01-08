using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;

namespace NetTrade.Models
{
    public class MarketOrderParameters : OrderParameters
    {
        public MarketOrderParameters(ISymbol symbol) : base(OrderType.Market, symbol)
        {
        }
    }
}