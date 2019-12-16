using NetTrade.Abstractions;
using NetTrade.Enums;
using System;

namespace NetTrade.Implementations
{
    public class PendingOrderParameters : OrderParameters
    {
        public PendingOrderParameters(OrderType orderType) : base(orderType)
        {
            if (orderType == OrderType.Market)
            {
                throw new ArgumentException("The order type for this class can only be set to one of the supported pending order" +
                    " types");
            }
        }

        public double TargetPrice { get; set; }

        public DateTimeOffset? ExpiryTime { get; set; }
    }
}