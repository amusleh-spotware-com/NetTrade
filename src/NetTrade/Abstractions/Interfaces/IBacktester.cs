using NetTrade.Helpers;
using System;
using System.Threading.Tasks;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IBacktester
    {
        IRobot Robot { get; }

        TimeSpan Interval { get; set; }

        event OnBacktestStartHandler OnBacktestStartEvent;

        event OnBacktestPauseHandler OnBacktestPauseEvent;

        event OnBacktestStopHandler OnBacktestStopEvent;

        Task StartAsync(IRobot robot, IBacktestSettings settings);

        IBacktestResult GetResult();
    }
}