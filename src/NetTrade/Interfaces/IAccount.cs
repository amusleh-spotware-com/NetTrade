using System.Collections.Generic;

namespace NetTrade.Interfaces
{
    public interface IAccount
    {
        IReadOnlyList<ITransaction> Transactions { get; }

        IReadOnlyList<IAccountChange> BalanceChanges { get; }

        IReadOnlyList<IAccountChange> EquityChanges { get; }

        double CurrentBalance { get; }

        double Equity { get; }

        long Id { get; }

        long Number { get; }

        string Label { get; }

        long Leverage { get; }

        string BrokerName { get; }

        ITradeEngine Trade { get; }

        void ChangeEquity(IAccountChange change, ITradeEngine tradeEngine);

        void ChangeBalance(IAccountChange change, ITradeEngine tradeEngine);
    }
}