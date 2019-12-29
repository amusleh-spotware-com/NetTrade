using NetTrade.Enums;
using System;

namespace NetTrade.Interfaces
{
    public interface IRobot
    {
        IRobotSettings Settings { get; }

        RunningMode RunningMode { get; }

        void Start();

        void Stop();

        void Pause();

        void Resume();

        void OnStart();

        void OnBar(ISymbol symbol, int index);

        void OnTick(ISymbol symbol);

        void OnStop();

        void SetTimeByBacktester(IBacktester backtester, DateTimeOffset time);
    }
}