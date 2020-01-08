﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTrade.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Abstractions.Interfaces;
using Moq;
using NetTrade.Enums;
using NetTrade.Models;
using NetTrade.Timers;

namespace NetTrade.Abstractions.Tests
{
    [TestClass()]
    public class RobotTests
    {
        private readonly Mock<Robot> _robotMock;

        private readonly Mock<IRobotParameters> _robotSettingsMock;

        public RobotTests()
        {
            _robotSettingsMock = new Mock<IRobotParameters>();

            var symbolMock = new Mock<Symbol>(new List<IBar>(), new Mock<IBars>().Object);

            _robotSettingsMock.SetupProperty(settings => settings.Symbols, new List<ISymbol> { symbolMock.Object });
            _robotSettingsMock.SetupProperty(settings => settings.Backtester, new Mock<IBacktester>().Object);
            _robotSettingsMock.SetupProperty(settings => settings.BacktestSettings, new Mock<IBacktestSettings>().Object);
            _robotSettingsMock.SetupProperty(settings => settings.Server, new Server());
            _robotSettingsMock.SetupProperty(settings => settings.Account, new Mock<IAccount>().Object);
            _robotSettingsMock.SetupProperty(settings => settings.TradeEngine, new Mock<ITradeEngine>().Object);
            _robotSettingsMock.SetupProperty(settings => settings.Mode, Mode.Backtest);
            _robotSettingsMock.SetupProperty(settings => settings.Timer, new DefaultTimer());

            _robotMock = new Mock<Robot>();
        }

        [TestMethod()]
        public void StartTest()
        {
            if (_robotMock.Object.RunningMode == RunningMode.Running)
            {
                _robotMock.Object.Stop();
            }

            _robotMock.Object.Start(_robotSettingsMock.Object);

            Assert.AreEqual(_robotMock.Object.RunningMode, RunningMode.Running);
        }

        [TestMethod()]
        public void StopTest()
        {
            if (_robotMock.Object.RunningMode != RunningMode.Running)
            {
                _robotMock.Object.Start(_robotSettingsMock.Object);
            }

            _robotMock.Object.Stop();

            Assert.AreEqual(_robotMock.Object.RunningMode, RunningMode.Stopped);
        }

        [TestMethod()]
        public void PauseTest()
        {
            if (_robotMock.Object.RunningMode != RunningMode.Running)
            {
                _robotMock.Object.Start(_robotSettingsMock.Object);
            }

            _robotMock.Object.Pause();

            Assert.AreEqual(_robotMock.Object.RunningMode, RunningMode.Paused);
        }

        [TestMethod()]
        public void ResumeTest()
        {
            if (_robotMock.Object.RunningMode == RunningMode.Stopped)
            {
                _robotMock.Object.Start(_robotSettingsMock.Object);
            }

            if (_robotMock.Object.RunningMode == RunningMode.Running)
            {
                _robotMock.Object.Pause();
            }

            _robotMock.Object.Resume();

            Assert.AreEqual(_robotMock.Object.RunningMode, RunningMode.Running);
        }

        [TestMethod()]
        public void SetTimeByBacktesterTest()
        {
            if (_robotMock.Object.RunningMode == RunningMode.Running)
            {
                _robotMock.Object.Stop();
            }

            if (_robotMock.Object.RunningMode == RunningMode.Stopped)
            {
                _robotMock.Object.Start(_robotSettingsMock.Object);
            }

            _robotMock.Object.Stop();

            var now = DateTimeOffset.UtcNow;

            _robotMock.Object.SetTimeByBacktester(_robotSettingsMock.Object.Backtester, now);

            Assert.AreEqual(now, _robotMock.Object.Server.CurrentTime);

            var anotherBacktester = new Mock<IBacktester>();

            Assert.ThrowsException<InvalidOperationException>(() => _robotMock.Object.SetTimeByBacktester(anotherBacktester.Object, now));
        }

        [TestMethod()]
        public void OnStartTest()
        {
            if (_robotMock.Object.RunningMode == RunningMode.Running)
            {
                _robotMock.Object.Stop();
            }

            _robotMock.Object.Start(_robotSettingsMock.Object);

            _robotMock.Verify(robot => robot.OnStart(), Times.Once);
        }

        [TestMethod()]
        public void OnPauseTest()
        {
            if (_robotMock.Object.RunningMode != RunningMode.Running)
            {
                _robotMock.Object.Start(_robotSettingsMock.Object);
            }

            _robotMock.Object.Pause();

            _robotMock.Verify(robot => robot.OnPause(), Times.Once);
        }

        [TestMethod()]
        public void OnResumeTest()
        {
            if (_robotMock.Object.RunningMode == RunningMode.Stopped)
            {
                _robotMock.Object.Start(_robotSettingsMock.Object);
            }

            if (_robotMock.Object.RunningMode == RunningMode.Running)
            {
                _robotMock.Object.Pause();
            }

            _robotMock.Object.Resume();

            _robotMock.Verify(robot => robot.OnResume(), Times.Once);
        }

        [TestMethod()]
        public void OnStopTest()
        {
            if (_robotMock.Object.RunningMode != RunningMode.Running)
            {
                _robotMock.Object.Start(_robotSettingsMock.Object);
            }

            _robotMock.Object.Stop();

            _robotMock.Verify(robot => robot.OnStop(), Times.Once);
        }
    }
}