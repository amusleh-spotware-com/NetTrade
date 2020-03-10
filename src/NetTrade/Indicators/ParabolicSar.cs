using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Collections;
using System;
using System.Linq;

namespace NetTrade.Indicators
{
    public class ParabolicSar : Indicator
    {
        private readonly ExpandableSeries<double> _data = new ExpandableSeries<double>();

        private double _currentAccelerationFactor, _extremePoint;

        private bool _isUpTrend;

        public ParabolicSar(ISymbol symbol, double minAccelerationFactor, double maxAccelerationFactor,
            double accelerationFactorStep)
        {
            Symbol = symbol;

            Symbol.IndicatorOnBarEvent += Symbol_OnBarEvent;

            MinAccelerationFactor = minAccelerationFactor;

            MaxAccelerationFactor = maxAccelerationFactor;

            AccelerationFactorStep = accelerationFactorStep;

            _currentAccelerationFactor = MinAccelerationFactor;
        }

        public ISymbol Symbol { get; }

        public double MinAccelerationFactor { get; }

        public double MaxAccelerationFactor { get; }

        public double AccelerationFactorStep { get; }

        public ISeries<double> Data => _data;

        private void Symbol_OnBarEvent(object sender, int index)
        {
            double dataPoint;

            if (index < 1)
            {
                dataPoint = ComputeInitialValue(index);
            }
            else
            {
                var isUpTrend = IsUpTrend(index);

                dataPoint = isUpTrend ? ComputeUptrend(index) : ComputeDowntrend(index);
            }

            _data.Add(dataPoint);

            OnNewValue(index);
        }

        private void IncreaseAccelaration()
        {
            double newAccelerationFactor = _currentAccelerationFactor + AccelerationFactorStep;

            _currentAccelerationFactor = newAccelerationFactor <= MaxAccelerationFactor ? newAccelerationFactor : MaxAccelerationFactor;
        }

        private double ComputeInitialValue(int index)
        {
            double result;

            _isUpTrend = IsUpTrend(index);

            if (_isUpTrend)
            {
                result = Symbol.Bars.Low.Min();
                _extremePoint = Symbol.Bars.High.Max();
            }
            else
            {
                result = Symbol.Bars.High.Max();
                _extremePoint = Symbol.Bars.Low.Min();
            }

            _currentAccelerationFactor = MinAccelerationFactor;

            return result;
        }

        private double ComputeUptrend(int index)
        {
            double result;

            var isTrendUnchanged = Symbol.Bars.Low[index - 1] > _data[index - 1];

            if (isTrendUnchanged)
            {
                // Current uptrend
                result = _data[index - 1] + _currentAccelerationFactor * (_extremePoint - _data[index - 1]);

                var isNewExtreme = Symbol.Bars.High[index] > _extremePoint;
                _extremePoint = Math.Max(_extremePoint, Symbol.Bars.High[index]);

                if (Symbol.Bars.Low[index] > result && isNewExtreme)
                {
                    IncreaseAccelaration();
                }
            }
            else
            {
                // Current downtrend
                result = _extremePoint;
                _extremePoint = Math.Min(_extremePoint, Symbol.Bars.Low[index]);
                _currentAccelerationFactor = AccelerationFactorStep;
            }

            _isUpTrend = isTrendUnchanged;

            return result;
        }

        private double ComputeDowntrend(int index)
        {
            double result;

            var isTrendUnchanged = Symbol.Bars.High[index - 1] < _data[index - 1];

            if (isTrendUnchanged)
            {
                // Current downtrend
                result = _data[index - 1] + _currentAccelerationFactor * (_extremePoint - _data[index - 1]);

                var isNewExtreme = Symbol.Bars.Low[index] < _extremePoint;

                _extremePoint = Math.Min(_extremePoint, Symbol.Bars.Low[index]);

                if (Symbol.Bars.High[index] < result && isNewExtreme)
                {
                    IncreaseAccelaration();
                }
            }
            else
            {
                // Current uptrend
                result = _extremePoint;
                _extremePoint = Math.Max(_extremePoint, Symbol.Bars.High[index]);
                _currentAccelerationFactor = AccelerationFactorStep;
            }

            _isUpTrend = !isTrendUnchanged;

            return result;
        }

        private bool IsUpTrend(int index)
        {
            return Symbol.Bars.High[index] > _data[index - 1];
        }
    }
}