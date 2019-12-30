using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Implementations;
using NetTrade.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetTrade.Abstractions
{
    public abstract class Optimizer : IOptimizer
    {
        private readonly List<Robot> _robots = new List<Robot>();

        private CancellationTokenSource _cancellationTokenSource;

        public Optimizer(IOptimizerSettings settings)
        {
            Settings = settings;
        }

        public IOptimizerSettings Settings { get; }

        public RunningMode RunningMode { get; private set; }

        public IReadOnlyList<Robot> Robots => _robots;

        public event OnOptimizationPassCompletionHandler OnOptimizationPassCompletionEvent;

        public event OnOptimizationStartedHandler OnOptimizationStartedEvent;

        public event OnOptimizationStoppedHandler OnOptimizationStoppedEvent;

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

        public void Start<TRobot>() where TRobot : Robot
        {
            if (RunningMode == RunningMode.Running)
            {
                throw new InvalidOperationException("The optimizer is already in running mode");
            }

            RunningMode = RunningMode.Running;

            _robots.Clear();

            OnOptimizationStartedEvent?.Invoke(this);

            OnStart<TRobot>();
        }

        public void Stop()
        {
            if (RunningMode == RunningMode.Stopped)
            {
                throw new InvalidOperationException("The optimizer is already stopped");
            }

            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }

            RunningMode = RunningMode.Stopped;

            OnOptimizationStoppedEvent?.Invoke(this);

            OnStop();
        }

        protected abstract void OnStart<TRobot>() where TRobot : Robot;

        protected virtual void OnStop()
        {
        }

        protected void AddRobot(Robot robot)
        {
            if (RunningMode == RunningMode.Running)
            {
                throw new InvalidOperationException("You can't add new robot while the optimizer is running");
            }

            _robots.Add(robot);
        }

        protected virtual void StartRobots()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = _cancellationTokenSource.Token,
                MaxDegreeOfParallelism = Settings.MaxProcessorNumber
            };

            try
            {
                Parallel.ForEach(_robots, parallelOptions, iRobot =>
                {
                    iRobot.Settings.Backtester.OnBacktestStopEvent += Backtester_OnBacktestStopEvent;

                    iRobot.Start();

                    parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                });

                if (RunningMode == RunningMode.Running)
                {
                    Stop();
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _cancellationTokenSource.Dispose();
            }
        }

        private void Backtester_OnBacktestStopEvent(object sender, IRobot robot)
        {
            OnOptimizationPassCompletionEvent?.Invoke(this, robot);
        }
    }
}