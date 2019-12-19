using NetTrade.Abstractions;

namespace NetTrade.Implementations
{
    public class MarketOrder : Order
    {
        public MarketOrder(MarketOrderParameters parameters) : base(parameters)
        {
            EntryPrice = parameters.EntryPrice;
        }

        public double EntryPrice { get; }
    }
}