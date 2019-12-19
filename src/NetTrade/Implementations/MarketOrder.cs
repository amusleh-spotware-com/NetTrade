using NetTrade.Abstractions;
using NetTrade.Enums;

namespace NetTrade.Implementations
{
    public class MarketOrder : Order
    {
        public MarketOrder(MarketOrderParameters parameters) : base(parameters)
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