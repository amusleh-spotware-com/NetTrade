using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace NetTrade.Optimizers.Tests
{
    [TestClass()]
    public class GridOptimizerTests
    {
        private readonly GridOptimizer _optimizer;

        private readonly OptimizerSettings _optimizerSettings;

        public GridOptimizerTests()
        {
            _optimizerSettings = new OptimizerSettings
            {
                AccountBalance = 10000,
                AccountLeverage = 500,
                BacktesterType = typeof(DefaultBacktester),
                BacktestSettingsType = typeof(BacktestSettings),
                BacktestSettingsParameters = new List<object>
            {
                DateTimeOffset.Now.AddDays(-10),
                DateTimeOffset.Now
            }.ToArray()
            };

            var symbolData = SampleDataGenerator.GetSampleData(200, DateTimeOffset.Now.AddDays(-10), DateTimeOffset.Now,
                TimeSpan.FromDays(1));

            var symbol = new Symbol(symbolData, new TimeBasedBars(TimeSpan.FromDays(1))) { Name = "Main" };

            _optimizerSettings.MainSymbol = symbol;
            _optimizerSettings.TradeEngineType = typeof(BacktestTradeEngine);
            _optimizerSettings.TimerType = typeof(DefaultTimer);
            _optimizerSettings.ServerType = typeof(Server);
            _optimizerSettings.RobotSettingsType = typeof(RobotSettings);
            _optimizerSettings.RobotType = typeof(SampleBot);
            _optimizerSettings.Parameters = new List<OptimizeParameter>()
            {
                new OptimizeParameter("Periods", 20, 50, 10),
                new OptimizeParameter("Deviation", 2, 3, 1),
                new OptimizeParameter("Range", 2000, 6000, 2000)
            };

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
            var robotSettings = _optimizer.GetRobotSettings();

            Assert.IsNotNull(robotSettings);
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