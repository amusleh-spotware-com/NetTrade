using NetTrade.Implementations;
using NetTrade.Models;
using System.Collections.Generic;

namespace NetTrade.Interfaces
{
    public interface ITradeEngine
    {
        IReadOnlyList<IOrder> Orders { get; }

        IReadOnlyList<ITrade> Trades { get; }

        IReadOnlyList<ITradingEvent> Journal { get; }

        IAccount Account { get; }

        IRobot Robot { get; }

        TradeResult Execute(IOrderParameters parameters);

        void UpdateSymbolOrders(ISymbol symbol);

        void CloseMarketOrder(MarketOrder order);

        void CancelPendingOrder(PendingOrder order);
    }
}