using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Interfaces;
using System.Linq;
using NetTrade.Implementations;

namespace NetTrade.Abstractions
{
    public abstract class Optimizer : IOptimizer
    {
        protected List<Robot> _robots = new List<Robot>();

        public Optimizer(IOptimizerSettings settings)
        {
            Settings = settings;
        }

        public IOptimizerSettings Settings { get; }

        public RunningMode RunningMode { get; protected set; }

        public IReadOnlyList<Robot> Robots => _robots;

        public event OnOptimizationPassCompletionHandler OnOptimizationPassCompletionEvent;
        public event OnOptimizationFinishedHandler OnOptimizationFinishedEvent;
        public event OnOptimizationProgressChangedHandler OnOptimizationProgressChangedEvent;

        public virtual IRobotSettings GetRobotSettings()
        {
            var robotSettings = Activator.CreateInstance(Settings.RobotSettingsType, Settings.RobotSettingsType) as
                IRobotSettings;

            robotSettings.Mode = Mode.Backtest;

            robotSettings.MainSymbol = Settings.MainSymbol.Clone() as ISymbol;

            if (Settings.OtherSymbols != null)
            {
                robotSettings.OtherSymbols = Settings.OtherSymbols.Select(iSymbol => iSymbol.Clone() as ISymbol).ToList();
            }

            robotSettings.BacktestSettings = Settings.BacktestSettings;

            robotSettings.Backtester = Activator.CreateInstance(Settings.BacktesterType, Settings.BacktesterParameters) as
                IBacktester;

            var accountTransactions = new List<Transaction>
            {
                new Transaction(Settings.AccountBalance, Settings.BacktestSettings.StartTime, string.Empty)
            };

            robotSettings.Server = Activator.CreateInstance(Settings.ServerType, Settings.ServerParameters) as IServer;

            var tradeEngineParameters = new List<object> { robotSettings.Server };

            if (Settings.TradeEngineParameters != null)
            {
                tradeEngineParameters.AddRange(Settings.TradeEngineParameters);
            }

            var tradeEngine = Activator.CreateInstance(Settings.TradeEngineType, tradeEngineParameters) as ITradeEngine;

            robotSettings.Account = new Account(0, 0, string.Empty, Settings.AccountLeverage, "Optimizer", accountTransactions,
                tradeEngine);

            return robotSettings;
        }

        public abstract void Pause();

        public abstract void Start<TRobot>() where TRobot : Robot;

        public abstract void Stop();
    }
}
