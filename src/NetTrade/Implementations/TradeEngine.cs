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

        public TradeEngine(IAccount account, IRobot robot)
        {
            Account = account;

            Robot = robot;
        }

        public IReadOnlyList<IOrder> Orders => _orders;

        public IReadOnlyList<ITrade> Trades => _trades;

        public IReadOnlyList<ITradingEvent> Journal => _journal;

        public IAccount Account { get; }

        public IRobot Robot { get; }

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

            double totalEquityChange = 0, totalBalanceChange = 0;

            foreach (var order in symbolOrders)
            {
                if (order.OrderType == OrderType.Market)
                {
                    var marketOrder = order as MarketOrder;

                    totalEquityChange += CalculateMarketOrderProfit(marketOrder);

                    bool closeOrder = IsTimeToCloseMarketOrder(marketOrder);

                    if (closeOrder)
                    {
                        totalBalanceChange += marketOrder.NetProfit;

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
                var note = $"{symbol.Name} Open Market Orders Total Net Profit Change";

                var equityChange = new AccountEquityChange(totalEquityChange, Robot.Time, note);

                Account.ChangeEquity(equityChange, this);
            }

            if (totalBalanceChange != 0)
            {
                var note = $"{symbol.Name} Closed Market Orders Total Net Profit Change";

                var balanceChange = new AccountBalanceChange(totalBalanceChange, Robot.Time, note);

                Account.ChangeBalance(balanceChange, this);
            }
        }

        public void CloseMarketOrder(MarketOrder order)
        {
            if (_orders.Contains(order))
            {
                _orders.Remove(order);
            }

            var trade = new Trade(order, Robot.Time);

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

            var order = new MarketOrder(parameters, Robot.Time)
            {
                Commission = parameters.Symbol.Commission * 2
            };

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
                var order = new PendingOrder(parameters, Robot.Time);

                AddOrder(order);

                return new TradeResult(order);
            }

            return new TradeResult(OrderErrorCode.InvalidTargetPrice);
        }

        private double CalculateMarketOrderProfit(MarketOrder order)
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

            order.GrossProfit = grossProfitInTicks * order.Symbol.TickValue;

            var netProfit = order.GrossProfit - (order.Commission * order.Volume);

            double result = netProfit - order.NetProfit;

            order.NetProfit = netProfit;

            return result;
        }

        private bool IsTimeToCloseMarketOrder(MarketOrder order)
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

        private bool IsTimeToTriggerPendingOrder(PendingOrder order)
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