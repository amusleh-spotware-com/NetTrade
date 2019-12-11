using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Abstractions
{
    public interface IBars
    {
        ITimeSeries Time { get; }

        IDataSeries Open { get; }

        IDataSeries High { get; }

        IDataSeries Low { get; }

        IDataSeries Close { get; }
    }
}
