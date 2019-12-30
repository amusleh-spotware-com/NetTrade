using NetTrade.Enums;
using NetTrade.Interfaces;
using System;
using NetTrade.Implementations;

namespace NetTrade.Abstractions
{
    public abstract class Robot : IRobot
    {
        public Robot(IRobotSettings settings)
        {
            Settings = settings;
        }

        public IRobotSettings Settings { get; }

        public RunningMode RunningMode { get; private set; }

        public void Start()
        {
            Settings.MainSymbol.OnTickEvent += Symbol_OnTickEvent;
            Settings.MainSymbol.Bars.OnBarEvent += SymbolBars_OnBarEvent;

            foreach (var symbol in Settings.OtherSymbols)
            {
                symbol.OnTickEvent += Symbol_OnTickEvent;
                symbol.Bars.OnBarEvent += SymbolBars_OnBarEvent;
            }

            RunningMode = RunningMode.Running;

            OnStart();

            switch (Settings.Mode)
            {
                case Mode.Backtest:
                    Backtest();
                    break;

                case Mode.Live:
                    Live();
                    break;
            }
        }

        public void Stop()
        {
            RunningMode = RunningMode.Stopped;

            OnStop();
        }

        public void Pause()
        {
            RunningMode = RunningMode.Paused;

            switch (Settings.Mode)
            {
                case Mode.Backtest:
                    Settings.Backtester.Pause();
                    break;

                case Mode.Live:
                    break;
            }
        }

        public void Resume()
        {
            RunningMode = RunningMode.Running;
        }

        public void SetTimeByBacktester(IBacktester backtester, DateTimeOffset time)
        {
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

            (Settings.Server as Server).CurrentTime = time;
        }

        public virtual void OnTick(ISymbol symbol)
        {
        }

        public virtual void OnBar(ISymbol symbol, int index)
        {
        }

        public abstract void OnStart();

        public abstract void OnStop();

        #region Robot different mode methods

        protected virtual void Backtest()
        {
            Settings.Backtester.OnBacktestStartEvent += Backtester_OnBacktestStartEvent;
            Settings.Backtester.OnBacktestPauseEvent += Backtester_OnBacktestPauseEvent;
            Settings.Backtester.OnBacktestStopEvent += Backtester_OnBacktestStopEvent;

            Settings.Backtester.Start(this, Settings.BacktestSettings);
        }

        protected virtual void Live()
        {
        }

        #endregion Robot different mode methods

        #region Backtester event handlers

        protected virtual void Backtester_OnBacktestStopEvent(object sender, IRobot robot)
        {
        }

        protected virtual void Backtester_OnBacktestPauseEvent(object sender, IRobot robot)
        {
        }

        protected virtual void Backtester_OnBacktestStartEvent(object sender, IRobot robot)
        {
        }

        #endregion Backtester event handlers

        #region Symbols on tick/bar event handlers

        private void Symbol_OnTickEvent(object sender)
        {
            var symbol = sender as ISymbol;

            Settings.Account.Trade.UpdateSymbolOrders(symbol);

            OnTick(symbol);
        }

        private void SymbolBars_OnBarEvent(object sender, int index)
        {
            var symbol = sender as ISymbol;

            OnBar(symbol, index);
        }

        #endregion Symbols on tick/bar event handlers
    }
}