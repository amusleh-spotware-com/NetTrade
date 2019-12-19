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

        TradeResult Execute(IOrderParameters parameters);

        void UpdateSymbolOrders(ISymbol symbol);

        void CloseMarketOrder(MarketOrder order);

        void CancelPendingOrder(PendingOrder order);
    }
}
