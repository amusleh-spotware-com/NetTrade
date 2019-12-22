using System;

namespace NetTrade.Interfaces
{
    public interface ITransaction
    {
        double Amount { get; }

        DateTimeOffset Time { get; }

        string Note { get; }
    }
}