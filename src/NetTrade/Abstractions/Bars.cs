using NetTrade.Abstractions.Interfaces;
using NetTrade.Collections;
using NetTrade.Enums;
using System;

namespace NetTrade.Abstractions
{
    public abstract class Bars : IBars
    {
        private readonly ExpandableSeries<DateTimeOffset> _time = new ExpandableSeries<DateTimeOffset>();

        private readonly ExpandableSeries<double> _open = new ExpandableSeries<double>();

        private readonly ExpandableSeries<double> _high = new ExpandableSeries<double>();

        private readonly ExpandableSeries<double> _low = new ExpandableSeries<double>();

        private readonly ExpandableSeries<double> _close = new ExpandableSeries<double>();

        private readonly ExpandableSeries<double> _volume = new ExpandableSeries<double>();

        public ISeries<DateTimeOffset> Time => _time;

        public ISeries<double> Open => _open;

        public ISeries<double> High => _high;

        public ISeries<double> Low => _low;

        public ISeries<double> Close => _close;

        public ISeries<double> Volume => _volume;

        public virtual int AddBar(IBar bar)
        {
            int index = _time.Count;

            _time.Add(index, bar.Time);
            _open.Add(index, bar.Open);
            _high.Add(index, bar.High);
            _low.Add(index, bar.Low);
            _close.Add(index, bar.Close);
            _volume.Add(index, bar.Volume);

            return index;
        }

        public virtual ISeries<double> GetData(DataSourceType source)
        {
            switch (source)
            {
                case DataSourceType.Open:
                    return Open;

                case DataSourceType.High:
                    return High;

                case DataSourceType.Low:
                    return Low;

                case DataSourceType.Close:
                    return Close;

                case DataSourceType.Volume:
                    return Volume;

                default:
                    throw new ArgumentOutOfRangeException(nameof(source));
            }
        }

        public abstract object Clone();
    }
}