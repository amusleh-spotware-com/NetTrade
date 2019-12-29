using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Interfaces;
using System.Linq;
using NetTrade.Abstractions;

namespace NetTrade.Implementations
{
    public class GridOptimizer : Optimizer
    {
        public GridOptimizer(IOptimizerSettings settings): base(settings)
        {
        }

        public override void Pause()
        {
            RunningMode = RunningMode.Paused;
        }

        public override void Start<TRobot>()
        {
            RunningMode = RunningMode.Running;

            var parameterSets = OptimizerParameterSetsCalculator.GetAllParameterSets(Settings.Parameters);

            foreach (var parameterSet in parameterSets)
            {
                var robot = OptimizerRobotCreator.GetRobot<TRobot>(parameterSet, this);

                _robots.Add(robot);
            }
        }

        public override void Stop()
        {
            RunningMode = RunningMode.Stopped;
        }
    }
}
