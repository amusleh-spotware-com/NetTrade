using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Models;
using NetTrade.Timers;
using NetTrade.TradeEngines;
using System;
using System.Collections.Generic;
using NetTrade.Symbols;

namespace NetTrade.Backtesters.Tests
{
    [TestClass()]
    public class OhlcBacktesterTests
    {
        private readonly Mock<Robot> _robotMock;

        private readonly Mock<IRobotParameters> _robotParametersMock;

        private readonly OhlcBacktester _backtester;

        private readonly BacktestSettings _backtestSettings;

        public OhlcBacktesterTests()
        {
            _robotParametersMock = new Mock<IRobotParameters>();

            var symbols = new List<ISymbol>
            {
                new OhlcSymbol(new Mock<IBars>().Object) { Name = "Main" },
                new OhlcSymbol(new Mock<IBars>().Object) { Name = "First"},
                new OhlcSymbol(new Mock<IBars>().Object) { Name = "Second"},
                new OhlcSymbol(new Mock<IBars>().Object) { Name = "Third"},
            };

            var startTime = DateTimeOffset.Now.AddDays(-30);
            var endTime = DateTimeOffset.Now;

            var symbolsData = new List<ISymbolBacktestData>();

            var random = new Random(0);

            symbols.ForEach(iSymbol => 
            {
                var symbolData = new SymbolBacktestData(iSymbol, GetData(100 * random.Next(1, 5), startTime, endTime));

                symbolsData.Add(symbolData);
            });

            _backtestSettings = new BacktestSettings(startTime, endTime, symbolsData);

            _backtester = new OhlcBacktester { Interval = TimeSpan.FromHours(1) };

            _robotParametersMock.SetupProperty(settings => settings.Symbols, symbols);
            _robotParametersMock.SetupProperty(settings => settings.Backtester, _backtester);
            _robotParametersMock.SetupProperty(settings => settings.BacktestSettings, _backtestSettings);
            _robotParametersMock.SetupProperty(settings => settings.Server, new Server());
            _robotParametersMock.SetupProperty(settings => settings.Account, new Mock<IAccount>().Object);
            _robotParametersMock.SetupProperty(settings => settings.Mode, Mode.Backtest);
            _robotParametersMock.SetupProperty(settings => settings.Timer, new DefaultTimer());

            var tradeEngine = new BacktestTradeEngine(_robotParametersMock.Object.Server, _robotParametersMock.Object.Account);

            _robotParametersMock.SetupProperty(settings => settings.TradeEngine, tradeEngine);

            _robotMock = new Mock<Robot>();
        }

        [TestMethod()]
        public void StartTest()
        {
            _robotMock.Object.Start(_robotParametersMock.Object);
        }

        private List<IBar> GetData(double startPrice, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            var interval = TimeSpan.FromDays(1);

            return SampleDataGenerator.GetSampleData(startPrice, startTime, endTime, interval);
        }
    }
}