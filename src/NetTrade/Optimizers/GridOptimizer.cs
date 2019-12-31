using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Helpers;

namespace NetTrade.Optimizers
{
    public class GridOptimizer : Optimizer
    {
        public GridOptimizer(IOptimizerSettings settings) : base(settings)
        {
        }

        protected override void OnStart<TRobot>()
        {
            var parametersGrid = OptimizerParameterGridCreator.GetParameterGrid(Settings.Parameters);

            foreach (var parameterGrid in parametersGrid)
            {
                var robot = OptimizerRobotCreator.GetRobot<TRobot>(parameterGrid, this);

                AddRobot(robot);
            }
        }
    }
}