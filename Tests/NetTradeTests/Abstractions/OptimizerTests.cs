using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Backtesters;
using NetTrade.Helpers;
using NetTrade.Models;
using NetTrade.Timers;
using NetTrade.TradeEngines;
using NetTradeTests.Samples;
using System;
using System.Collections.Generic;

namespace NetTrade.Abstractions.Tests
{
    [TestClass()]
    public class OptimizerTests
    {
        private Mock<Optimizer> _optimizerMock;

        private OptimizerSettings _optimizerSettings;

        public OptimizerTests()
        {
            _optimizerSettings = new OptimizerSettings();

            _optimizerSettings.AccountBalance = 10000;
            _optimizerSettings.AccountLeverage = 500;
            _optimizerSettings.BacktesterType = typeof(DefaultBacktester);
            _optimizerSettings.BacktestSettingsType = typeof(BacktestSettings);
            _optimizerSettings.BacktestSettingsParameters = new List<object>
            {
                DateTimeOffset.Now.AddDays(-10),
                DateTimeOffset.Now
            }.ToArray();

            var symbolData = SampleDataGenerator.GetSampleData(200, DateTimeOffset.Now.AddDays(-10), DateTimeOffset.Now,
                TimeSpan.FromDays(1));

            var symbol = new Symbol(symbolData, new Mock<IBars>().Object) { Name = "Main" };

            _optimizerSettings.MainSymbol = symbol;
            _optimizerSettings.TradeEngineType = typeof(BacktestTradeEngine);
            _optimizerSettings.TimerType = typeof(DefaultTimer);
            _optimizerSettings.ServerType = typeof(Server);
            _optimizerSettings.RobotSettingsType = typeof(RobotSettings);
            _optimizerSettings.RobotType = typeof(SampleBot);

            _optimizerMock = new Mock<Optimizer>(_optimizerSettings);

        }

        [TestMethod()]
        public void OptimizerTest()
        {
            Assert.IsNotNull(_optimizerMock.Object);
        }
    }
}