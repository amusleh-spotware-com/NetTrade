using NetTrade.Abstractions;
using NetTrade.Helpers;
using NetTrade.Interfaces;

namespace NetTrade.Implementations
{
    public class GridOptimizer : Optimizer
    {
        public GridOptimizer(IOptimizerSettings settings) : base(settings)
        {
        }

        protected override void OnStart<TRobot>()
        {
            var parameterSets = GridOptimizerParametersCalculator.GetParametersGrid(Settings.Parameters);

            foreach (var parameterSet in parameterSets)
            {
                var robot = OptimizerRobotCreator.GetRobot<TRobot>(parameterSet, this);

                AddRobot(robot);
            }
        }
    }
}