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
        public GridOptimizer(IBacktestSettings backtestSettings, IAccount account, ISymbol mainSymbol,
            IEnumerable<ISymbol> otherSymbols, IEnumerable<IOptimizeParameter> parameters)
        {
            BacktestSettings = backtestSettings;
            Account = account;
            MainSymbol = mainSymbol;
            OtherSymbols = otherSymbols.ToList();
            Parameters = parameters.ToList();
        }

        public IBacktestSettings BacktestSettings { get; }

        public IAccount Account { get; }

        public ISymbol MainSymbol { get; }

        public IReadOnlyList<ISymbol> OtherSymbols { get; }

        public IReadOnlyList<IOptimizeParameter> Parameters { get; }

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

            int counter = 0;

            foreach (var parameter in Parameters)
            {
                if (!parameter.Optimize)
                {

                    continue;
                }

                if (parameter.Step is int)
                {

                }
                else if (parameter.Step is double)
                {

                }
                else if (parameter.Step is Enum)
                {

                }
            }
        }

        public void Stop()
        {
            RunningMode = RunningMode.Stopped;
        }
    }
}
