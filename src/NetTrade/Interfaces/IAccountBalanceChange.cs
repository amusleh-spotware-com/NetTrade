using System;

namespace NetTrade.Interfaces
{
    public interface IAccountBalanceChange
    {
        double Amount { get; }

        DateTimeOffset Time { get; }

        string Note { get; }
    }
}