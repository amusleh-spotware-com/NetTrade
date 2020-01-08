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

        protected override void OnStart()
        {
            var parametersGrid = OptimizerParameterGridCreator.GetParameterGrid(Settings.Parameters);

            foreach (var parameterGrid in parametersGrid)
            {
                var robot = OptimizerRobotCreator.GetRobot(Settings.RobotType, parameterGrid);

                AddRobot(robot);
            }
        }
    }
}