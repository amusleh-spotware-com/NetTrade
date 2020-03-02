using System;
using NetTrade.Enums;

namespace NetTrade.Models
{
    public class TradeParameters
    {
        public MarketOrder Order { get; set; }

        public DateTimeOffset ExitTime { get; set; }

        public double ExitPrice { get; set; }

        public double Equity { get; set; }

        public double Balance { get; set; }

        public double SharpeRatio { get; set; }

        public double SortinoRatio { get; set; }

        public int BarsPeriod { get; set; }

        public long Id { get; set; }

        public double EquityMaxDrawDown { get; set; }

        public double BalanceMaxDrawDown { get; set; }

        public CloseReason CloseReason { get; set; }
    }
}