using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Collections;
using System.Linq;

namespace NetTrade.Indicators
{
    public class AverageTrueRange : Indicator
    {
        private readonly TrueRange _trueRange;

        private readonly ExpandableSeries<double> _data = new ExpandableSeries<double>();

        public AverageTrueRange(ISymbol symbol)
        {
            Symbol = symbol;

            _trueRange = new TrueRange(Symbol);

            _trueRange.OnNewValueEvent += _trueRange_OnNewValueEvent;
        }

        public int Periods { get; set; } = 14;

        public ISymbol Symbol { get; }

        public ISeries<double> Data => _data;

        private void _trueRange_OnNewValueEvent(object sender, int index)
        {
            var dataPoint = double.NaN;

            if (index + 1 >= Periods)
            {
                var dataWindow = _trueRange.Data.Skip(_trueRange.Data.Count - Periods);

                dataPoint = dataWindow.Sum() / dataWindow.Count();
            }

            _data.Add(dataPoint);

            OnNewValue(index);
        }
    }
}