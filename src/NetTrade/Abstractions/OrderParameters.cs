using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;
using NetTrade.Enums;

namespace NetTrade.Abstractions
{
    public abstract class OrderParameters: IOrderParameters
    {
        public OrderParameters(OrderType orderType)
        {
            OrderType = orderType;
        }

        public OrderType OrderType { get; }

        public long Volume { get; set; }

        public TradeType TradeType { get; set; }

        public double? StopLossPrice { get; set; }

        public double? TakeProfitPrice { get; set; }

        public string Comment { get; set; }
    }
}
