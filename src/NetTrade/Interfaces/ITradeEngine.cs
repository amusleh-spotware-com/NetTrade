using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Models;

namespace NetTrade.Interfaces
{
    public interface ITradeEngine
    {
        ISymbol Symbol { get; }

        List<IOrder> Orders { get; }

        List<ITrade> Trades { get; }

        TradeResult PlaceOrder(IOrderParameters parameters);
    }
}
