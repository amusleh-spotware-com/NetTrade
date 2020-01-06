using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Helpers;

namespace NetTrade.Abstractions.Interfaces
{
    public interface ITimer
    {
        TimeSpan Interval { get; }

        bool Enabled { get; }

        DateTimeOffset LastElapsedTime { get; }

        event OnTimerElapsedHandler OnTimerElapsedEvent;

        event OnTimerStartHandler OnTimerStartEvent;

        event OnTimerStopHandler OnTimerStopEvent;

        event OnTimerIntervalChangedHandler OnTimerIntervalChangedEvent;

        void Start();

        void Stop();

        void SetCurrentTime(DateTimeOffset currentTime);

        void SetInterval(TimeSpan interval);
    }
}
