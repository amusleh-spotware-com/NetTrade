using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Enums
{
    public enum RobotExceptionSource
    {
        OnTick,
        OnBar,
        OnStop,
        OnResume,
        OnPause,
        OnTimer,
        OnStart
    }
}
