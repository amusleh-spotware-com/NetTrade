using NetTrade.Enums;
using NetTrade.Interfaces;
using System;

namespace NetTrade.Abstractions
{
    public abstract class Order : IOrder
    {
        public Order(OrderType orderType, DateTimeOffset openTime, string comment)
        {
            OrderType = orderType;

            OpenTime = openTime;

            Comment = comment;
        }

        public OrderType OrderType { get; }

        public DateTimeOffset OpenTime { get; }

        public string Comment { get; }

        public double? StopLossPrice { get; set; }

        public double? TakeProfitPrice { get; set; }
    }
}