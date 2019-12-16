using NetTrade.Enums;
using NetTrade.Interfaces;
using NetTrade.Models;
using System;

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
            RunningMode = RunningMode.Running;

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
            RunningMode = RunningMode.Stopped;

            OnStop();
        }

        public void Pause()
        {
            RunningMode = RunningMode.Paused;
        }

        public void Resume()
        {
            RunningMode = RunningMode.Running;
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

        private void Backtester_OnBacktestFinishedEvent(object sender, IBacktestResult result)
        {
            throw new NotImplementedException();
        }

        #endregion Backtester event handlers
    }
}