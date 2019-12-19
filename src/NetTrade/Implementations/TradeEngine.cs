using NetTrade.Enums;
using NetTrade.Interfaces;
using NetTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Implementations
{
    public class TradeEngine : ITradeEngine
    {
        #region Fields

        private readonly List<IOrder> _orders = new List<IOrder>();
        private readonly List<ITrade> _trades = new List<ITrade>();
        private readonly List<ITradingEvent> _journal = new List<ITradingEvent>();

        #endregion Fields

        public IReadOnlyList<IOrder> Orders => _orders;

        public IReadOnlyList<ITrade> Trades => _trades;

        public IReadOnlyList<ITradingEvent> Journal => _journal;

        public TradeResult Execute(IOrderParameters parameters)
        {
            switch (parameters.OrderType)
            {
                case OrderType.Market:
                    return ExecuteMarketOrder(parameters as MarketOrderParameters);

                case OrderType.Limit:
                case OrderType.Stop:
                    return PlacePendingOrder(parameters as PendingOrderParameters);

                default:
                    throw new ArgumentException("Unknown order type");
            }
        }

        public void UpdateSymbolOrders(ISymbol symbol)
        {
            var symbolOrders = _orders.Where(iOrder => iOrder.Symbol == symbol);

            foreach (var order in symbolOrders)
            {
                if (order.OrderType == OrderType.Market)
                {
                    bool closeOrder = false;

                    if (order.TradeType == TradeType.Buy && (symbol.Bid >= order.TakeProfitPrice || symbol.Bid <= order.StopLossPrice))
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

        private void AddOrder(IOrder order)
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

            Execute(marketOrderParameters);
        }

        private TradeResult ExecuteMarketOrder(MarketOrderParameters parameters)
        {
            var symbolPrice = parameters.Symbol.GetPrice(parameters.TradeType);
            var symbolSlippageInPrice = parameters.Symbol.Slippage * parameters.Symbol.TickSize;

            if (parameters.TradeType == TradeType.Buy)
            {
                parameters.EntryPrice = symbolPrice + symbolSlippageInPrice;
            }
            else
            {
                parameters.EntryPrice = symbolPrice - symbolSlippageInPrice;
            }

            var order = new MarketOrder(parameters);

            AddOrder(order);

            var result = new TradeResult(order);

            return result;
        }

        private TradeResult PlacePendingOrder(PendingOrderParameters parameters)
        {
            double price = parameters.Symbol.GetPrice(parameters.TradeType);

            bool isPriceValid = true;

            switch (parameters.OrderType)
            {
                case OrderType.Limit:
                    isPriceValid = parameters.TradeType == TradeType.Buy ? parameters.TargetPrice < price : parameters.TargetPrice > price;
                    break;

                case OrderType.Stop:
                    isPriceValid = parameters.TradeType == TradeType.Buy ? parameters.TargetPrice > price : parameters.TargetPrice < price;
                    break;
            }

            if (isPriceValid)
            {
                var order = new PendingOrder(parameters);

                AddOrder(order);

                return new TradeResult(order);
            }

            return new TradeResult(OrderErrorCode.InvalidTargetPrice);
        }
    }
}