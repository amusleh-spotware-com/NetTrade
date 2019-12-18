using NetTrade.Helpers;
using NetTrade.Implementations;
using NetTrade.Interfaces;
using System;

namespace NetTrade.Models
{
    public class Bars
    {
        private ExpandableSeries<DateTimeOffset> _time = new ExpandableSeries<DateTimeOffset>();

        private ExpandableSeries<double> _open = new ExpandableSeries<double>();

        private ExpandableSeries<double> _high = new ExpandableSeries<double>();

        private ExpandableSeries<double> _low = new ExpandableSeries<double>();

        private ExpandableSeries<double> _close = new ExpandableSeries<double>();

        private ExpandableSeries<long> _volume = new ExpandableSeries<long>();

        public Bars(TimeSpan timeFrame)
        {
            TimeFrame = timeFrame;
        }

        public ISeries<DateTimeOffset> Time => _time;

        public ISeries<double> Open => _open;

        public ISeries<double> High => _high;

        public ISeries<double> Low => _low;

        public ISeries<double> Close => _close;

        public ISeries<long> Volume => _volume;

        public TimeSpan TimeFrame { get; }

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