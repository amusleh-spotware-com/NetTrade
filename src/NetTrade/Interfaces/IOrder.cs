using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Enums;

namespace NetTrade.Interfaces
{
    public interface IOrder
    {
        OrderType OrderType { get; }

        DateTimeOffset OpenTime { get; }

        double? StopLossPrice { get; }

        double? TakeProfitPrice { get; }

        string Comment { get; }

        long Volume { get; }

        ISymbol Symbol { get; }

        TradeType TradeType { get; }
    }
}
