using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;

namespace NetTrade.Models
{
    public class TradingEvent : ITradingEvent
    {
        public TradingEvent(TradingEventType type, IOrder order, string description)
        {
            Type = type;

            Order = order;

            Description = description;
        }

        public TradingEventType Type { get; }

        public IOrder Order { get; }

        public string Description { get; }
    }
}