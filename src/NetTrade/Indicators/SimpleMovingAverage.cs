using NetTrade.Abstractions.Interfaces;
using NetTrade.Abstractions;
using NetTrade.Collections;
using NetTrade.Enums;
using System.Linq;

namespace NetTrade.Indicators
{
    public class SimpleMovingAverage : Indicator
    {
        private readonly ExpandableSeries<double> _data = new ExpandableSeries<double>();

        public SimpleMovingAverage(ISymbol symbol)
        {
            Symbol = symbol;

            Symbol.IndicatorOnBarEvent += Symbol_OnBarEvent;
        }

        public ISymbol Symbol { get; }

        public int Periods { get; set; } = 14;

        public DataSourceType DataSourceType { get; set; } = DataSourceType.Close;

        public ISeries<double> Data => _data;

        private void Symbol_OnBarEvent(object sender, int index)
        {
            var symbolData = Symbol.Bars.GetData(DataSourceType);

            var dataPoint = double.NaN;

            if (symbolData.Count >= Periods)
            {
                var dataWindow = symbolData.Skip(symbolData.Count - Periods);

                dataPoint = dataWindow.Sum() / dataWindow.Count();
            }

            _data.Add(dataPoint);

            OnNewValue(index);
        }
    }
}