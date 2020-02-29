using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using System;

namespace NetTrade.Models
{
    public class PendingOrderParameters : OrderParameters
    {
        public PendingOrderParameters(OrderType orderType, ISymbol symbol) : base(orderType, symbol)
        {
            if (orderType == OrderType.Market)
            {
                throw new ArgumentException("The order type for this class can only be set to one of the supported pending order" +
                    " types");
            }
        }

        public double TargetPrice { get; set; }

        public DateTimeOffset? ExpiryTime { get; set; }

        public double? StopLossPrice { get; set; }

        public double? TakeProfitPrice { get; set; }
    }
}