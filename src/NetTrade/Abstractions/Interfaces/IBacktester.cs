using NetTrade.Helpers;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IBacktester
    {
        IRobot Robot { get; }

        TimeSpan Interval { get; set; }

        IBacktestSettings Settings { get; }

        IEnumerable<ISymbolBacktestData> SymbolsData { get; }

        event OnBacktestStartHandler OnBacktestStartEvent;

        event OnBacktestPauseHandler OnBacktestPauseEvent;

        event OnBacktestStopHandler OnBacktestStopEvent;

        event OnBacktestProgressChangedHandler OnBacktestProgressChangedEvent;

        Task StartAsync(IRobot robot, IBacktestSettings settings, IEnumerable<ISymbolBacktestData> symbolsBacktestData);

        IBacktestResult GetResult(double riskFreeReturnPercentage);
    }
}