using System.Collections.Generic;

namespace NetTrade.Interfaces
{
    public interface IAccount
    {
        IReadOnlyList<ITransaction> Transactions { get; }

        IReadOnlyList<IAccountBalanceChange> BalanceChanges { get; }

        IReadOnlyList<IAccountEquityChange> EquityChanges { get; }

        double CurrentBalance { get; }

        double Equity { get; }

        long Id { get; }

        long Number { get; }

        string Label { get; }

        long Leverage { get; }

        string BrokerName { get; }

        ITradeEngine Trade { get; }

        void ChangeEquity(IAccountEquityChange change, ITradeEngine tradeEngine);

        void ChangeBalance(IAccountBalanceChange change, ITradeEngine tradeEngine);
    }
}