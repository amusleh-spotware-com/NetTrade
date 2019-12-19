using NetTrade.Enums;
using NetTrade.Interfaces;

namespace NetTrade.Abstractions
{
    public abstract class OrderParameters : IOrderParameters
    {
        public OrderParameters(OrderType orderType, ISymbol symbol)
        {
            OrderType = orderType;

            Symbol = symbol;
        }

        public OrderType OrderType { get; }

        public long Volume { get; set; }

        public TradeType TradeType { get; set; }

        public double? StopLossPrice { get; set; }

        public double? TakeProfitPrice { get; set; }

        public string Comment { get; set; }

        public ISymbol Symbol { get; }
    }
}