using NetTrade.Enums;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IOrderParameters
    {
        ISymbol Symbol { get; }

        TradeType TradeType { get; }

        OrderType OrderType { get; }

        double Volume { get; }

        string Comment { get; }

        double? StopLossPrice { get; }

        double? TakeProfitPrice { get; }
    }
}