using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetTrade.Implementations
{
    public class Backtester : IBacktester
    {
        public IRobot Robot { get; private set; }

        public DateTimeOffset StartTime { get; private set; }

        public DateTimeOffset EndTime { get; private set; }

        public event OnBacktestStart OnBacktestStartEvent;

        public event OnBacktestPause OnBacktestPauseEvent;

        public event OnBacktestStop OnBacktestStopEvent;

        public void SetTime(DateTimeOffset startTime, DateTimeOffset endTime)
        {
            if (Robot != null && Robot.RunningMode != RunningMode.Stopped)
            {
                throw new InvalidOperationException("You can only change the backtester time when robot is stopped");
            }

            StartTime = startTime;

            EndTime = endTime;
        }

        public void Start(IRobot robot)
        {
            if (Robot != null && Robot.RunningMode != RunningMode.Stopped)
            {
                throw new InvalidOperationException("You can only start the backtester when robot is stopped");
            }

            Robot = robot;

            OnBacktestStartEvent?.Invoke(this, Robot);

            Task.Run(StartDataFeed);
        }

        public void Pause()
        {
            if (Robot != null && Robot.RunningMode != RunningMode.Running)
            {
                throw new InvalidOperationException("You can only pause the backtester when robot is running");
            }

            OnBacktestPauseEvent?.Invoke(this, Robot);
        }

        public void Stop()
        {
            if (Robot != null && Robot.RunningMode == RunningMode.Stopped)
            {
                throw new InvalidOperationException("You can only pause the backtester when robot is running/paused");
            }

            OnBacktestStopEvent?.Invoke(this, Robot);
        }

        private async void StartDataFeed()
        {
            var symbol = Robot.Settings.MainSymbol as Symbol;

            var barsDataOrdered = symbol.BarsData.OrderBy(iBar => iBar.Time);

            foreach (var bar in barsDataOrdered)
            {
                if (bar.Time < StartTime || bar.Time > EndTime)
                {
                    continue;
                }

                bool continueDataFeed = await ContinueDataFeed();

                if (!continueDataFeed)
                {
                    break;
                }

                foreach (var otherSymbol in Robot.Settings.OtherSymbols)
                {
                    var otherSymbolBar = (otherSymbol as Symbol).BarsData.FirstOrDefault(iBar => iBar.Time == bar.Time);

                    if (otherSymbolBar != null)
                    {
                        (otherSymbol as Symbol).PublishBar(otherSymbolBar);
                    }
                }

                symbol.PublishBar(bar);
            }

            if (Robot.RunningMode != RunningMode.Stopped)
            {
                Stop();
            }
        }

        private async Task<bool> ContinueDataFeed()
        {
            if (Robot.RunningMode == RunningMode.Stopped)
            {
                return false;
            }
            else if (Robot.RunningMode == RunningMode.Paused)
            {
                while (Robot.RunningMode == RunningMode.Paused)
                {
                    await Task.Delay(1000);
                }

                return await ContinueDataFeed();
            }

            return true;
        }

        public IBacktestResult GetResult()
        {
            var tradeEngine = Robot.Settings.Account.Trade;

            var result = new BacktestResult
            {
                TotalTradesNumber = tradeEngine.Trades.Count,
                LongTradesNumber = tradeEngine.Trades.Where(iTrade => iTrade.Order.TradeType == TradeType.Buy).Count(),
                ShortTradesNumber = tradeEngine.Trades.Where(iTrade => iTrade.Order.TradeType == TradeType.Sell).Count(),
                NetProfit = tradeEngine.Trades.Select(iTrade => iTrade.NetProfit).Sum(),
                WinningRate = tradeEngine.Trades.Where(iTrade => iTrade.NetProfit > 0).Count() / tradeEngine.Trades.Count,
            };

            var grossProfit = tradeEngine.Trades.Where(iTrade => iTrade.GrossProfit > 0).Select(iTrade => iTrade.GrossProfit).Sum();
            var grossLoss = tradeEngine.Trades.Where(iTrade => iTrade.GrossProfit < 0).Select(iTrade => iTrade.GrossProfit).Sum();

            result.ProfitFactor = grossProfit / grossLoss;

            return result;
        }
    }
}