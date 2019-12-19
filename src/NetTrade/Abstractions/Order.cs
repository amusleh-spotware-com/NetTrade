using NetTrade.Enums;
using NetTrade.Interfaces;
using System;

namespace NetTrade.Abstractions
{
    public abstract class Order : IOrder
    {
        public Order(IOrderParameters orderParameters)
        {
            Symbol = orderParameters.Symbol;

            TradeType = orderParameters.TradeType;

            OrderType = orderParameters.OrderType;

            Volume = orderParameters.Volume;

            Comment = orderParameters.Comment;

            StopLossPrice = orderParameters.StopLossPrice;

            TakeProfitPrice = orderParameters.TakeProfitPrice;

            OpenTime = DateTimeOffset.Now;
        }

        public TradeType TradeType { get; }

        public OrderType OrderType { get; }

        public DateTimeOffset OpenTime { get; }

        public string Comment { get; }

        public double? StopLossPrice { get; set; }

        public double? TakeProfitPrice { get; set; }

        public long Volume { get; }

        public ISymbol Symbol { get; }
    }
}