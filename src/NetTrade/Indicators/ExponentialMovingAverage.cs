using NetTrade.Abstractions.Interfaces;
using NetTrade.Abstractions;
using NetTrade.Collections;
using NetTrade.Enums;
using System.Linq;

namespace NetTrade.Indicators
{
    public class ExponentialMovingAverage : Indicator
    {
        private readonly ExpandableSeries<double> _data = new ExpandableSeries<double>();

        public ExponentialMovingAverage(ISymbol symbol, int periods, DataSourceType dataSourceType)
        {
            Symbol = symbol;

            Symbol.IndicatorOnBarEvent += Symbol_OnBarEvent;

            Periods = periods;

            DataSourceType = dataSourceType;

            WeightedMultiplier = 2 / (Periods + 1.0);
        }

        public ISymbol Symbol { get; }

        public double WeightedMultiplier { get; }

        public int Periods { get; }

        public DataSourceType DataSourceType { get; }

        public ISeries<double> Data => _data;

        private void Symbol_OnBarEvent(object sender, int index)
        {
            var symbolData = Symbol.Bars.GetData(DataSourceType);

            var dataPoint = double.NaN;

            if (index == 0)
            {
                dataPoint = symbolData[index];
            }
            else
            {
                var previousEma = _data[index - 1];

                dataPoint = (symbolData[index] - previousEma) * WeightedMultiplier + previousEma;
            }

            _data.Add(dataPoint);

            OnNewValue(index);
        }
    }
}