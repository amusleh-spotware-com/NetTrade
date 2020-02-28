using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Models;
using System;
using System.Linq;

namespace NetTrade.TradeEngines
{
    public class BacktestTradeEngine : TradeEngine
    {
        public BacktestTradeEngine(IServer server, IAccount account) : base(server, account)
        {
        }

        public override TradeResult Execute(IOrderParameters parameters)
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

        public override void CloseMarketOrder(MarketOrder order)
        {
            if (_orders.Contains(order))
            {
                _orders.Remove(order);
            }

            var exitPrice = order.TradeType == TradeType.Buy ? order.Symbol.GetPrice(TradeType.Sell) :
                order.Symbol.GetPrice(TradeType.Buy);

            var barsPeriod = order.Symbol.Bars.Time.Where(iBarTime => iBarTime >= order.OpenTime).Count();

            var tradingEvent = new TradingEvent(Server.CurrentTime, TradingEventType.MarketOrderClosed, order, string.Empty);

            _journal.Add(tradingEvent);

            Account.ChangeMargin(-order.MarginUsed, Server.CurrentTime, string.Empty, AccountChangeType.Trading);

            Account.ChangeBalance(order.NetProfit, Server.CurrentTime, string.Empty, AccountChangeType.Trading);

            var tradeData = Trades.Select(iTrade => iTrade.Order.NetProfit);

            var sharpeRatio = SharpeRatioCalculator.GetSharpeRatio(tradeData);
            var sortinoRatio = SortinoRatioCalculator.GetSortinoRatio(tradeData);

            var equityMaxDrawDown = MaxDrawdownCalculator.GetMaxDrawdown(Account.EquityChanges);
            var balanceMaxDrawDown = MaxDrawdownCalculator.GetMaxDrawdown(Account.BalanceChanges);

            var id = _trades.Count + 1;

            var trade = new Trade(id, order, Server.CurrentTime, exitPrice, Account.Equity, Account.CurrentBalance,
                sharpeRatio, sortinoRatio, equityMaxDrawDown, balanceMaxDrawDown, barsPeriod);

            _trades.Add(trade);
        }

        public override void CancelPendingOrder(PendingOrder order)
        {
            if (_orders.Contains(order))
            {
                _orders.Remove(order);
            }

            var tradingEvent = new TradingEvent(Server.CurrentTime, TradingEventType.PendingOrderCanceled, order, string.Empty);

            _journal.Add(tradingEvent);
        }

        private TradeResult ExecuteMarketOrder(MarketOrderParameters parameters)
        {
            var marginRequired = (parameters.Volume * parameters.Symbol.VolumeUnitValue) / Account.Leverage;

            if (marginRequired >= Account.FreeMargin)
            {
                return new TradeResult(OrderErrorCode.NotEnoughMargin);
            }
            else
            {
                var symbolPrice = parameters.Symbol.GetPrice(parameters.TradeType);
                var symbolSlippageInPrice = parameters.Symbol.Slippage * parameters.Symbol.TickSize;

                double entryPrice;

                if (parameters.TradeType == TradeType.Buy)
                {
                    entryPrice = symbolPrice + symbolSlippageInPrice;
                }
                else
                {
                    entryPrice = symbolPrice - symbolSlippageInPrice;
                }

                var order = new MarketOrder(entryPrice, parameters, Server.CurrentTime)
                {
                    Commission = parameters.Symbol.Commission * 2,
                    MarginUsed = marginRequired
                };

                AddOrder(order);

                Account.ChangeMargin(marginRequired, Server.CurrentTime, string.Empty, AccountChangeType.Trading);

                return new TradeResult(order);
            }
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
                var order = new PendingOrder(parameters, Server.CurrentTime);

                AddOrder(order);

                return new TradeResult(order);
            }

            return new TradeResult(OrderErrorCode.InvalidTargetPrice);
        }
    }
}