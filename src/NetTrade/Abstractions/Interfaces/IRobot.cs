using NetTrade.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IRobot
    {
        IAccount Account { get; }

        IEnumerable<ISymbol> Symbols { get; }

        ITradeEngine Trade { get; }

        RunningMode RunningMode { get; }

        Mode Mode { get; }

        IBacktester Backtester { get; }

        IBacktestSettings BacktestSettings { get; }

        IServer Server { get; }

        ITimer Timer { get; }

        Task StartAsync(IRobotParameters settings);

        void Stop();

        void Pause();

        void Resume();

        void OnStart();

        void OnPause();

        void OnResume();

        void OnStop();

        void OnBar(ISymbol symbol, int index);

        void OnTick(ISymbol symbol);

        void OnTimer();

        void SetTimeByBacktester(IBacktester backtester, DateTimeOffset time);
    }
}