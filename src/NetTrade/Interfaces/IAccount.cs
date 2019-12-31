using System.Collections.Generic;
using NetTrade.Helpers;
using System;

namespace NetTrade.Interfaces
{
    public interface IAccount
    {
        IReadOnlyList<ITransaction> Transactions { get; }

        IReadOnlyList<IAccountChange> BalanceChanges { get; }

        IReadOnlyList<IAccountChange> EquityChanges { get; }

        IReadOnlyList<IAccountChange> MarginChanges { get; }

        double CurrentBalance { get; }

        double Equity { get; }

        double UsedMargin { get; }

        double FreeMargin { get; }

        long Id { get; }

        long Number { get; }

        string Label { get; }

        long Leverage { get; }

        string BrokerName { get; }

        double MarginCallPercentage { get; }

        event OnMarginCallHandler OnMarginCallEvent;

        void AddTransaction(ITransaction transaction);

        void ChangeBalance(double amount, DateTimeOffset time, string note);

        void ChangeEquity(double amount, DateTimeOffset time, string note);

        void ChangeMargin(double amount, DateTimeOffset time, string note);
    }
}