using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using NetTrade.Symbols;

namespace NetTrade.Backtesters
{
    public class OhlcBacktester : IBacktester
    {
        public IRobot Robot { get; private set; }
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(1);

        public event OnBacktestStartHandler OnBacktestStartEvent;

        public event OnBacktestPauseHandler OnBacktestPauseEvent;

        public event OnBacktestStopHandler OnBacktestStopEvent;

        public Task StartAsync(IRobot robot, IBacktestSettings settings)
        {
            _ = robot ?? throw new ArgumentNullException(nameof(robot));

            Robot = robot;

            if (Robot.RunningMode != RunningMode.Running)
            {
                throw new InvalidOperationException("You can only start the backtester when robot is in running mode");
            }

            OnBacktestStartEvent?.Invoke(this, Robot);

            return StartDataFeedAsync(settings);
        }

        public IBacktestResult GetResult()
        {
            var tradeEngine = Robot.Trade;
            var trades = tradeEngine.Trades.ToList();

            var result = new BacktestResult
            {
                TotalTradesNumber = trades.Count,
                LongTradesNumber = trades.Where(iTrade => iTrade.Order.TradeType == TradeType.Buy).Count(),
                ShortTradesNumber = trades.Where(iTrade => iTrade.Order.TradeType == TradeType.Sell).Count(),
                NetProfit = trades.Select(iTrade => iTrade.Order.NetProfit).Sum(),
                WinningRate = trades.Count > 0 ? trades.Where(iTrade => iTrade.Order.NetProfit > 0).Count() / (double)trades.Count : 0,
            };

            var grossProfit = trades.Where(iTrade => iTrade.Order.GrossProfit > 0)
                .Select(iTrade => iTrade.Order.GrossProfit).Sum();

            var grossLoss = trades.Where(iTrade => iTrade.Order.GrossProfit < 0)
                .Select(iTrade => iTrade.Order.GrossProfit).Sum();

            result.ProfitFactor = grossProfit / grossLoss;

            result.MaxEquityDrawdown = MaxDrawdownCalculator.GetMaxDrawdown(Robot.Account.EquityChanges);
            result.MaxBalanceDrawdown = MaxDrawdownCalculator.GetMaxDrawdown(Robot.Account.BalanceChanges);

            return result;
        }

        private async Task StartDataFeedAsync(IBacktestSettings settings)
        {
            for (var currentTime = settings.StartTime; currentTime <= settings.EndTime; currentTime = currentTime.Add(Interval))
            {
                bool shouldContinueDataFeed = await ShouldContinueDataFeed();

                if (!shouldContinueDataFeed)
                {
                    break;
                }

                Robot.SetTimeByBacktester(this, currentTime);

                foreach (var symbolData in settings.SymbolsData)
                {
                    var bar = symbolData.GetBar(currentTime);

                    if (bar != null)
                    {
                        (symbolData.Symbol as OhlcSymbol).PublishBar(bar);
                    }
                }
            }

            OnBacktestStopEvent?.Invoke(this, Robot);
        }

        private async Task<bool> ShouldContinueDataFeed()
        {
            if (Robot.RunningMode == RunningMode.Stopped)
            {
                OnBacktestStopEvent?.Invoke(this, Robot);

                return false;
            }
            else if (Robot.RunningMode == RunningMode.Paused)
            {
                OnBacktestPauseEvent?.Invoke(this, Robot);

                while (Robot.RunningMode == RunningMode.Paused)
                {
                    await Task.Delay(1000);
                }

                return await ShouldContinueDataFeed();
            }

            return true;
        }
    }
}