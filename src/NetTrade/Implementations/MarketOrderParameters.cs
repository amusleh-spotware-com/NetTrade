using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Abstractions;
using NetTrade.Enums;

namespace NetTrade.Implementations
{
    public class MarketOrderParameters: OrderParameters
    {
        public MarketOrderParameters(): base(OrderType.Market)
        {
        }
    }
}
