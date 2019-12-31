using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface ITransaction
    {
        double Amount { get; }

        DateTimeOffset Time { get; }

        string Note { get; }
    }
}