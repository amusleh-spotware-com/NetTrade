using NetTrade.Helpers;
using NetTrade.Interfaces;
using System;
using System.Linq;

namespace NetTrade.Implementations
{
    public class Backtester : IBacktester
    {
        private IRobot _robot;

        public event OnBacktestFinished OnBacktestFinishedEvent;

        public event OnBacktestStart OnBacktestStartEvent;

        public event OnBacktestPause OnBacktestPauseEvent;

        public event OnBacktestStop OnBacktestStopEvent;

        public void Start(IRobot robot)
        {
            _robot = robot;

            OnBacktestStartEvent?.Invoke(this, _robot);

            StartDataFeed();

            OnBacktestFinishedEvent?.Invoke(this, _robot);
        }

        public void Pause()
        {
            StopDataFeed();

            OnBacktestPauseEvent?.Invoke(this, _robot);
        }

        public void Stop()
        {
            StopDataFeed();

            OnBacktestStopEvent?.Invoke(this, _robot);
        }

        private void StartDataFeed()
        {
            _robot.Settings.MainSymbol.SubscribeToDataFeed();

            foreach (var otherSymbol in _robot.Settings.OtherSymbols)
            {
                otherSymbol.SubscribeToDataFeed();
            }
        }

        private void StopDataFeed()
        {
            _robot.Settings.MainSymbol.UnsubscribeFromDataFeed();

            foreach (var otherSymbol in _robot.Settings.OtherSymbols)
            {
                otherSymbol.UnsubscribeFromDataFeed();
            }
        }

        public IBacktestResult GetResult()
        {
            throw new NotImplementedException();
        }
    }
}