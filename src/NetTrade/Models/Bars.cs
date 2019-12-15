using NetTrade.Helpers;
using NetTrade.Implementations;
using NetTrade.Interfaces;
using System;

namespace NetTrade.Models
{
    public class Bars
    {
        private CustomSeries<DateTime> _time = new CustomSeries<DateTime>();

        private CustomSeries<double> _open = new CustomSeries<double>();

        private CustomSeries<double> _high = new CustomSeries<double>();

        private CustomSeries<double> _low = new CustomSeries<double>();

        private CustomSeries<double> _close = new CustomSeries<double>();

        private CustomSeries<long> _volume = new CustomSeries<long>();

        public ISeries<DateTime> Time => _time;

        public ISeries<double> Open => _open;

        public ISeries<double> High => _high;

        public ISeries<double> Low => _low;

        public ISeries<double> Close => _close;

        public ISeries<long> Volume => _volume;

        public event OnBarHandler OnBar;

        internal int AddValue(Bar bar)
        {
            int index = _time.Count;

            _time.Add(index, bar.Time);
            _open.Add(index, bar.Open);
            _high.Add(index, bar.High);
            _low.Add(index, bar.Low);
            _volume.Add(index, bar.Volume);

            OnBar?.Invoke(this, index);

            return index;
        }
    }
}