using NetTrade.Helpers;
using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IBacktester
    {
        IRobot Robot { get; }

        TimeSpan Interval { get; set; }

        event OnBacktestStartHandler OnBacktestStartEvent;

        event OnBacktestPauseHandler OnBacktestPauseEvent;

        event OnBacktestStopHandler OnBacktestStopEvent;

        void Start(IRobot robot, IBacktestSettings settings);

        IBacktestResult GetResult();
    }
}