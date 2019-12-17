using NetTrade.Models;
using System.Collections.Generic;

namespace NetTrade.Interfaces
{
    public interface IAccount
    {
        List<Transaction> Transactions { get; }

        double CurrentBalance { get; }

        double Equity { get; }

        long Id { get; }

        long Number { get; }

        string Label { get; }

        long Leverage { get; }

        string BrokerName { get; }

        ITradeEngine Trade { get; }
    }
}