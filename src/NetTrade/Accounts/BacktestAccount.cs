using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Models;
using System;
using System.Collections.Generic;

namespace NetTrade.Accounts
{
    public class BacktestAccount : IAccount
    {
        private readonly List<ITransaction> _transactions = new List<ITransaction>();

        private readonly List<IAccountChange> _equityChanges = new List<IAccountChange>();

        private readonly List<IAccountChange> _balanceChanges = new List<IAccountChange>();

        private readonly List<IAccountChange> _marginChanges = new List<IAccountChange>();

        public BacktestAccount(long id, long number, string label, long leverage, string brokerName)
        {
            Id = id;
            Number = number;
            Label = label;
            Leverage = leverage;
            BrokerName = brokerName;
        }

        public IReadOnlyList<ITransaction> Transactions => _transactions;

        public IReadOnlyList<IAccountChange> BalanceChanges => _balanceChanges;

        public IReadOnlyList<IAccountChange> EquityChanges => _equityChanges;

        public IReadOnlyList<IAccountChange> MarginChanges => _marginChanges;

        public double CurrentBalance { get; private set; }

        public double Equity { get; private set; }

        public double UsedMargin { get; private set; }

        public double FreeMargin => Equity - UsedMargin;

        public long Id { get; }

        public long Number { get; }

        public string Label { get; }

        public long Leverage { get; }

        public string BrokerName { get; }

        public double MarginCallPercentage { get; set; } = 0;

        public event OnMarginCallHandler OnMarginCallEvent;

        public void AddTransaction(ITransaction transaction)
        {
            var changeType = transaction.Amount > 0 ? AccountChangeType.Deposit : AccountChangeType.Withdrawal;

            ChangeBalance(transaction.Amount, transaction.Time, transaction.Note, changeType);

            ChangeEquity(transaction.Amount, transaction.Time, transaction.Note, changeType);

            _transactions.Add(transaction);

            CheckForMarginCall();
        }

        public void ChangeBalance(double amount, DateTimeOffset time, string note, AccountChangeType type)
        {
            var change = new AccountChange(CurrentBalance, amount, time, note, type);

            _balanceChanges.Add(change);

            CurrentBalance = change.NewValue;
        }

        public void ChangeEquity(double amount, DateTimeOffset time, string note, AccountChangeType type)
        {
            var change = new AccountChange(Equity, amount, time, note, type);

            _equityChanges.Add(change);

            Equity = change.NewValue;

            CheckForMarginCall();
        }

        public void ChangeMargin(double amount, DateTimeOffset time, string note, AccountChangeType type)
        {
            var change = new AccountChange(UsedMargin, amount, time, note, type);

            _marginChanges.Add(change);

            UsedMargin = change.NewValue;

            CheckForMarginCall();
        }

        private void CheckForMarginCall()
        {
            var marginPercentage = (FreeMargin / CurrentBalance) * 100;

            if (marginPercentage <= MarginCallPercentage)
            {
                OnMarginCallEvent?.Invoke(this);
            }
        }
    }
}