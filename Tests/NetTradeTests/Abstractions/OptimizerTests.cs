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
        private readonly Mock<Optimizer> _optimizerMock;

        private readonly OptimizerSettings _optimizerSettings;

        public OptimizerTests()
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

            var symbol = new Symbol(symbolData, new Mock<IBars>().Object) { Name = "Main" };

            _optimizerSettings.Symbols = new List<ISymbol> { symbol };
            _optimizerSettings.TradeEngineType = typeof(BacktestTradeEngine);
            _optimizerSettings.TimerType = typeof(DefaultTimer);
            _optimizerSettings.ServerType = typeof(Server);
            _optimizerSettings.RobotSettingsType = typeof(RobotParameters);
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