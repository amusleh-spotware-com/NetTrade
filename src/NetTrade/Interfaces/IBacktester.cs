using NetTrade.Helpers;
using System;

namespace NetTrade.Interfaces
{
    public interface IBacktester
    {
        IRobot Robot { get; }

        DateTimeOffset StartTime { get; }

        DateTimeOffset EndTime { get; }

        event OnBacktestStart OnBacktestStartEvent;

        event OnBacktestPause OnBacktestPauseEvent;

        event OnBacktestStop OnBacktestStopEvent;

        void SetTime(DateTimeOffset startTime, DateTimeOffset endTime);

        void Start(IRobot robot);

        void Pause();

        void Stop();

        IBacktestResult GetResult();
    }
}