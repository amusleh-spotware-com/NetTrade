using NetTrade.Enums;
using NetTrade.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        Task StartAsync();

        void Stop();

        IRobotParameters GetRobotParameters();
    }
}