using NetTrade.Enums;
using NetTrade.Helpers;
using System.Collections.Generic;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IOptimizer
    {
        IOptimizerSettings Settings { get; }

        RunningMode RunningMode { get; }

        IReadOnlyList<Robot> Robots { get; }

        event OnOptimizationPassCompletionHandler OnOptimizationPassCompletionEvent;

        event OnOptimizationStartedHandler OnOptimizationStartedEvent;

        event OnOptimizationStoppedHandler OnOptimizationStoppedEvent;

        void Start();

        void Stop();

        IRobotSettings GetRobotSettings();
    }
}