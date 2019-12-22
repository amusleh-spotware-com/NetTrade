using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Interfaces
{
    public interface IBacktestSettings
    {
        DateTimeOffset StartTime { get; }

        DateTimeOffset EndTime { get; }
    }
}
