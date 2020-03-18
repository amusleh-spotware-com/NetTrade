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

        private int _periods;

        public ExponentialMovingAverage(ISymbol symbol)
        {
            Symbol = symbol;

            Symbol.IndicatorOnBarEvent += Symbol_OnBarEvent;

            Periods = 14;
        }

        public ISymbol Symbol { get; }

        public double WeightedMultiplier { get; private set; }

        public int Periods 
        {
            get => _periods;
            set
            {
                _periods = value;

                WeightedMultiplier = 2 / (_periods + 1.0);
            }
        }

        public DataSourceType DataSourceType { get; set; } = DataSourceType.Close;

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