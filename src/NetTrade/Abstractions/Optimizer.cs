﻿using NetTrade.Abstractions.Interfaces;
using NetTrade.Accounts;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Models;
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

        public RunningMode RunningMode { get; private set; } = RunningMode.Stopped;

        public IReadOnlyList<Robot> Robots => _robots;

        public event OnOptimizationPassCompletionHandler OnOptimizationPassCompletionEvent;

        public event OnOptimizationStartedHandler OnOptimizationStartedEvent;

        public event OnOptimizationStoppedHandler OnOptimizationStoppedEvent;

        public virtual IRobotSettings GetRobotSettings()
        {
            var robotSettings = Activator.CreateInstance(Settings.RobotSettingsType, Settings.RobotSettingsParameters) as
                IRobotSettings;

            robotSettings.Mode = Mode.Backtest;

            robotSettings.MainSymbol = Settings.MainSymbol.Clone() as ISymbol;

            if (Settings.OtherSymbols != null)
            {
                robotSettings.OtherSymbols = Settings.OtherSymbols.Select(iSymbol => iSymbol.Clone() as ISymbol).ToList();
            }

            robotSettings.BacktestSettings = Activator.CreateInstance(Settings.BacktestSettingsType,
                Settings.BacktestSettingsParameters) as IBacktestSettings;

            robotSettings.Backtester = Activator.CreateInstance(Settings.BacktesterType, Settings.BacktesterParameters) as
                IBacktester;

            robotSettings.Server = Activator.CreateInstance(Settings.ServerType, Settings.ServerParameters) as IServer;

            robotSettings.Timer = Activator.CreateInstance(Settings.TimerType, Settings.TimerParameters) as ITimer;

            robotSettings.Account = new DefaultAccount(0, 0, string.Empty, Settings.AccountLeverage, "Optimizer");

            var transaction = new Transaction(Settings.AccountBalance, robotSettings.BacktestSettings.StartTime, string.Empty);

            robotSettings.Account.AddTransaction(transaction);

            var tradeEngineParameters = new List<object> { robotSettings.Server, robotSettings.Account };

            if (Settings.TradeEngineParameters != null)
            {
                tradeEngineParameters.AddRange(Settings.TradeEngineParameters);
            }

            robotSettings.TradeEngine = Activator.CreateInstance(Settings.TradeEngineType, tradeEngineParameters.ToArray()) as
                ITradeEngine;

            return robotSettings;
        }

        public void Start()
        {
            if (RunningMode == RunningMode.Running)
            {
                throw new InvalidOperationException("The optimizer is already in running mode");
            }

            _robots.Clear();

            OnOptimizationStartedEvent?.Invoke(this);

            OnStart();

            RunningMode = RunningMode.Running;

            StartRobots();
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
                var robotsWithSettings = new List<(IRobot, IRobotSettings)>();

                foreach (var robot in _robots)
                {
                    var robotSettings = GetRobotSettings();

                    robotsWithSettings.Add((robot, robotSettings));
                }

                Parallel.ForEach(robotsWithSettings, parallelOptions, iRobotWithSettings =>
                {
                    iRobotWithSettings.Item2.Backtester.OnBacktestStopEvent += Backtester_OnBacktestStopEvent;

                    iRobotWithSettings.Item1.Start(iRobotWithSettings.Item2);

                    parallelOptions.CancellationToken.ThrowIfCancellationRequested();
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