using System;

namespace NetTrade.Interfaces
{
    public interface ITrade
    {
        IOrder Order { get; }

        double GrossProfit { get; }

        double NetProfit { get; }

        DateTimeOffset ExitTime { get; }
    }
}