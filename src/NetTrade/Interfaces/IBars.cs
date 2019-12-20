using NetTrade.Helpers;
using System;

namespace NetTrade.Interfaces
{
    public interface IBars
    {
        ISeries<DateTimeOffset> Time { get; }

        ISeries<double> Open { get; }

        ISeries<double> High { get; }

        ISeries<double> Low { get; }

        ISeries<double> Close { get; }

        ISeries<long> Volume { get; }

        event OnBarHandler OnBarEvent;

        int AddBar(IBar bar);
    }
}