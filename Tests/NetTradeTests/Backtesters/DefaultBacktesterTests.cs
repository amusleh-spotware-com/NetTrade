using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetTrade.Abstractions;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Models;
using NetTrade.TradeEngines;
using System;
using System.Collections.Generic;
using NetTrade.Timers;

namespace NetTrade.Backtesters.Tests
{
    [TestClass()]
    public class DefaultBacktesterTests
    {
        private readonly Mock<Robot> _robotMock;

        private readonly Mock<IRobotSettings> _robotSettingsMock;

        private readonly DefaultBacktester _backtester;

        private readonly BacktestSettings _backtestSettings;

        private readonly Symbol _mainSymbol;

        public DefaultBacktesterTests()
        {
            _robotSettingsMock = new Mock<IRobotSettings>();

            _backtestSettings = new BacktestSettings(DateTimeOffset.Now.AddDays(-30), DateTimeOffset.Now);

            _mainSymbol = new Symbol(GetData(100, _backtestSettings), new Mock<IBars>().Object) { Name = "Main" };

            var otherSymbols = new List<ISymbol>
            {
                new Symbol(GetData(200, _backtestSettings), new Mock<IBars>().Object) { Name = "First"},
                new Symbol(GetData(300, _backtestSettings), new Mock<IBars>().Object) { Name = "Second"},
                new Symbol(GetData(400, _backtestSettings), new Mock<IBars>().Object) { Name = "Third"},
            };

            _backtester = new DefaultBacktester { Interval = TimeSpan.FromHours(1) };

            _robotSettingsMock.SetupProperty(settings => settings.MainSymbol, _mainSymbol);
            _robotSettingsMock.SetupProperty(settings => settings.OtherSymbols, otherSymbols);
            _robotSettingsMock.SetupProperty(settings => settings.Backtester, _backtester);
            _robotSettingsMock.SetupProperty(settings => settings.BacktestSettings, _backtestSettings);
            _robotSettingsMock.SetupProperty(settings => settings.Server, new Server());
            _robotSettingsMock.SetupProperty(settings => settings.Account, new Mock<IAccount>().Object);
            _robotSettingsMock.SetupProperty(settings => settings.Mode, Mode.Backtest);
            _robotSettingsMock.SetupProperty(settings => settings.Timer, new DefaultTimer());

            var tradeEngine = new BacktestTradeEngine(_robotSettingsMock.Object.Server, _robotSettingsMock.Object.Account);

            _robotSettingsMock.SetupProperty(settings => settings.TradeEngine, tradeEngine);

            _robotMock = new Mock<Robot>();
        }

        [TestMethod()]
        public void StartTest()
        {
            _robotMock.Object.Start(_robotSettingsMock.Object);
        }

        private List<IBar> GetData(double startPrice, IBacktestSettings backtestSettings)
        {
            var interval = TimeSpan.FromDays(1);

            return SampleDataGenerator.GetSampleData(startPrice, backtestSettings.StartTime, backtestSettings.EndTime, interval);
        }
    }
}