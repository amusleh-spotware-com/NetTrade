using NetTrade.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Implementations
{
    public class Account : IAccount
    {
        private List<ITransaction> _transactions;

        private List<IAccountEquityChange> _equityChanges = new List<IAccountEquityChange>();

        private List<IAccountBalanceChange> _balanceChanges = new List<IAccountBalanceChange>();

        public Account(long id, long number, string label, long leverage, string brokerName,
            IEnumerable<ITransaction> transactions, ITradeEngine tradeEngine)
        {
            Id = id;
            Number = number;
            Label = label;
            Leverage = leverage;
            BrokerName = brokerName;
            _transactions = transactions.ToList();
            Trade = tradeEngine;

            CurrentBalance = _transactions.Sum(iTransaction => iTransaction.Amount);
            Equity = CurrentBalance;
        }

        public IReadOnlyList<ITransaction> Transactions => _transactions;

        public IReadOnlyList<IAccountBalanceChange> BalanceChanges => _balanceChanges;

        public IReadOnlyList<IAccountEquityChange> EquityChanges => _equityChanges;

        public double CurrentBalance { get; private set; }

        public double Equity { get; private set; }

        public long Id { get; }

        public long Number { get; }

        public string Label { get; }

        public long Leverage { get; }

        public string BrokerName { get; }

        public ITradeEngine Trade { get; }

        public void ChangeBalance(IAccountBalanceChange change, ITradeEngine tradeEngine)
        {
            if (tradeEngine != Trade)
            {
                throw new ArgumentException("The provided trade engine doesn't match the account trade engine");
            }

            _balanceChanges.Add(change);

            CurrentBalance += change.Amount;
        }

        public void ChangeEquity(IAccountEquityChange change, ITradeEngine tradeEngine)
        {
            if (tradeEngine != Trade)
            {
                throw new ArgumentException("The provided trade engine doesn't match the account trade engine");
            }

            _equityChanges.Add(change);

            Equity += change.Amount;
        }

        public void AddTransaction(ITransaction transaction)
        {
            _transactions.Add(transaction);

            var balanceChange = new AccountBalanceChange(transaction.Amount, transaction.Time, transaction.Note);

            ChangeBalance(balanceChange, Trade);

            var equityChange = new AccountEquityChange(transaction.Amount, transaction.Time, transaction.Note);

            ChangeEquity(equityChange, Trade);
        }
    }
}