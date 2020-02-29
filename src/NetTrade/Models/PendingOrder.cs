using NetTrade.Abstractions;
using System;

namespace NetTrade.Models
{
    public class PendingOrder : Order
    {
        public PendingOrder(PendingOrderParameters parameters, DateTimeOffset openTime) : base(parameters, openTime)
        {
            TargetPrice = parameters.TargetPrice;
            ExpiryTime = parameters.ExpiryTime;

            StopLossPrice = parameters.StopLossPrice;

            TakeProfitPrice = parameters.TakeProfitPrice;
        }

        public double TargetPrice { get; set; }

        public DateTimeOffset? ExpiryTime { get; set; }
    }
}