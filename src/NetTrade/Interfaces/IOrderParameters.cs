using NetTrade.Enums;

namespace NetTrade.Interfaces
{
    public interface IOrderParameters
    {
        ISymbol Symbol { get; }

        TradeType TradeType { get; }

        OrderType OrderType { get; }

        long Volume { get; }

        string Comment { get; }

        double? StopLossPrice { get; }

        double? TakeProfitPrice { get; }
    }
}