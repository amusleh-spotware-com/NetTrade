using ConsoleTester.Robots;
using CsvHelper;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Accounts;
using NetTrade.Attributes;
using NetTrade.Backtesters;
using NetTrade.BarTypes;
using NetTrade.Enums;
using NetTrade.Models;
using NetTrade.Optimizers;
using NetTrade.Symbols;
using NetTrade.Timers;
using NetTrade.TradeEngines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConsoleTester
{
    internal class Program
    {
        private static object _optimizerLock = new object();

        private static void Main(string[] args)
        {
            var robotType = typeof(SingleSymbolMaCrossOverBot);

            var robotAttribute = robotType.GetCustomAttribute<RobotAttribute>();

            Console.WriteLine($"Testing {robotAttribute.Name}");

            Console.WriteLine("Please provide the symbol name (MSFT or AMZN)");

            var symbolName = Console.ReadLine().ToUpperInvariant();

            var symbol = new OhlcSymbol(new TimeBasedBars(TimeSpan.FromDays(1)))
            {
                Digits = 0,
                TickSize = 0.1,
                TickValue = 1,
                VolumeStep = 1000,
                MaxVolume = 100000000,
                MinVolume = 1000,
                VolumeUnitValue = 1,
                Commission = 1,
                Name = symbolName,
                Slippage = 0
            };

            var data = GetData($"Data\\daily_{symbolName}.csv");

            Console.WriteLine("For backtesting please type backtest and for optimization please type optimize, then press 'Enter'");

            var command = Console.ReadLine().ToLowerInvariant();

            switch (command)
            {
                case "backtest":
                    Backtest(symbol, data);
                    break;

                case "optimize":
                    Optimize(symbol, data);
                    break;
            }

            Main(args);
        }

        #region Backtest methods

        private static void Backtest(ISymbol symbol, IEnumerable<IBar> data)
        {
            var startTime = data.Min(iBar => iBar.Time);
            var endTime = data.Max(iBar => iBar.Time);

            var robotParmeters = new RobotParameters
            {
                Account = new BacktestAccount(1, 1, string.Empty, 500, "ConsoleTester"),
                Backtester = new OhlcBacktester { Interval = TimeSpan.FromHours(1) },
                BacktestSettings = new BacktestSettings(startTime, endTime),
                Mode = Mode.Backtest,
                Server = new Server(),
                Symbols = new List<ISymbol> { symbol },
                SymbolsBacktestData = new List<ISymbolBacktestData> { new SymbolBacktestData(symbol, data) },
                Timer = new DefaultTimer(),
            };

            robotParmeters.TradeEngine = new BacktestTradeEngine(robotParmeters.Server, robotParmeters.Account);

            robotParmeters.Account.AddTransaction(new Transaction(10000, startTime));

            robotParmeters.Backtester.OnBacktestStopEvent += Backtester_OnBacktestStopEvent;
            robotParmeters.Backtester.OnBacktestProgressChangedEvent += Backtester_OnBacktestProgressChangedEvent;

            var robot = new SingleSymbolMaCrossOverBot();

            robot.Start(robotParmeters);
        }

        private static void Backtester_OnBacktestProgressChangedEvent(object sender, DateTimeOffset time)
        {
            Console.WriteLine($"Current Backtest Time: {time}");
        }

        private static void Backtester_OnBacktestStopEvent(object sender, IRobot robot)
        {
            Console.WriteLine("Backtest stopped");

            var result = robot.Backtester.GetResult();

            Console.WriteLine($"Total Trades: {result.TotalTradesNumber}");
            Console.WriteLine($"Long Trades Number: {result.LongTradesNumber}");
            Console.WriteLine($"Short Trades Number: {result.ShortTradesNumber}");
            Console.WriteLine($"Winning Rate: {result.WinningRate}");
            Console.WriteLine($"Profit Factor: {result.ProfitFactor}");
            Console.WriteLine($"Max Balance Drawdown: {result.MaxBalanceDrawdown}");
            Console.WriteLine($"Max Equity Drawdown: {result.MaxEquityDrawdown}");
            Console.WriteLine($"Net Profit: {result.NetProfit}");
            Console.WriteLine($"Commission: {result.Commission}");
        }

        #endregion Backtest methods

        #region Optimization methods

        private static void Optimize(ISymbol symbol, IEnumerable<IBar> data)
        {
            var startTime = data.Min(iBar => iBar.Time);
            var endTime = data.Max(iBar => iBar.Time);

            var symbolsData = new List<ISymbolBacktestData> { new SymbolBacktestData(symbol, data) };

            var optimizerSettings = new OptimizerSettings
            {
                AccountBalance = 10000,
                AccountLeverage = 500,
                BacktesterType = typeof(OhlcBacktester),
                BacktestSettingsType = typeof(BacktestSettings),
                BacktesterInterval = TimeSpan.FromHours(1),
            };

            optimizerSettings.SymbolsData = symbolsData;
            optimizerSettings.BacktestSettingsParameters = new List<object>
            {
                startTime,
                endTime,
            }.ToArray();
            optimizerSettings.TradeEngineType = typeof(BacktestTradeEngine);
            optimizerSettings.TimerType = typeof(DefaultTimer);
            optimizerSettings.ServerType = typeof(Server);
            optimizerSettings.RobotSettingsType = typeof(RobotParameters);
            optimizerSettings.RobotType = typeof(SingleSymbolMaCrossOverBot);
            optimizerSettings.Parameters = new List<OptimizeParameter>()
            {
                new OptimizeParameter("Fast MA Period", 5, 15, 5),
                new OptimizeParameter("Slow MA Period", 20),
                new OptimizeParameter("Volume", 1)
            };

            var optimizer = new GridOptimizer(optimizerSettings);

            optimizer.OnOptimizationPassCompletionEvent += Optimizer_OnOptimizationPassCompletionEvent;
            optimizer.OnOptimizationStoppedEvent += Optimizer_OnOptimizationStoppedEvent;
            optimizer.OnOptimizationStartedEvent += Optimizer_OnOptimizationStartedEvent;

            optimizer.Start();
        }

        private static void Optimizer_OnOptimizationStartedEvent(object sender)
        {
            Console.WriteLine("Optimization Started");
        }

        private static void Optimizer_OnOptimizationStoppedEvent(object sender)
        {
            Console.WriteLine("Optimization Stopped");
        }

        private static void Optimizer_OnOptimizationPassCompletionEvent(object sender, IRobot robot)
        {
            lock (_optimizerLock)
            {
                Console.WriteLine("Optimization Pass Completed");

                var result = robot.Backtester.GetResult();

                Console.WriteLine($"Total Trades: {result.TotalTradesNumber}");
                Console.WriteLine($"Long Trades Number: {result.LongTradesNumber}");
                Console.WriteLine($"Short Trades Number: {result.ShortTradesNumber}");
                Console.WriteLine($"Winning Rate: {result.WinningRate}");
                Console.WriteLine($"Profit Factor: {result.ProfitFactor}");
                Console.WriteLine($"Max Balance Drawdown: {result.MaxBalanceDrawdown}");
                Console.WriteLine($"Max Equity Drawdown: {result.MaxEquityDrawdown}");
                Console.WriteLine($"Net Profit: {result.NetProfit}");
                Console.WriteLine($"Commission: {result.Commission}");

                Console.WriteLine("----------------------------------------------------------");
            }
        }

        #endregion Optimization methods

        private static IEnumerable<Bar> GetData(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                return csv.GetRecords<Bar>().ToList();
            }
        }
    }
}