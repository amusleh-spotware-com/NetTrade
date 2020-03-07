using NetTrade.Abstractions.Interfaces;
using NetTrade.Accounts;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetTrade.Abstractions
{
    public abstract class Optimizer : IOptimizer
    {
        private ConcurrentBag<Robot> _robots;

        private CancellationTokenSource _cancellationTokenSource;

        public Optimizer(IOptimizerSettings settings)
        {
            Settings = settings;
        }

        public IOptimizerSettings Settings { get; }

        public RunningMode RunningMode { get; private set; } = RunningMode.Stopped;

        public event OnOptimizationPassCompletionHandler OnOptimizationPassCompletionEvent;

        public event OnOptimizationStartedHandler OnOptimizationStartedEvent;

        public event OnOptimizationStoppedHandler OnOptimizationStoppedEvent;

        public virtual IRobotParameters GetRobotParameters()
        {
            var robotParameters = Activator.CreateInstance(Settings.RobotSettingsType, Settings.RobotSettingsParameters) as
                IRobotParameters;

            robotParameters.Mode = Mode.Backtest;

            robotParameters.SymbolsBacktestData = Settings.SymbolsData.Select(iSymbolData => iSymbolData.Clone() as ISymbolBacktestData).ToList();

            robotParameters.Symbols = robotParameters.SymbolsBacktestData.Select(iSymbolData => iSymbolData.Symbol).ToList();

            robotParameters.BacktestSettings = Activator.CreateInstance(Settings.BacktestSettingsType,
                Settings.BacktestSettingsParameters) as IBacktestSettings;

            robotParameters.Backtester = Activator.CreateInstance(Settings.BacktesterType, Settings.BacktesterParameters) as
                IBacktester;

            robotParameters.Backtester.Interval = Settings.BacktesterInterval;

            robotParameters.Server = Activator.CreateInstance(Settings.ServerType, Settings.ServerParameters) as IServer;

            robotParameters.Timer = Activator.CreateInstance(Settings.TimerType, Settings.TimerParameters) as ITimer;

            robotParameters.Account = new BacktestAccount(0, 0, string.Empty, Settings.AccountLeverage, "Optimizer");

            var transaction = new Transaction(Settings.AccountBalance, robotParameters.BacktestSettings.StartTime, string.Empty);

            robotParameters.Account.AddTransaction(transaction);

            var tradeEngineParameters = new List<object> { robotParameters.Server, robotParameters.Account };

            if (Settings.TradeEngineParameters != null)
            {
                tradeEngineParameters.AddRange(Settings.TradeEngineParameters);
            }

            robotParameters.TradeEngine = Activator.CreateInstance(Settings.TradeEngineType, tradeEngineParameters.ToArray()) as
                ITradeEngine;

            return robotParameters;
        }

        public void Start()
        {
            if (RunningMode == RunningMode.Running)
            {
                throw new InvalidOperationException("The optimizer is already in running mode");
            }

            _robots = new ConcurrentBag<Robot>();

            OnStart();

            RunningMode = RunningMode.Running;

            OnOptimizationStartedEvent?.Invoke(this);

            StartRobotsBacktest();
        }

        public Task StartAsync() => Task.Run(Start);

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

        protected abstract void OnStart();

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

        protected virtual void StartRobotsBacktest()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = _cancellationTokenSource.Token,
                MaxDegreeOfParallelism = Settings.MaxProcessorNumber
            };

            try
            {
                var robotsCount = _robots.Count;

                Parallel.For(0, robotsCount, parallelOptions, async (iRobotIndex, state) =>
                {
                    if (_robots.TryTake(out var iRobot))
                    {
                        var robotParameters = GetRobotParameters();

                        robotParameters.Backtester.OnBacktestStopEvent += Backtester_OnBacktestStopEvent;

                        await iRobot.StartAsync(robotParameters).ConfigureAwait(false);
                    }

                    if (parallelOptions.CancellationToken.IsCancellationRequested)
                    {
                        state.Stop();
                    }
                });
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (RunningMode == RunningMode.Running)
                {
                    Stop();
                }

                _cancellationTokenSource.Dispose();
            }
        }

        private void Backtester_OnBacktestStopEvent(object sender, IRobot robot)
        {
            OnOptimizationPassCompletionEvent?.Invoke(this, robot);
        }
    }
}