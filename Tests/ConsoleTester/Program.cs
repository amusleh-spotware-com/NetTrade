using System;
using ConsoleTester.Robots;
using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Models;
using NetTrade.Timers;
using NetTrade.TradeEngines;
using NetTrade.Symbols;
using NetTrade.BarTypes;
using System.Collections.Generic;
using NetTrade.Backtesters;
using NetTrade.Accounts;
using System.IO;
using CsvHelper;
using System.Linq;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
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
                Name = "AMZN",
                Slippage = 0
            };

            var data = GetData("Data\\daily_AMZN.csv");

            var startTime = data.Min(iBar => iBar.Time);
            var endTime = data.Max(iBar => iBar.Time);

            var symbolsData = new List<ISymbolBacktestData> { new SymbolBacktestData(symbol, data) };

            var robotParmeters = new RobotParameters
            {
                Account = new BacktestAccount(1, 1, string.Empty, 500, "ConsoleTester"),
                Backtester = new OhlcBacktester { Interval = TimeSpan.FromHours(1) },
                BacktestSettings = new BacktestSettings(startTime, endTime, symbolsData),
                Mode = Mode.Backtest,
                Server = new Server(),
                Symbols = new List<ISymbol> { symbol },
                Timer = new DefaultTimer(),
            };

            robotParmeters.TradeEngine = new BacktestTradeEngine(robotParmeters.Server, robotParmeters.Account);

            robotParmeters.Account.AddTransaction(new Transaction(10000, startTime));

            robotParmeters.Backtester.OnBacktestStopEvent += Backtester_OnBacktestStopEvent;

            var robot = new SingleSymbolMaCrossOverBot();

            robot.Start(robotParmeters);

            Console.ReadLine();
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

            Console.ReadLine();
        }

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
