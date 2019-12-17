using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Models;
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

        TradeResult Execute(ITradeEngine tradeEngine);
    }
}
