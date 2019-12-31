using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IAccountChange
    {
        double Amount { get; }

        DateTimeOffset Time { get; }

        string Note { get; }

        double PreviousValue { get; }

        double NewValue { get; }
    }
}