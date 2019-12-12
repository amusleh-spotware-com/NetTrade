using System;
using NetTrade.Helpers;

namespace NetTrade.Interfaces
{
    public interface IBars
    {
        ISeries<DateTime> Time { get; }

        ISeries<double> Open { get; }

        ISeries<double> High { get; }

        ISeries<double> Low { get; }

        ISeries<double> Close { get; }

        ISeries<long> Volume { get; }

        event OnBarHandler OnBar;
    }
}