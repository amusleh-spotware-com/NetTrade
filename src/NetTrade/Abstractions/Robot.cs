using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Models;
using System;
using System.Timers;
using NetTrade.Attributes;
using System.Linq;
using System.Reflection;

namespace NetTrade.Abstractions
{
    public abstract class Robot : IRobot
    {
        private Timer _timer;

        public IRobotSettings Settings { get; private set; }

        public ITradeEngine Trade => Settings.TradeEngine;

        public RunningMode RunningMode { get; private set; } = RunningMode.Stopped;

        public void Start(IRobotSettings settings)
        {
            if (RunningMode == RunningMode.Running || RunningMode == RunningMode.Paused)
            {
                throw new InvalidOperationException("The robot is already in running/paused mode");
            }

            _ = settings ?? throw new ArgumentNullException(nameof(settings));

            Settings = settings;

            Settings.MainSymbol.OnTickEvent += Symbol_OnTickEvent;
            Settings.MainSymbol.Bars.OnBarEvent += SymbolBars_OnBarEvent;

            if (Settings.OtherSymbols != null)
            {
                foreach (var symbol in Settings.OtherSymbols)
                {
                    symbol.OnTickEvent += Symbol_OnTickEvent;
                    symbol.Bars.OnBarEvent += SymbolBars_OnBarEvent;
                }
            }

            Settings.Timer.OnTimerElapsedEvent += timer => OnTimer();

            SetParameterValuesToDefault();

            RunningMode = RunningMode.Running;

            OnStart();

            if (Settings.Mode == Mode.Backtest)
            {
                Backtest();
            }
            else
            {
                InitializeLiveTimer();

                Settings.Timer.OnTimerIntervalChangedEvent += (timer, interval) => _timer.Interval = interval.TotalMilliseconds;
                Settings.Timer.OnTimerStartEvent += timer => _timer.Start();
                Settings.Timer.OnTimerStopEvent += timer => _timer.Stop();
            }
        }

        public void Stop()
        {
            if (RunningMode == RunningMode.Stopped)
            {
                throw new InvalidOperationException("The robot is already stopped");
            }

            RunningMode = RunningMode.Stopped;

            if (Settings.Mode == Mode.Live)
            {
                _timer.Dispose();
            }

            OnStop();
        }

        public void Pause()
        {
            if (RunningMode != RunningMode.Running)
            {
                throw new InvalidOperationException("The robot is not running");
            }

            RunningMode = RunningMode.Paused;

            if (Settings.Mode == Mode.Live)
            {
                _timer.Stop();
            }

            OnPause();
        }

        public void Resume()
        {
            if (RunningMode != RunningMode.Paused)
            {
                throw new InvalidOperationException("The robot is not paused");
            }

            RunningMode = RunningMode.Running;

            if (Settings.Mode == Mode.Live)
            {
                _timer.Start();
            }

            OnResume();
        }

        public void SetTimeByBacktester(IBacktester backtester, DateTimeOffset time)
        {
            _ = backtester ?? throw new ArgumentNullException(nameof(backtester));

            if (Settings.Mode == Mode.Live)
            {
                throw new InvalidOperationException("You can not set the robot time with a back tester when the robot is on" +
                    " live mode");
            }

            if (Settings.Backtester != backtester)
            {
                throw new InvalidOperationException("You can not set the robot time with another back tester, " +
                    "the provided back tester isn't the one available on robot settings");
            }

            Settings.Timer.SetCurrentTime(time);

            (Settings.Server as Server).CurrentTime = time;
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

            Settings.TradeEngine.UpdateSymbolOrders(symbol);

            if (RunningMode == RunningMode.Running)
            {
                OnTick(symbol);
            }
        }

        private void SymbolBars_OnBarEvent(object sender, int index)
        {
            var symbol = sender as ISymbol;

            if (RunningMode == RunningMode.Running)
            {
                OnBar(symbol, index);
            }
        }

        #endregion Symbols on tick/bar event handlers

        #region Backtest methods and event handlers

        private void Backtest()
        {
            Settings.Backtester.OnBacktestStopEvent += Backtester_OnBacktestStopEvent;
            Settings.Backtester.OnBacktestStartEvent += Backtester_OnBacktestStartEvent;
            Settings.Backtester.OnBacktestPauseEvent += Backtester_OnBacktestPauseEvent;

            Settings.Backtester.StartAsync(this, Settings.BacktestSettings);
        }

        protected virtual void Backtester_OnBacktestPauseEvent(object sender, IRobot robot)
        {
        }

        protected virtual void Backtester_OnBacktestStartEvent(object sender, IRobot robot)
        {
        }

        protected virtual void Backtester_OnBacktestStopEvent(object sender, IRobot robot)
        {
            if (RunningMode != RunningMode.Stopped)
            {
                Stop();
            }
        }

        #endregion Backtest methods and event handlers

        #region Other methods

        private void InitializeLiveTimer()
        {
            _timer = new Timer(Settings.Timer.Interval.TotalMilliseconds);

            _timer.Elapsed += (sender, args) => Settings.Timer.SetCurrentTime(DateTimeOffset.Now);

            if (Settings.Timer.Enabled)
            {
                _timer.Start();
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