using NetTrade.Helpers;
using NetTrade.Interfaces;
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

            DataReader.SetSymbolsData(_robot.Settings.CsvConfiguration, _robot.Settings.MainSymbol);
            DataReader.SetSymbolsData(_robot.Settings.CsvConfiguration, _robot.Settings.OtherSymbols.ToArray());

            StartIteration();

            OnBacktestStartEvent?.Invoke(this, _robot);
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
            foreach (var bar in _robot.Settings.MainSymbol.Data)
            {
                _robot.Settings.MainSymbol.Bars.AddValue(bar);

                foreach (var otherSymbol in _robot.Settings.OtherSymbols)
                {
                    var otherSymbolBar = otherSymbol.Data.FirstOrDefault(iBar => iBar.Time == bar.Time);

                    if (otherSymbolBar != null)
                    {
                        otherSymbol.Bars.AddValue(otherSymbolBar);
                    }
                }
            }
        }
    }
}