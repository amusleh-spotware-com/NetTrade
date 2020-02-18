using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IBars : ICloneable
    {
        ISeries<DateTimeOffset> Time { get; }

        ISeries<double> Open { get; }

        ISeries<double> High { get; }

        ISeries<double> Low { get; }

        ISeries<double> Close { get; }

        ISeries<long> Volume { get; }

        int AddBar(IBar bar);
    }
}