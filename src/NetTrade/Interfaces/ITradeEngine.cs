using NetTrade.Implementations;
using NetTrade.Models;
using System.Collections.Generic;
using NetTrade.Helpers;

namespace NetTrade.Interfaces
{
    public interface ITradeEngine
    {
        IReadOnlyList<IOrder> Orders { get; }

        IReadOnlyList<ITrade> Trades { get; }

        IReadOnlyList<ITradingEvent> Journal { get; }

        IServer Server { get; }

        event OnEquityChangedHandler OnEquityChangedHandlerEvent;

        event OnBalanceChangedHandler OnBalanceChangedHandlerEvent;

        TradeResult Execute(IOrderParameters parameters);

        void UpdateSymbolOrders(ISymbol symbol);

        void CloseMarketOrder(MarketOrder order);

        void CancelPendingOrder(PendingOrder order);
    }
}