using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;

namespace NetTrade.Implementations
{
    public class Bars : IBars
    {
        public ISeries<DateTime> Time { get; } = new CustomSeries<DateTime>();

        public ISeries<double> Open { get; } = new CustomSeries<double>();

        public ISeries<double> High { get; } = new CustomSeries<double>();

        public ISeries<double> Low { get; } = new CustomSeries<double>();

        public ISeries<double> Close { get; } = new CustomSeries<double>();
    }
}
