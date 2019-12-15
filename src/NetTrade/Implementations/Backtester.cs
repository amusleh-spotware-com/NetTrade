using NetTrade.Helpers;
using NetTrade.Interfaces;
using NetTrade.Models;
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

            StartIteration();

            var result = GetResult();

            OnBacktestFinishedEvent?.Invoke(this, result);
        }

        public void Pause()
        {
            OnBacktestPauseEvent?.Invoke(this, _robot);
        }

        public void Stop()
        {
            OnBacktestStopEvent?.Invoke(this, _robot);
        }

        private void StartIteration()
        {
            var symbolDataOrdered = _robot.Settings.MainSymbol.Data.OrderBy(iBar => iBar.Time);

            foreach (var bar in symbolDataOrdered)
            {
                var index = _robot.Settings.MainSymbol.Bars.AddValue(bar);

                _robot.Settings.MainSymbol.Data.Remove(bar);

                foreach (var otherSymbol in _robot.Settings.OtherSymbols)
                {
                    var otherSymbolBar = otherSymbol.Data.FirstOrDefault(iBar => iBar.Time == bar.Time);

                    if (otherSymbolBar != null)
                    {
                        otherSymbol.Bars.AddValue(otherSymbolBar);

                        otherSymbol.Data.Remove(otherSymbolBar);
                    }
                }

                _robot.OnBar(index);
            }
        }

        public IBacktestResult GetResult()
        {
            throw new NotImplementedException();
        }
    }
}