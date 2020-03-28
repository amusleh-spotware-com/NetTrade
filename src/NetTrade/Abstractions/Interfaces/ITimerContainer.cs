using System;
using System.Collections.Generic;

namespace NetTrade.Abstractions.Interfaces
{
    public interface ITimerContainer: IDisposable
    {
        IReadOnlyList<ITimer> Timers { get; }

        void RegisterTimer(ITimer timer);

        bool RemoveTimer(ITimer timer);

        void SetCurrentTime(DateTimeOffset currentTime);

        void PauseSystemTimer();

        void ResumeSystemTimer();
    }
}