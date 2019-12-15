using NetTrade.Enums;
using NetTrade.Interfaces;
using NetTrade.Models;
using System;

namespace NetTrade.Abstractions
{
    public abstract class Robot : IRobot
    {
        public Robot(RobotSettings settings)
        {
            Settings = settings;
        }

        public bool IsRunning { get; private set; }

        public RobotSettings Settings { get; }

        public void Start()
        {
            IsRunning = true;

            OnStart();

            switch (Settings.Mode)
            {
                case Mode.Backtest:
                    Backtest();
                    break;

                case Mode.Optimization:
                    break;

                case Mode.Live:
                    break;
            }
        }

        public void Stop()
        {
            IsRunning = false;

            OnStop();
        }

        public void Pause()
        {
            IsRunning = false;
        }

        public abstract void OnBar(int index);

        public abstract void OnStart();

        public abstract void OnStop();

        private void Backtest()
        {
            Settings.Backtester.OnBacktestFinishedEvent += Backtester_OnBacktestFinishedEvent;
            Settings.Backtester.OnBacktestStartEvent += Backtester_OnBacktestStartEvent;
            Settings.Backtester.OnBacktestPauseEvent += Backtester_OnBacktestPauseEvent; ;
            Settings.Backtester.OnBacktestStopEvent += Backtester_OnBacktestStopEvent; ;

            Settings.Backtester.Start(this);
        }

        #region Backtester event handlers

        private void Backtester_OnBacktestStopEvent(object sender, IRobot robot)
        {
            throw new NotImplementedException();
        }

        private void Backtester_OnBacktestPauseEvent(object sender, IRobot robot)
        {
            throw new NotImplementedException();
        }

        private void Backtester_OnBacktestStartEvent(object sender, IRobot robot)
        {
            throw new NotImplementedException();
        }

        private void Backtester_OnBacktestFinishedEvent(object sender, BackTestResult result)
        {
        }

        #endregion Backtester event handlers
    }
}