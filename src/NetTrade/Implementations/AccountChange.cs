using NetTrade.Interfaces;
using System;

namespace NetTrade.Implementations
{
    public class AccountChange : IAccountChange
    {
        public AccountChange(double previousValue, double amount, DateTimeOffset time, string note)
        {
            PreviousValue = previousValue;

            Amount = amount;

            Time = time;

            Note = note;

            NewValue = PreviousValue + Amount;
        }

        public double Amount { get; }

        public DateTimeOffset Time { get; }

        public string Note { get; }

        public double PreviousValue { get; }

        public double NewValue { get; }
    }
}