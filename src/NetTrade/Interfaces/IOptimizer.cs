using NetTrade.Helpers;
using System.Collections.Generic;

namespace NetTrade.Interfaces
{
    public interface IOptimizer
    {
        IBacktestSettings BacktestSettings { get; }

        IAccount SampleAccount { get; }

        ISymbol MainSymbol { get; }

        List<ISymbol> OtherSymbols { get; }

        IReadOnlyList<IOptimizeParameter> Parameters { get; }

        event OnOptimizationPassCompletionHandler OnOptimizationPassCompletionEvent;

        event OnOptimizationFinishedHandler OnOptimizationFinishedEvent;

        event OnOptimizationProgressChangedHandler OnOptimizationProgressChangedEvent;

        void Start<TRobot, TBacktester>();

        void Pause();

        void Stop();
    }
}