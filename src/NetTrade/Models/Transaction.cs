using NetTrade.Abstractions.Interfaces;
using System;

namespace NetTrade.Models
{
    public class Transaction : ITransaction
    {
        public Transaction(double amount, DateTimeOffset time): this(amount, time, string.Empty)
        {
        }

        public Transaction(double amount, DateTimeOffset time, string note)
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