using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Models;
using NetTrade.Implementations;

namespace NetTrade.Interfaces
{
    public interface ITradeEngine
    {
        IReadOnlyList<IOrder> Orders { get; }

        IReadOnlyList<ITrade> Trades { get; }

        IReadOnlyList<ITradingEvent> Journal { get; }

        TradeResult PlaceOrder(IOrderParameters parameters);

        void UpdateSymbolOrders(ISymbol symbol);

        void AddOrder(IOrder order);

        void CloseMarketOrder(MarketOrder order);

        void CancelPendingOrder(PendingOrder order);
    }
}
