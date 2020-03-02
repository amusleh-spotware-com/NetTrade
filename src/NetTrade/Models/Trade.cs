using NetTrade.Abstractions.Interfaces;
using System;
using NetTrade.Enums;

namespace NetTrade.Models
{
    public class Trade : ITrade
    {
        public Trade(TradeParameters parameters)
        {
            Id = parameters.Id;

            Order = parameters.Order;

            ExitTime = parameters.ExitTime;

            ExitPrice = parameters.ExitPrice;

            Equity = parameters.Equity;

            Balance = parameters.Balance;

            SharpeRatio = parameters.SharpeRatio;

            SortinoRatio = parameters.SortinoRatio;

            EquityMaxDrawDown = parameters.EquityMaxDrawDown;

            BalanceMaxDrawDown = parameters.BalanceMaxDrawDown;

            BarsPeriod = parameters.BarsPeriod;

            CloseReason = parameters.CloseReason;
        }

        public MarketOrder Order { get; }

        public DateTimeOffset ExitTime { get; }

        public double ExitPrice { get; }

        public double Equity { get; }

        public double Balance { get; }

        public double SharpeRatio { get; }

        public double SortinoRatio { get; }

        public TimeSpan Duration => Order != null ? ExitTime - Order.OpenTime : TimeSpan.FromSeconds(0);

        public int BarsPeriod { get; }

        public long Id { get; }

        public double EquityMaxDrawDown { get; }

        public double BalanceMaxDrawDown { get; }

        public CloseReason CloseReason { get; }
    }
}