using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;
using NetTrade.Models;
using System.Linq;
using NetTrade.Enums;

namespace NetTrade.Implementations
{
    public class TradeEngine : ITradeEngine
    {
        #region Fields

        private readonly List<IOrder> _orders = new List<IOrder>();
        private readonly List<ITrade> _trades = new List<ITrade>();
        private readonly List<ITradingEvent> _journal = new List<ITradingEvent>();

        #endregion

        public IReadOnlyList<IOrder> Orders => _orders;

        public IReadOnlyList<ITrade> Trades => _trades;

        public IReadOnlyList<ITradingEvent> Journal => _journal;

        public TradeResult PlaceOrder(IOrderParameters parameters) => parameters.Execute(this);

        public void UpdateSymbolOrders(ISymbol symbol)
        {
            var symbolOrders = _orders.Where(iOrder => iOrder.Symbol == symbol);

            foreach (var order in symbolOrders)
            {
                if (order.OrderType == OrderType.Market)
                {
                    bool closeOrder = false;

                    if (order.TradeType == TradeType.Buy && (symbol.Bid >= order.TakeProfitPrice || symbol.Bid <= order.StopLossPrice ))
                    {
                        closeOrder = true;
                    }
                    else if (order.TradeType == TradeType.Sell && (symbol.Ask <= order.TakeProfitPrice || symbol.Ask >= order.StopLossPrice))
                    {
                        closeOrder = true;
                    }

                    if (closeOrder)
                    {
                        CloseMarketOrder(order as MarketOrder);
                    }
                }
                else if (order is PendingOrder)
                {
                    double price = symbol.GetPrice(order.TradeType);

                    var pendingOrder = order as PendingOrder;

                    bool triggerOrder = false;

                    if (order.TradeType == TradeType.Buy)
                    {
                        if (order.OrderType == OrderType.Limit && price <= pendingOrder.TargetPrice)
                        {
                            triggerOrder = true;
                        }
                        else if (order.OrderType == OrderType.Stop && price >= pendingOrder.TargetPrice)
                        {
                            triggerOrder = true;
                        }
                    }
                    else
                    {
                        if (order.OrderType == OrderType.Limit && price >= pendingOrder.TargetPrice)
                        {
                            triggerOrder = true;
                        }
                        else if (order.OrderType == OrderType.Stop && price <= pendingOrder.TargetPrice)
                        {
                            triggerOrder = true;
                        }
                    }

                    if (triggerOrder)
                    {
                        TriggerPendingOrder(pendingOrder);
                    }
                }
            }
        }

        public void AddOrder(IOrder order)
        {
            _orders.Add(order);

            switch (order.OrderType)
            {
                case OrderType.Market:
                    var tradingEvent = new TradingEvent(TradingEventType.MarketOrderExecuted, order, string.Empty);

                    _journal.Add(tradingEvent);

                    break;
                case OrderType.Limit:
                case OrderType.Stop:
                    tradingEvent = new TradingEvent(TradingEventType.PendingOrderPlaced, order, string.Empty);

                    _journal.Add(tradingEvent);

                    break;
            }
        }

        public void CloseMarketOrder(MarketOrder order)
        {
            if (_orders.Contains(order))
            {
                _orders.Remove(order);
            }

            double price;

            double grossProfit;

            if (order.TradeType == TradeType.Buy)
            {
                price = order.Symbol.Bid;

                grossProfit = price - order.EntryPrice;
            }
            else
            {
                price = order.Symbol.Ask;

                grossProfit = order.EntryPrice - price;
            }

            grossProfit *= Math.Pow(10, order.Symbol.Digits);

            double netProfit = grossProfit - (order.Symbol.Commission * 2);

            var trade = new Trade(order, grossProfit, netProfit, DateTimeOffset.Now);

            _trades.Add(trade);

            var tradingEvent = new TradingEvent(TradingEventType.MarketOrderClosed, order, string.Empty);

            _journal.Add(tradingEvent);
        }

        public void CancelPendingOrder(PendingOrder order)
        {
            if (_orders.Contains(order))
            {
                _orders.Remove(order);
            }

            var tradingEvent = new TradingEvent(TradingEventType.PendingOrderCanceled, order, string.Empty);

            _journal.Add(tradingEvent);
        }

        private void TriggerPendingOrder(PendingOrder order)
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

            var tradingEvent = new TradingEvent(TradingEventType.PendingOrderFilled, order, string.Empty);

            _journal.Add(tradingEvent);

            PlaceOrder(marketOrderParameters);
        }
    }
}
