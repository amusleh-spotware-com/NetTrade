using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetTrade.Abstractions
{
    public abstract class Backtester : IBacktester
    {
        public IRobot Robot { get; private set; }
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(1);

        public IBacktestSettings Settings { get; private set; }

        public IEnumerable<ISymbolBacktestData> SymbolsData { get; private set; }

        public event OnBacktestStartHandler OnBacktestStartEvent;

        public event OnBacktestPauseHandler OnBacktestPauseEvent;

        public event OnBacktestStopHandler OnBacktestStopEvent;

        public event OnBacktestProgressChangedHandler OnBacktestProgressChangedEvent;

        public virtual Task StartAsync(IRobot robot, IBacktestSettings settings,
            IEnumerable<ISymbolBacktestData> symbolsBacktestData)
        {
            _ = robot ?? throw new ArgumentNullException(nameof(robot));

            Robot = robot;

            if (Robot.RunningMode != RunningMode.Running)
            {
                throw new InvalidOperationException("You can only start the backtester when robot is in running mode");
            }

            InvokeOnBacktestStartEvent();

            Settings = settings;

            SymbolsData = symbolsBacktestData;

            return StartDataFeed();
        }

        public virtual IBacktestResult GetResult()
        {
            var tradeEngine = Robot.Trade;
            var trades = tradeEngine.Trades.ToList();

            var result = new BacktestResult
            {
                TotalTradesNumber = trades.Count,
                LongTradesNumber = trades.Where(iTrade => iTrade.Order.TradeType == TradeType.Buy).Count(),
                ShortTradesNumber = trades.Where(iTrade => iTrade.Order.TradeType == TradeType.Sell).Count(),
                NetProfit = trades.Select(iTrade => iTrade.Order.NetProfit).Sum(),
                WinningRate = trades.Count > 0 ? trades.Where(iTrade => iTrade.Order.NetProfit > 0).Count() / (double)trades.Count * 100 : 0,
            };

            var grossProfit = trades.Where(iTrade => iTrade.Order.GrossProfit > 0).Sum(iTrade => iTrade.Order.GrossProfit);

            var grossLoss = trades.Where(iTrade => iTrade.Order.GrossProfit < 0).Sum(iTrade => Math.Abs(iTrade.Order.GrossProfit));

            result.ProfitFactor = grossLoss > 0 ? grossProfit / grossLoss : grossProfit;

            result.MaxEquityDrawdown = MaxDrawdownCalculator.GetMaxDrawdown(Robot.Account.EquityChanges);
            result.MaxBalanceDrawdown = MaxDrawdownCalculator.GetMaxDrawdown(Robot.Account.BalanceChanges);

            result.Commission = trades.Sum(iTrade => iTrade.Order.Commission);

            if (Robot.Account.CurrentBalance > 0)
            {
                var data = SymbolsData.Select(iSymbol => iSymbol.Data.Select(iBar => iBar.Close)).ToList();

                var dataReturns = data.Select(iSymbol => iSymbol.Skip(1)
                    .Zip(iSymbol, (current, previous) => previous == 0 ? 0 : Math.Round((current - previous) / previous, 2))
                    .Sum())
                    .Sum();

                var depositTransaction = Robot.Account.Transactions.FirstOrDefault(iTransaction => iTransaction.Amount > 0);

                if (depositTransaction != null && tradeEngine.Trades.Any())
                {
                    var initialDeposit = depositTransaction.Amount;

                    var returns = tradeEngine.Trades.Select(iTrade => iTrade.Order.NetProfit / initialDeposit).ToList();

                    result.VsBuyHoldRatio = returns.Sum() / dataReturns * 100;

                    var variance = returns.Select(iValue => Math.Pow(iValue - returns.Average(), 2)).Sum() / returns.Count > 1 ? (returns.Count - 1) : 1;

                    var standardDeviation = Math.Sqrt(variance);

                    result.SharpeRatio = returns.Average() / standardDeviation;

                    var downReturnsStd = returns.Where(iReturn => iReturn < 0).Select(iReturn => Math.Sqrt(Math.Pow(iReturn, 2)))
                        .Average();

                    result.SortinoRatio = returns.Average() / downReturnsStd;
                }
            }

            var winningTrades = tradeEngine.Trades.Where(iTrade => iTrade.Order.NetProfit > 0);
            var losingTrades = tradeEngine.Trades.Where(iTrade => iTrade.Order.NetProfit < 0);

            result.AverageProfit = winningTrades.Count() > 0 ? winningTrades.Average(iTrade => iTrade.Order.NetProfit) : 0;
            result.AverageLoss = losingTrades.Count() > 0 ? losingTrades.Average(iTrade => iTrade.Order.NetProfit) : 0;
            result.AverageReturn = tradeEngine.Trades.Count > 0 ? tradeEngine.Trades.Average(iTrade => iTrade.Order.NetProfit) : 0;

            return result;
        }

        protected virtual async Task<bool> ShouldContinueDataFeed()
        {
            if (Robot.RunningMode == RunningMode.Stopped)
            {
                InvokeOnBacktestStopEvent();

                return false;
            }
            else if (Robot.RunningMode == RunningMode.Paused)
            {
                InvokeOnBacktestPauseEvent();

                while (Robot.RunningMode == RunningMode.Paused)
                {
                    await Task.Delay(1000);
                }

                return await ShouldContinueDataFeed();
            }

            return true;
        }

        protected abstract Task StartDataFeed();

        protected void InvokeOnBacktestStartEvent()
        {
            OnBacktestStartEvent?.Invoke(this, Robot);
        }

        protected void InvokeOnBacktestProgressChangedEvent(DateTimeOffset currentTime)
        {
            OnBacktestProgressChangedEvent?.Invoke(this, currentTime);
        }

        protected void InvokeOnBacktestStopEvent()
        {
            OnBacktestStopEvent?.Invoke(this, Robot);
        }

        protected void InvokeOnBacktestPauseEvent()
        {
            OnBacktestPauseEvent?.Invoke(this, Robot);
        }
    }
}