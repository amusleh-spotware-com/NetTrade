using System;

namespace NetTrade.Interfaces
{
    public interface ITrade
    {
        IOrder Order { get; }

        DateTimeOffset ExitTime { get; }
    }
}