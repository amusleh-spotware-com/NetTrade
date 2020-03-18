using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Collections;
using System.Linq;

namespace NetTrade.Indicators
{
    public class IchimokuCloud : Indicator
    {
        private readonly ExpandableSeries<double> _tenkanSen = new ExpandableSeries<double>();
        private readonly ExpandableSeries<double> _kijunSen = new ExpandableSeries<double>();
        private readonly ExpandableSeries<double> _senkouSpanA = new ExpandableSeries<double>();
        private readonly ExpandableSeries<double> _senkouSpanB = new ExpandableSeries<double>();
        private readonly ExpandableSeries<double> _chikouSpan = new ExpandableSeries<double>();

        public IchimokuCloud(ISymbol symbol)
        {
            Symbol = symbol;

            Symbol.IndicatorOnBarEvent += Symbol_OnBarEvent;

            for (int iIndex = 0; iIndex < SenkouSpanShift; iIndex++)
            {
                _senkouSpanA.Add(double.NaN);
                _senkouSpanB.Add(double.NaN);
            }

            for (int iIndex = 0; iIndex < ChikouShift; iIndex++)
            {
                _chikouSpan.Add(double.NaN);
            }
        }

        public ISymbol Symbol { get; }

        public int TenkanSenPeriods { get; set; } = 9;
        public int KijunSenPeriods { get; set; } = 26;
        public int SenkouSpanBPeriods { get; set; } = 52;
        public int ChikouShift { get; set; } = 26;
        public int SenkouSpanShift { get; set; } = 26;

        public ISeries<double> TenkanSen => _tenkanSen;
        public ISeries<double> KijunSen => _kijunSen;
        public ISeries<double> SenkouSpanA => _senkouSpanA;
        public ISeries<double> SenkouSpanB => _senkouSpanB;
        public ISeries<double> ChikouSpan => _chikouSpan;

        private void Symbol_OnBarEvent(object sender, int index)
        {
            double tenkanSen = double.NaN;

            if (index >= TenkanSenPeriods)
            {
                tenkanSen = (Symbol.Bars.High.Skip(index - TenkanSenPeriods).Max() + Symbol.Bars.Low.Skip(index - TenkanSenPeriods).Min()) / 2.0;
            }

            double kijunSen = double.NaN;

            if (index >= KijunSenPeriods)
            {
                kijunSen = (Symbol.Bars.High.Skip(index - KijunSenPeriods).Max() + Symbol.Bars.Low.Skip(index - KijunSenPeriods).Min()) / 2.0;
            }

            var senkouSpanA = double.NaN;

            if (!double.IsNaN(tenkanSen) && !double.IsNaN(kijunSen))
            {
                senkouSpanA = (tenkanSen + kijunSen) / 2.0;
            }

            var senkouSpanB = double.NaN;

            if (index >= SenkouSpanBPeriods)
            {
                senkouSpanB = (Symbol.Bars.High.Skip(index - SenkouSpanBPeriods).Max() + Symbol.Bars.Low.Skip(index - SenkouSpanBPeriods).Min()) / 2.0;
            }

            var chikouSpan = Symbol.Bars.Close[index];

            _tenkanSen.Add(tenkanSen);
            _kijunSen.Add(kijunSen);
            _senkouSpanA.Add(senkouSpanA);
            _senkouSpanB.Add(senkouSpanB);

            if (index >= ChikouShift)
            {
                _chikouSpan.Add(index - ChikouShift, chikouSpan);
            }

            OnNewValue(index);
        }
    }
}