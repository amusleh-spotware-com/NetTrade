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

        public event OnBacktestStartHandler OnBacktestStartEvent;

        public event OnBacktestPauseHandler OnBacktestPauseEvent;

        public event OnBacktestStopHandler OnBacktestStopEvent;

        public async void Start(IRobot robot, IBacktestSettings settings)
        {
            if (Robot != null && Robot.RunningMode != RunningMode.Stopped)
            {
                throw new InvalidOperationException("You can only start the backtester when robot is stopped");
            }

            Robot = robot;

            OnBacktestStartEvent?.Invoke(this, Robot);

            await StartDataFeed(settings.StartTime, settings.EndTime).ConfigureAwait(false);
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

        private async Task StartDataFeed(DateTimeOffset startTime, DateTimeOffset endTime)
        {
            var symbol = Robot.Settings.MainSymbol as Symbol;

            for (var currentTime = startTime; currentTime <= endTime; currentTime = currentTime.AddTicks(1))
            {
                bool continueDataFeed = await ContinueDataFeed();

                if (!continueDataFeed)
                {
                    break;
                }

                Robot.SetTimeByBacktester(this, currentTime);

                var symbolBar = symbol.BarsData.FirstOrDefault(iBar => iBar.Time == currentTime);

                if (symbolBar == null)
                {
                    continue;
                }

                foreach (var otherSymbol in Robot.Settings.OtherSymbols)
                {
                    var otherSymbolBar = (otherSymbol as Symbol).BarsData.FirstOrDefault(iBar => iBar.Time == symbolBar.Time);

                    if (otherSymbolBar != null)
                    {
                        (otherSymbol as Symbol).PublishBar(otherSymbolBar);
                    }
                }

                symbol.PublishBar(symbolBar);
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
            var trades = tradeEngine.Trades.ToList();

            var result = new BacktestResult
            {
                TotalTradesNumber = trades.Count,
                LongTradesNumber = trades.Where(iTrade => iTrade.Order.TradeType == TradeType.Buy).Count(),
                ShortTradesNumber = trades.Where(iTrade => iTrade.Order.TradeType == TradeType.Sell).Count(),
                NetProfit = trades.Select(iTrade => iTrade.Order.NetProfit).Sum(),
                WinningRate = trades.Where(iTrade => iTrade.Order.NetProfit > 0).Count() / trades.Count,
            };

            var grossProfit = trades.Where(iTrade => iTrade.Order.GrossProfit > 0)
                .Select(iTrade => iTrade.Order.GrossProfit).Sum();

            var grossLoss = trades.Where(iTrade => iTrade.Order.GrossProfit < 0)
                .Select(iTrade => iTrade.Order.GrossProfit).Sum();

            result.ProfitFactor = grossProfit / grossLoss;

            result.MaxEquityDrawdown = MaxDrawdownCalculator.GetMaxDrawdown(Robot.Settings.Account.EquityChanges);
            result.MaxBalanceDrawdown = MaxDrawdownCalculator.GetMaxDrawdown(Robot.Settings.Account.BalanceChanges);

            return result;
        }
    }
}