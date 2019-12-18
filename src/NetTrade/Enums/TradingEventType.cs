using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Enums
{
    public enum TradingEventType
    {
        MarketOrderExecuted,
        MarketOrderClosed,
        PendingOrderPlaced,
        PendingOrderCanceled,
        PendingOrderFilled
    }
}
