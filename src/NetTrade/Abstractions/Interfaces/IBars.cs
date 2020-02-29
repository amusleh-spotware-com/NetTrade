using System;
using NetTrade.Enums;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IBars : ICloneable
    {
        ISeries<DateTimeOffset> Time { get; }

        ISeries<double> Open { get; }

        ISeries<double> High { get; }

        ISeries<double> Low { get; }

        ISeries<double> Close { get; }

        ISeries<double> Volume { get; }

        int AddBar(IBar bar);

        ISeries<double> GetData(DataSourceType source);
    }
}