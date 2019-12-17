using NetTrade.Abstractions;
using NetTrade.Enums;
using NetTrade.Interfaces;
using NetTrade.Models;
using System;

namespace NetTrade.Implementations
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

        public override TradeResult Execute(ITradeEngine tradeEngine)
        {
            double price = Symbol.GetPrice(TradeType);

            bool isPriceValid = true;

            switch (OrderType)
            {
                case OrderType.Limit:
                    isPriceValid = TradeType == TradeType.Buy ? TargetPrice < price : TargetPrice > price;
                    break;

                case OrderType.Stop:
                    isPriceValid = TradeType == TradeType.Buy ? TargetPrice > price : TargetPrice < price;
                    break;
            }

            if (isPriceValid)
            {
                var order = new PendingOrder(this);

                return new TradeResult(order);
            }

            return new TradeResult(OrderErrorCode.InvalidTargetPrice);
        }
    }
}