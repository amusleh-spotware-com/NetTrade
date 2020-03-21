using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Attributes;
using NetTrade.Enums;
using NetTrade.Indicators;
using NetTrade.Models;
using System;
using System.Linq;

namespace ConsoleTester.Robots
{
    [Robot(Name = "Single Symbol Ma Cross Over Bot", Group = "Sample")]
    public class SingleSymbolMaCrossOverBot : Robot
    {
        private SimpleMovingAverage _fastMa, _slowMa;

        [Parameter("Fast MA Period", DefaultValue = 5)]
        public int FastMaPeriod { get; set; }

        [Parameter("Slow MA Period", DefaultValue = 10)]
        public int SlowMaPeriod { get; set; }

        [Parameter("Volume", DefaultValue = 1)]
        public double Volume { get; set; }

        public override void OnStart()
        {
            if (Symbols.Count() > 1)
            {
                throw new InvalidOperationException("This robot is only for single symbol use, not multi symbol");
            }

            _fastMa = new SimpleMovingAverage(Symbols.First()) { DataSourceType = DataSourceType.Close, Periods = FastMaPeriod };

            _slowMa = new SimpleMovingAverage(Symbols.First()) { DataSourceType = DataSourceType.Close, Periods = SlowMaPeriod };
        }

        public override void OnBar(ISymbol symbol, int index)
        {
            if (_fastMa.Data[index] > _slowMa.Data[index])
            {
                Trade.CloseAllMarketOrders(TradeType.Sell);

                if (_fastMa.Data[index - 1] <= _slowMa.Data[index - 1] && !Trade.Orders.Any(iOrder => iOrder.OrderType == OrderType.Market && iOrder.TradeType == TradeType.Buy))
                {
                    var marketOrderParameters = new MarketOrderParameters(symbol)
                    {
                        Volume = Volume,
                        TradeType = TradeType.Buy,
                    };

                    Trade.Execute(marketOrderParameters);
                }
            }
            else if (_fastMa.Data[index] < _slowMa.Data[index])
            {
                Trade.CloseAllMarketOrders(TradeType.Buy);

                if (_fastMa.Data[index - 1] >= _slowMa.Data[index - 1] && !Trade.Orders.Any(iOrder => iOrder.OrderType == OrderType.Market && iOrder.TradeType == TradeType.Sell))
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
    }
}