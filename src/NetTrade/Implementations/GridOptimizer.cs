using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Interfaces;
using System.Linq;

namespace NetTrade.Implementations
{
    public class GridOptimizer : IOptimizer
    {
        public GridOptimizer(IOptimizerSettings settings)
        {
            Settings = settings;
        }

        public IOptimizerSettings Settings { get; }

        public RunningMode RunningMode { get; private set; }

        public event OnOptimizationPassCompletionHandler OnOptimizationPassCompletionEvent;
        public event OnOptimizationFinishedHandler OnOptimizationFinishedEvent;
        public event OnOptimizationProgressChangedHandler OnOptimizationProgressChangedEvent;

        public void Pause()
        {
            RunningMode = RunningMode.Paused;
        }

        public void Start<TRobot, TBacktester>()
        {
            RunningMode = RunningMode.Running;

            var parameterSets = OptimizerParameterSetsCalculator.GetAllParameterSets(Settings.Parameters);
        }

        public void Stop()
        {
            RunningMode = RunningMode.Stopped;
        }
    }
}
