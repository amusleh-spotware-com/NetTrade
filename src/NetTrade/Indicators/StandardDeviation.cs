using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Collections;
using NetTrade.Enums;
using NetTrade.Helpers;
using System.Linq;

namespace NetTrade.Indicators
{
    public class StandardDeviation : Indicator
    {
        private readonly ExpandableSeries<double> _data = new ExpandableSeries<double>();

        public StandardDeviation(ISymbol symbol, int periods, DataSourceType dataSourceType)
        {
            Symbol = symbol;

            Symbol.IndicatorOnBarEvent += Symbol_OnBarEvent;

            Periods = periods;

            DataSourceType = dataSourceType;
        }

        public ISymbol Symbol { get; }

        public int Periods { get; }

        public DataSourceType DataSourceType { get; }

        public ISeries<double> Data => _data;

        private void Symbol_OnBarEvent(object sender, int index)
        {
            var symbolData = Symbol.Bars.GetData(DataSourceType);

            var dataPoint = double.NaN;

            if (symbolData.Count >= Periods)
            {
                var dataWindow = symbolData.Skip(symbolData.Count - Periods);

                dataPoint = StdCalculator.GetStd(dataWindow, true);
            }

            _data.Add(dataPoint);

            OnNewValue(index);
        }
    }
}