using NetTrade.Enums;

namespace NetTrade.Interfaces
{
    public interface ITradingEvent
    {
        TradingEventType Type { get; }

        IOrder Order { get; }

        string Description { get; }
    }
}