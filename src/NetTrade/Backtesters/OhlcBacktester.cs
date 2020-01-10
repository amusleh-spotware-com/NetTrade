using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Models;
using NetTrade.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetTrade.Backtesters
{
    public class OhlcBacktester : IBacktester
    {
        public IRobot Robot { get; private set; }
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(1);

        public event OnBacktestStartHandler OnBacktestStartEvent;

        public event OnBacktestPauseHandler OnBacktestPauseEvent;

        public event OnBacktestStopHandler OnBacktestStopEvent;

        public event OnBacktestProgressChangedHandler OnBacktestProgressChangedEvent;

        public Task StartAsync(IRobot robot, IBacktestSettings settings, IEnumerable<ISymbolBacktestData> symbolsBacktestData)
        {
            _ = robot ?? throw new ArgumentNullException(nameof(robot));

            Robot = robot;

            if (Robot.RunningMode != RunningMode.Running)
            {
                throw new InvalidOperationException("You can only start the backtester when robot is in running mode");
            }

            OnBacktestStartEvent?.Invoke(this, Robot);

            return StartDataFeed(settings, symbolsBacktestData);
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

            var grossProfit = trades.Where(iTrade => iTrade.Order.GrossProfit > 0).Sum(iTrade => iTrade.Order.GrossProfit);

            var grossLoss = trades.Where(iTrade => iTrade.Order.GrossProfit < 0).Sum(iTrade => Math.Abs(iTrade.Order.GrossProfit));

            result.ProfitFactor = grossProfit / grossLoss;

            result.MaxEquityDrawdown = MaxDrawdownCalculator.GetMaxDrawdown(Robot.Account.EquityChanges);
            result.MaxBalanceDrawdown = MaxDrawdownCalculator.GetMaxDrawdown(Robot.Account.BalanceChanges);

            result.Commission = trades.Sum(iTrade => iTrade.Order.Commission);

            return result;
        }

        private async Task StartDataFeed(IBacktestSettings settings, IEnumerable<ISymbolBacktestData> symbolsBacktestData)
        {
            for (var currentTime = settings.StartTime; currentTime <= settings.EndTime; currentTime = currentTime.Add(Interval))
            {
                bool shouldContinueDataFeed = await ShouldContinueDataFeed();

                if (!shouldContinueDataFeed)
                {
                    break;
                }

                Robot.SetTimeByBacktester(this, currentTime);

                foreach (var symbolData in symbolsBacktestData)
                {
                    var bar = symbolData.GetBar(currentTime);

                    if (bar != null)
                    {
                        (symbolData.Symbol as OhlcSymbol).PublishBar(bar);
                    }
                }

                OnBacktestProgressChangedEvent?.Invoke(this, currentTime);
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