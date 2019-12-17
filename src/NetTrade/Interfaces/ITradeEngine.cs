using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Models;

namespace NetTrade.Interfaces
{
    public interface ITradeEngine
    {
        ISeries<IOrder> Orders { get; }

        ISeries<ITrade> Trades { get; }

        TradeResult PlaceOrder(IOrderParameters parameters);

        void UpdateSymbolOrders(ISymbol symbol);

        void AddOrder(IOrder order);
    }
}
