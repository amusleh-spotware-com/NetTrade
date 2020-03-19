using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Enums
{
    public enum CloseReason
    {
        None,
        StopLossTriggered,
        TakeProfitTriggered,
        Manual,
        Forced
    }
}
