using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Backtesters;
using NetTrade.BarTypes;
using NetTrade.Helpers;
using NetTrade.Models;
using NetTrade.Timers;
using NetTrade.TradeEngines;
using NetTradeTests.Samples;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetTrade.Symbols;

namespace NetTrade.Optimizers.Tests
{
    [TestClass()]
    public class GridOptimizerTests
    {
        private readonly GridOptimizer _optimizer;

        private readonly OptimizerSettings _optimizerSettings;

        public GridOptimizerTests()
        {
            var startTime = DateTimeOffset.Now.AddDays(-10);
            var endTime = DateTimeOffset.Now;

            _optimizerSettings = new OptimizerSettings
            {
                AccountBalance = 10000,
                AccountLeverage = 500,
                BacktesterType = typeof(OhlcBacktester),
                BacktestSettingsType = typeof(BacktestSettings),
            };

            var data = SampleDataGenerator.GetSampleData(200, startTime, endTime, TimeSpan.FromDays(1));

            var symbol = new OhlcSymbol(new TimeBasedBars(TimeSpan.FromDays(1))) { Name = "Main" };
            var symbolData = new SymbolBacktestData(symbol, data);

            _optimizerSettings.SymbolsData = new List<ISymbolBacktestData> { symbolData };
            _optimizerSettings.BacktestSettingsParameters = new List<object> 
            { 
                startTime,
                endTime,
            }.ToArray();
            _optimizerSettings.TradeEngineType = typeof(BacktestTradeEngine);
            _optimizerSettings.TimerType = typeof(DefaultTimer);
            _optimizerSettings.ServerType = typeof(Server);
            _optimizerSettings.RobotSettingsType = typeof(RobotParameters);
            _optimizerSettings.RobotType = typeof(SampleBot);
            _optimizerSettings.Parameters = new List<OptimizeParameter>()
            {
                new OptimizeParameter("Periods", 30, 50, 10),
                new OptimizeParameter("Deviation", 2),
                new OptimizeParameter("Range", 2000, 6000, 2000)
            };
            _optimizerSettings.BacktesterInterval = TimeSpan.FromHours(1);

            _optimizer = new GridOptimizer(_optimizerSettings);
        }

        [TestMethod()]
        public void GridOptimizerTest()
        {
            Assert.IsNotNull(_optimizer);
        }

        [TestMethod()]
        public void GetRobotSettingsTest()
        {
            var robotParameters = _optimizer.GetRobotParameters();

            Assert.IsNotNull(robotParameters);
        }

        [TestMethod()]
        public void StartTest()
        {
            int completedPassCounter = 0;

            _optimizer.OnOptimizationPassCompletionEvent += (sender, robot) =>
            {
                completedPassCounter++;
            };

            _optimizer.Start();

            Assert.AreEqual(_optimizer.Robots.Count, completedPassCounter);
        }

        [TestMethod()]
        public async Task StartAsyncTest()
        {
            int completedPassCounter = 0;

            _optimizer.OnOptimizationPassCompletionEvent += (sender, robot) =>
            {
                completedPassCounter++;
            };

            await _optimizer.StartAsync();

            Assert.AreEqual(_optimizer.Robots.Count, completedPassCounter);
        }
    }
}