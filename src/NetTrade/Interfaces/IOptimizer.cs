using NetTrade.Helpers;
using System.Collections.Generic;
using NetTrade.Enums;
using NetTrade.Abstractions;

namespace NetTrade.Interfaces
{
    public interface IOptimizer
    {
        IOptimizerSettings Settings { get; }

        RunningMode RunningMode { get; }

        IReadOnlyList<Robot> Robots { get; }

        event OnOptimizationPassCompletionHandler OnOptimizationPassCompletionEvent;

        event OnOptimizationStartedHandler OnOptimizationStartedEvent;

        event OnOptimizationStoppedHandler OnOptimizationStoppedEvent;

        void Start<TRobot>() where TRobot: Robot;

        void Stop();

        IRobotSettings GetRobotSettings();
    }
}