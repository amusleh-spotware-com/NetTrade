using NetTrade.Enums;
using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IRobot
    {
        IRobotSettings Settings { get; }

        RunningMode RunningMode { get; }

        void Start(IRobotSettings settings);

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