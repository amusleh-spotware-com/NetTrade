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

        public double Commission { get; set; }

        public double GrossProfit { get; set; }

        public double NetProfit { get; set; }
    }
}