using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Helpers;
using System.Threading.Tasks;

namespace NetTrade.Optimizers
{
    public class GridOptimizer : Optimizer
    {
        public GridOptimizer(IOptimizerSettings settings) : base(settings)
        {
        }

        protected override void OnStart()
        {
            var parametersGrid = OptimizerParameterGridCreator.GetParameterGrid(Settings.Parameters).ToArray();

            Parallel.For(0, parametersGrid.Length, iRobotIndex =>
            {
                var robot = OptimizerRobotCreator.GetRobot(Settings.RobotType, parametersGrid[iRobotIndex]);

                AddRobot(robot);
            });
        }
    }
}