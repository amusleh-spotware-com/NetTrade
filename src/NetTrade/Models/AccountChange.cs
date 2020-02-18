using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using System;

namespace NetTrade.Models
{
    public class AccountChange : IAccountChange
    {
        public AccountChange(double previousValue, double amount, DateTimeOffset time, string note, AccountChangeType type)
        {
            PreviousValue = previousValue;

            Amount = amount;

            Time = time;

            Note = note;

            NewValue = PreviousValue + Amount;

            Type = type;
        }

        public double Amount { get; }

        public DateTimeOffset Time { get; }

        public string Note { get; }

        public double PreviousValue { get; }

        public double NewValue { get; }

        public AccountChangeType Type { get; }
    }
}