using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Abstractions
{
    public abstract class TradeEngine : ITradeEngine
    {
        #region Fields

        protected readonly List<IOrder> _orders = new List<IOrder>();
        protected readonly List<ITrade> _trades = new List<ITrade>();
        protected readonly List<ITradingEvent> _journal = new List<ITradingEvent>();

        #endregion Fields

        public TradeEngine(IServer server, IAccount account)
        {
            Server = server;

            Account = account;
        }

        public IReadOnlyList<IOrder> Orders => _orders.ToList();

        public IReadOnlyList<ITrade> Trades => _trades.ToList();

        public IReadOnlyList<ITradingEvent> Journal => _journal.ToList();

        public IServer Server { get; }

        public IAccount Account { get; }

        public abstract TradeResult Execute(IOrderParameters parameters);

        public abstract void CloseMarketOrder(MarketOrder order);

        public abstract void CancelPendingOrder(PendingOrder order);

        public virtual void UpdateSymbolOrders(ISymbol symbol)
        {
            var symbolOrders = _orders.Where(iOrder => iOrder.Symbol == symbol).ToList();

            double totalEquityChange = 0;

            foreach (var order in symbolOrders)
            {
                if (order.OrderType == OrderType.Market)
                {
                    var marketOrder = order as MarketOrder;

                    totalEquityChange += CalculateMarketOrderProfit(marketOrder);

                    bool closeOrder = IsTimeToCloseMarketOrder(marketOrder);

                    if (closeOrder)
                    {
                        CloseMarketOrder(marketOrder);
                    }
                }
                else if (order is PendingOrder)
                {
                    var pendingOrder = order as PendingOrder;

                    bool triggerOrder = IsTimeToTriggerPendingOrder(pendingOrder);

                    if (triggerOrder)
                    {
                        TriggerPendingOrder(pendingOrder);
                    }
                }
            }

            if (totalEquityChange != 0)
            {
                Account.ChangeEquity(totalEquityChange, Server.CurrentTime, string.Empty, AccountChangeType.Trading);
            }
        }

        public virtual void CloseAllMarketOrders()
        {
            foreach (var order in Orders)
            {
                if (order.OrderType != OrderType.Market)
                {
                    continue;
                }

                CloseMarketOrder(order as MarketOrder);
            }
        }

        public virtual void CloseAllMarketOrders(TradeType tradeType)
        {
            foreach (var order in Orders)
            {
                if (order.OrderType != OrderType.Market || order.TradeType != tradeType)
                {
                    continue;
                }

                CloseMarketOrder(order as MarketOrder);
            }
        }

        protected void AddOrder(IOrder order)
        {
            _orders.Add(order);

            switch (order.OrderType)
            {
                case OrderType.Market:
                    var tradingEvent = new TradingEvent(Server.CurrentTime, TradingEventType.MarketOrderExecuted, order, string.Empty);

                    _journal.Add(tradingEvent);

                    break;

                case OrderType.Limit:
                case OrderType.Stop:
                    tradingEvent = new TradingEvent(Server.CurrentTime, TradingEventType.PendingOrderPlaced, order, string.Empty);

                    _journal.Add(tradingEvent);

                    break;
            }
        }

        protected virtual void TriggerPendingOrder(PendingOrder order)
        {
            if (_orders.Contains(order))
            {
                _orders.Remove(order);
            }

            var marketOrderParameters = new MarketOrderParameters(order.Symbol)
            {
                Volume = order.Volume,
                TradeType = order.TradeType,
                StopLossPrice = order.StopLossPrice,
                TakeProfitPrice = order.TakeProfitPrice,
                Comment = order.Comment,
            };

            var tradingEvent = new TradingEvent(Server.CurrentTime, TradingEventType.PendingOrderFilled, order, string.Empty);

            _journal.Add(tradingEvent);

            Execute(marketOrderParameters);
        }

        protected virtual double CalculateMarketOrderProfit(MarketOrder order)
        {
            double price;

            double grossProfitInTicks;

            if (order.TradeType == TradeType.Buy)
            {
                price = order.Symbol.Bid;

                grossProfitInTicks = price - order.EntryPrice;
            }
            else
            {
                price = order.Symbol.Ask;

                grossProfitInTicks = order.EntryPrice - price;
            }

            grossProfitInTicks *= Math.Pow(10, order.Symbol.Digits);

            order.GrossProfit = grossProfitInTicks * order.Symbol.TickValue * order.Volume;

            var netProfit = order.GrossProfit - (order.Commission * order.Volume);

            double result = netProfit - order.NetProfit;

            order.NetProfit = netProfit;

            return result;
        }

        protected virtual bool IsTimeToCloseMarketOrder(MarketOrder order)
        {
            bool result = false;

            if (order.TradeType == TradeType.Buy &&
                (order.Symbol.Bid >= order.TakeProfitPrice || order.Symbol.Bid <= order.StopLossPrice))
            {
                result = true;
            }
            else if (order.TradeType == TradeType.Sell &&
                (order.Symbol.Ask <= order.TakeProfitPrice || order.Symbol.Ask >= order.StopLossPrice))
            {
                result = true;
            }

            return result;
        }

        protected virtual bool IsTimeToTriggerPendingOrder(PendingOrder order)
        {
            bool result = false;

            double price = order.Symbol.GetPrice(order.TradeType);

            if (order.TradeType == TradeType.Buy)
            {
                if (order.OrderType == OrderType.Limit && price <= order.TargetPrice)
                {
                    result = true;
                }
                else if (order.OrderType == OrderType.Stop && price >= order.TargetPrice)
                {
                    result = true;
                }
            }
            else
            {
                if (order.OrderType == OrderType.Limit && price >= order.TargetPrice)
                {
                    result = true;
                }
                else if (order.OrderType == OrderType.Stop && price <= order.TargetPrice)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}