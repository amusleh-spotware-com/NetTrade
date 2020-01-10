using NetTrade.Abstractions.Interfaces;
using NetTrade.Attributes;
using NetTrade.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;
using NetTrade.Exceptions;

namespace NetTrade.Abstractions
{
    public abstract class Robot : IRobot
    {
        private Timer _systemTimer;

        public Robot()
        {
            SetParameterValuesToDefault();
        }

        public ITradeEngine Trade { get; private set; }

        public RunningMode RunningMode { get; private set; } = RunningMode.Stopped;

        public IAccount Account { get; private set; }

        public IEnumerable<ISymbol> Symbols { get; private set; }

        public Mode Mode { get; private set; }

        public IBacktester Backtester { get; private set; }

        public IBacktestSettings BacktestSettings { get; private set; }

        public IServer Server { get; private set; }

        public ITimer Timer { get; private set; }

        public void Start(IRobotParameters parameters)
        {
            if (RunningMode == RunningMode.Running || RunningMode == RunningMode.Paused)
            {
                throw new InvalidOperationException("The robot is already in running/paused mode");
            }

            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));

            Trade = parameters.TradeEngine;
            Account = parameters.Account;
            Symbols = parameters.Symbols;
            Mode = parameters.Mode;
            Backtester = parameters.Backtester;
            BacktestSettings = parameters.BacktestSettings;
            Server = parameters.Server;
            Timer = parameters.Timer;

            foreach (var symbol in Symbols)
            {
                symbol.OnTickEvent += Symbol_OnTickEvent;
                symbol.OnBarEvent += SymbolBars_OnBarEvent;
            }

            Timer.OnTimerElapsedEvent += timer => OnTimer();

            Account.OnMarginCallEvent += Account_OnMarginCallEvent;

            RunningMode = RunningMode.Running;

            try
            {
                OnStart();
            }
            catch (Exception ex)
            {
                Stop();

                throw new RobotException(RobotExceptionSource.OnStart, ex);
            }

            if (Mode == Mode.Backtest)
            {
                Backtest(parameters.SymbolsBacktestData);
            }
            else
            {
                InitializeLiveTimer();

                Timer.OnTimerIntervalChangedEvent += (timer, interval) => _systemTimer.Interval = interval.TotalMilliseconds;
                Timer.OnTimerStartEvent += timer => _systemTimer.Start();
                Timer.OnTimerStopEvent += timer => _systemTimer.Stop();
            }
        }

        public void Stop()
        {
            if (RunningMode == RunningMode.Stopped)
            {
                return;
            }

            RunningMode = RunningMode.Stopped;

            if (Mode == Mode.Live)
            {
                _systemTimer.Dispose();
            }

            try
            {
                OnStop();
            }
            catch (Exception ex)
            {
                throw new RobotException(RobotExceptionSource.OnStop, ex);
            }
        }

        public void Pause()
        {
            if (RunningMode != RunningMode.Running)
            {
                throw new InvalidOperationException("The robot is not running");
            }

            RunningMode = RunningMode.Paused;

            if (Mode == Mode.Live)
            {
                _systemTimer.Stop();
            }

            try
            {
                OnPause();
            }
            catch (Exception ex)
            {
                if (RunningMode != RunningMode.Stopped)
                {
                    Stop();
                }

                throw new RobotException(RobotExceptionSource.OnPause, ex);
            }
        }

        public void Resume()
        {
            if (RunningMode != RunningMode.Paused)
            {
                throw new InvalidOperationException("The robot is not paused");
            }

            RunningMode = RunningMode.Running;

            if (Mode == Mode.Live)
            {
                _systemTimer.Start();
            }

            try
            {
                OnResume();
            }
            catch (Exception ex)
            {
                if (RunningMode != RunningMode.Stopped)
                {
                    Stop();
                }

                throw new RobotException(RobotExceptionSource.OnResume, ex);
            }
        }

        public void SetTimeByBacktester(IBacktester backtester, DateTimeOffset time)
        {
            _ = backtester ?? throw new ArgumentNullException(nameof(backtester));

            if (Mode == Mode.Live)
            {
                throw new InvalidOperationException("You can not set the robot time with a back tester when the robot is on" +
                    " live mode");
            }

            if (Backtester != backtester)
            {
                throw new InvalidOperationException("You can not set the robot time with another back tester, " +
                    "the provided back tester isn't the one available on robot settings");
            }

            Timer.SetCurrentTime(time);

            Server.SetTime(this, time);
        }

        public virtual void OnTick(ISymbol symbol)
        {
        }

        public virtual void OnBar(ISymbol symbol, int index)
        {
        }

        public virtual void OnTimer()
        {
        }

        public virtual void OnPause()
        {
        }

        public virtual void OnResume()
        {
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnStop()
        {
        }

        #region Symbols on tick/bar event handlers

        private void Symbol_OnTickEvent(object sender)
        {
            var symbol = sender as ISymbol;

            Trade.UpdateSymbolOrders(symbol);

            try
            {
                if (RunningMode == RunningMode.Running)
                {
                    OnTick(symbol);
                }
            }
            catch (Exception ex)
            {
                if (RunningMode != RunningMode.Stopped)
                {
                    Stop();
                }

                throw new RobotException(RobotExceptionSource.OnBar, ex);
            }
        }

        private void SymbolBars_OnBarEvent(object sender, int index)
        {
            var symbol = sender as ISymbol;

            try
            {
                if (RunningMode == RunningMode.Running)
                {
                    OnBar(symbol, index);
                }
            }
            catch (Exception ex)
            {
                if (RunningMode != RunningMode.Stopped)
                {
                    Stop();
                }

                throw new RobotException(RobotExceptionSource.OnBar, ex);
            }
        }

        #endregion Symbols on tick/bar event handlers

        #region Backtest methods and event handlers

        private async void Backtest(IEnumerable<ISymbolBacktestData> symbolsBacktestData)
        {
            _ = Backtester ?? throw new NullReferenceException(nameof(Backtester));

            Backtester.OnBacktestStopEvent += Backtester_OnBacktestStopEvent;
            Backtester.OnBacktestStartEvent += Backtester_OnBacktestStartEvent;
            Backtester.OnBacktestPauseEvent += Backtester_OnBacktestPauseEvent;

            await Backtester.StartAsync(this, BacktestSettings, symbolsBacktestData);
        }

        protected virtual void Backtester_OnBacktestPauseEvent(object sender, IRobot robot)
        {
        }

        protected virtual void Backtester_OnBacktestStartEvent(object sender, IRobot robot)
        {
        }

        protected virtual void Backtester_OnBacktestStopEvent(object sender, IRobot robot)
        {
            Stop();
        }

        #endregion Backtest methods and event handlers

        #region Account event handlers

        protected virtual void Account_OnMarginCallEvent(object sender)
        {
            Stop();
        }

        #endregion

        #region Other methods

        private void InitializeLiveTimer()
        {
            _systemTimer = new Timer(Timer.Interval.TotalMilliseconds);

            _systemTimer.Elapsed += (sender, args) => Timer.SetCurrentTime(DateTimeOffset.Now);

            if (Timer.Enabled)
            {
                _systemTimer.Start();
            }
        }

        private void SetParameterValuesToDefault()
        {
            var robotParameters = this.GetType().GetProperties()
                .Where(iProperty => iProperty.GetCustomAttributes(true).Any()).ToList();

            if (!robotParameters.Any())
            {
                return;
            }

            foreach (var robotParamter in robotParameters)
            {
                if (!robotParamter.CanWrite)
                {
                    continue;
                }

                var parameterAttribute = robotParamter.GetCustomAttribute<ParameterAttribute>();

                if (parameterAttribute.DefaultValue != null)
                {
                    robotParamter.SetValue(this, parameterAttribute.DefaultValue);
                }
            }
        }

        #endregion Other methods
    }
}