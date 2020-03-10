using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Collections;
using System;

namespace NetTrade.Indicators
{
    public class TrueRange : Indicator
    {
        private readonly ExpandableSeries<double> _data = new ExpandableSeries<double>();

        public TrueRange(ISymbol symbol)
        {
            Symbol = symbol;

            Symbol.IndicatorOnBarEvent += Symbol_OnBarEvent;
        }

        public ISymbol Symbol { get; }

        public ISeries<double> Data => _data;

        private void Symbol_OnBarEvent(object sender, int index)
        {
            var highMinusLow = Symbol.Bars.High[index] - Symbol.Bars.Low[index];

            if (index == 0)
            {
                _data.Add(highMinusLow);
            }
            else
            {
                var highMinusPreviousClose = Math.Abs(Symbol.Bars.High[index] - Symbol.Bars.Close[index - 1]);
                var lowMinusPreviousClose = Math.Abs(Symbol.Bars.Low[index] - Symbol.Bars.Close[index - 1]);

                var trueRange = Math.Max(highMinusLow, Math.Max(highMinusPreviousClose, lowMinusPreviousClose));

                _data.Add(trueRange);
            }

            OnNewValue(index);
        }
    }
}