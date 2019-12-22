using NetTrade.Abstractions;
using System;

namespace NetTrade.Implementations
{
    public class PendingOrder : Order
    {
        public PendingOrder(PendingOrderParameters parameters, DateTimeOffset openTime) : base(parameters, openTime)
        {
            TargetPrice = parameters.TargetPrice;
            ExpiryTime = parameters.ExpiryTime;
        }

        public double TargetPrice { get; set; }

        public DateTimeOffset? ExpiryTime { get; set; }
    }
}