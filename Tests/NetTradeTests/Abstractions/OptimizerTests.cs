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
using NetTrade.Symbols;

namespace NetTrade.Abstractions.Tests
{
    [TestClass()]
    public class OptimizerTests
    {
        private readonly Mock<Optimizer> _optimizerMock;

        private readonly OptimizerSettings _optimizerSettings;

        public OptimizerTests()
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

            var symbol = new OhlcSymbol(new Mock<IBars>().Object) { Name = "Main" };
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

            _optimizerMock = new Mock<Optimizer>(_optimizerSettings);

        }

        [TestMethod()]
        public void OptimizerTest()
        {
            Assert.IsNotNull(_optimizerMock.Object);
        }
    }
}