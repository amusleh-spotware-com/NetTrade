using NetTrade.Interfaces;
using NetTrade.Models;
using System.Collections.Generic;

namespace NetTrade.Implementations
{
    public class Account : IAccount
    {
        public Account(long id, long number, string label, long leverage, string brokerName, ITradeEngine tradeEngine)
        {
            Id = id;
            Number = number;
            Label = label;
            Leverage = leverage;
            BrokerName = brokerName;
            Trade = tradeEngine;
        }

        public List<Transaction> Transactions { get; } = new List<Transaction>();

        public double CurrentBalance { get; set; }

        public double Equity { get; set; }

        public long Id { get; }

        public long Number { get; }

        public string Label { get; }

        public long Leverage { get; }

        public string BrokerName { get; }

        public ITradeEngine Trade { get; }
    }
}