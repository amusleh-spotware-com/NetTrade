using NetTrade.Helpers;
using System.Collections.Generic;
using NetTrade.Enums;

namespace NetTrade.Interfaces
{
    public interface IOptimizer
    {
        IOptimizerSettings Settings { get; }

        RunningMode RunningMode { get; }

        event OnOptimizationPassCompletionHandler OnOptimizationPassCompletionEvent;

        event OnOptimizationFinishedHandler OnOptimizationFinishedEvent;

        event OnOptimizationProgressChangedHandler OnOptimizationProgressChangedEvent;

        void Start<TRobot, TBacktester>();

        void Pause();

        void Stop();
    }
}