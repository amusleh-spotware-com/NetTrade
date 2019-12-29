using NetTrade.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Implementations
{
    public class Account : IAccount
    {
        private List<ITransaction> _transactions;

        private List<IAccountChange> _equityChanges = new List<IAccountChange>();

        private List<IAccountChange> _balanceChanges = new List<IAccountChange>();

        public Account(long id, long number, string label, long leverage, string brokerName,
            IEnumerable<ITransaction> transactions, ITradeEngine tradeEngine)
        {
            Id = id;
            Number = number;
            Label = label;
            Leverage = leverage;
            BrokerName = brokerName;
            _transactions = transactions.ToList();

            CurrentBalance = _transactions.Sum(iTransaction => iTransaction.Amount);
            Equity = CurrentBalance;

            Trade = tradeEngine;

            Trade.OnBalanceChangedHandlerEvent += Trade_OnBalanceChangedHandlerEvent;
            Trade.OnEquityChangedHandlerEvent += Trade_OnEquityChangedHandlerEvent;
        }

        public IReadOnlyList<ITransaction> Transactions => _transactions;

        public IReadOnlyList<IAccountChange> BalanceChanges => _balanceChanges;

        public IReadOnlyList<IAccountChange> EquityChanges => _equityChanges;

        public double CurrentBalance { get; private set; }

        public double Equity { get; private set; }

        public long Id { get; }

        public long Number { get; }

        public string Label { get; }

        public long Leverage { get; }

        public string BrokerName { get; }

        public ITradeEngine Trade { get; }

        public void AddTransaction(ITransaction transaction)
        {
            _transactions.Add(transaction);

            var balanceChange = new AccountChange(CurrentBalance, transaction.Amount, transaction.Time, transaction.Note);

            ChangeBalance(balanceChange);

            var equityChange = new AccountChange(Equity, transaction.Amount, transaction.Time, transaction.Note);

            ChangeEquity(equityChange);
        }

        private void Trade_OnEquityChangedHandlerEvent(object sender, double amount, DateTimeOffset time)
        {
            var change = new AccountChange(Equity, amount, time, string.Empty);

            ChangeEquity(change);
        }

        private void Trade_OnBalanceChangedHandlerEvent(object sender, double amount, DateTimeOffset time)
        {
            var change = new AccountChange(CurrentBalance, amount, time, string.Empty);

            ChangeBalance(change);
        }

        private void ChangeBalance(IAccountChange change)
        {
            _balanceChanges.Add(change);

            CurrentBalance = change.NewValue;
        }

        private void ChangeEquity(IAccountChange change)
        {
            _equityChanges.Add(change);

            Equity = change.NewValue;
        }
    }
}