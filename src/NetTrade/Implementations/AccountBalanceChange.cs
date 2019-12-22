using NetTrade.Interfaces;
using System;

namespace NetTrade.Implementations
{
    public class AccountBalanceChange : IAccountBalanceChange
    {
        public AccountBalanceChange(double amount, DateTimeOffset time, string note)
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