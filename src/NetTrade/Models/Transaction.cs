using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Models
{
    public class Transaction
    {
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
