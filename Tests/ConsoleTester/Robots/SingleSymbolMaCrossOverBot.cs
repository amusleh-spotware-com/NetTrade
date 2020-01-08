using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Attributes;
using NetTrade.Collections;
using NetTrade.Enums;
using NetTrade.Models;
using System.Linq;
using System;

namespace ConsoleTester.Robots
{
    public class SingleSymbolMaCrossOverBot : Robot
    {
        private ExpandableSeries<double> _fastMa, _slowMa;

        [Parameter("Fast MA Period", DefaultValue = 10)]
        public int FastMaPeriod { get; set; }

        [Parameter("Slow MA Period", DefaultValue = 20)]
        public int SlowMaPeriod { get; set; }

        [Parameter("Volume", DefaultValue = 1000)]
        public double Volume { get; set; }

        public override void OnStart()
        {
            if (Symbols.Count() > 1)
            {
                throw new InvalidOperationException("This robot is only for single symbol use, not multi symbol");
            }

            _fastMa = new ExpandableSeries<double>();
            _slowMa = new ExpandableSeries<double>();
        }

        public override void OnBar(ISymbol symbol, int index)
        {
            _fastMa.Add(index, GetAverage(symbol.Bars.Close, FastMaPeriod));
            _slowMa.Add(index, GetAverage(symbol.Bars.Close, SlowMaPeriod));

            if (_fastMa[index] > _slowMa[index])
            {
                ClosePositions(TradeType.Sell);

                if (_fastMa[index - 1] <= _slowMa[index - 1] && !Trade.Orders.Any(iOrder => iOrder.OrderType == OrderType.Market && iOrder.TradeType == TradeType.Buy))
                {
                    var marketOrderParameters = new MarketOrderParameters(symbol)
                    {
                        Volume = Volume,
                        TradeType = TradeType.Buy,
                    };

                    Trade.Execute(marketOrderParameters);
                }
            }
            else if (_fastMa[index] < _slowMa[index])
            {
                ClosePositions(TradeType.Buy);

                if (_fastMa[index - 1] >= _slowMa[index - 1] && !Trade.Orders.Any(iOrder => iOrder.OrderType == OrderType.Market && iOrder.TradeType == TradeType.Sell))
                {
                    var marketOrderParameters = new MarketOrderParameters(symbol)
                    {
                        Volume = Volume,
                        TradeType = TradeType.Sell,
                    };

                    Trade.Execute(marketOrderParameters);
                }
            }
        }

        private double GetAverage(ISeries<double> price, int period)
        {
            if (price.Count < period)
            {
                return double.NaN;
            }

            var data = price.TakeLast(period);

            return data.Sum() / data.Count();
        }

        private void ClosePositions(TradeType tradeType)
        {
            foreach (var order in Trade.Orders)
            {
                if (order.OrderType != OrderType.Market || order.TradeType != tradeType)
                {
                    continue;
                }

                Trade.CloseMarketOrder(order as MarketOrder);
            }
        }
    }
}