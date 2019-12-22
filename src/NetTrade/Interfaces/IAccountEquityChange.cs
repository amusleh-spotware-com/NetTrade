using System;

namespace NetTrade.Interfaces
{
    public interface IAccountEquityChange
    {
        double Amount { get; }

        DateTimeOffset Time { get; }

        string Note { get; }
    }
}