using System;

namespace NetTrade.Interfaces
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