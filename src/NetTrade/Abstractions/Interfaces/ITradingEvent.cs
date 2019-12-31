using NetTrade.Enums;

namespace NetTrade.Abstractions.Interfaces
{
    public interface ITradingEvent
    {
        TradingEventType Type { get; }

        IOrder Order { get; }

        string Description { get; }
    }
}