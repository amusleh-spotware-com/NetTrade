using System;
using System.Collections.Generic;

namespace NetTrade.Abstractions.Interfaces
{
    public interface ITimerContainer
    {
        IReadOnlyList<ITimer> Timers { get; }

        void RegisterTimer(ITimer timer);

        bool RemoveTimer(ITimer timer);

        void SetCurrentTime(DateTimeOffset currentTime);
    }
}