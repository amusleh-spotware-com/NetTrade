using NetTrade.Interfaces;
using System;

namespace NetTrade.Implementations
{
    public class AccountEquityChange : IAccountEquityChange
    {
        public AccountEquityChange(double amount, DateTimeOffset time, string note)
        {
            Amount = amount;

            Time = time;

            Note = note;
        }

        public double Amount { get; }

        public DateTimeOffset Time { get; }

        public string Note { get; }
    }
}