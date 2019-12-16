using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Enums;
using NetTrade.Abstractions;

namespace NetTrade.Implementations
{
    public class PendingOrder : Order
    {
        public PendingOrder(OrderType orderType,DateTimeOffset openTime, string comment): base(orderType, openTime, comment)
        {
        }

        public double TargetPrice { get; set; }

        public DateTimeOffset? ExpiryTime { get; set; }
    }
}
