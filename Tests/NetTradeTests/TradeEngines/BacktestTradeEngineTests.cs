using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTrade.TradeEngines;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NetTrade.Abstractions.Interfaces;
using NetTrade.Models;
using NetTrade.Enums;
using System.Linq;
using NetTrade.Accounts;

namespace NetTrade.TradeEngines.Tests
{
    [TestClass()]
    public class BacktestTradeEngineTests
    {
        private BacktestTradeEngine _tradeEngine;

        private ISymbol _symbol;

        public BacktestTradeEngineTests()
        {
            var server = new Server
            {
                CurrentTime = DateTimeOffset.Now
            };


            var account = new DefaultAccount(1, 1, "Demo", 500, "Tester");

            account.AddTransaction(new Transaction(10000, DateTimeOffset.Now, string.Empty));

            _symbol = new Symbol(new List<IBar>(), new Mock<IBars>().Object)
            {
                Digits = 5,
                TickSize = 0.00001,
                TickValue = 1,
                VolumeStep = 1000,
                MaxVolume = 100000000,
                MinVolume = 1000,
                VolumeUnitValue = 1,
                Commission = 1,
                Name = "EURUSD",
                Slippage = 0.0001
            };

            _tradeEngine = new BacktestTradeEngine(server, account);
        }

        [TestMethod()]
        public void BacktestTradeEngineTest()
        {
            Assert.IsNotNull(_tradeEngine);
        }

        [TestMethod()]
        public void ExecuteTest()
        {
            var orderParameters = new MarketOrderParameters(_symbol)
            {
                Volume = 1000,
                TradeType = TradeType.Buy,
                StopLossPrice = .95,
                TakeProfitPrice = 1.05,
            };

            var result = _tradeEngine.Execute(orderParameters);

            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod()]
        public void UpdateSymbolOrdersTest()
        {
            var orderParameters = new MarketOrderParameters(_symbol)
            {
                Volume = 1000,
                TradeType = TradeType.Buy,
                StopLossPrice = .95,
                TakeProfitPrice = 1.05,
            };

            var result = _tradeEngine.Execute(orderParameters);

            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(_tradeEngine.Orders.Contains(result.Order));

            (_symbol as Symbol).PublishBar(new Bar(DateTimeOffset.Now, 1, 1, 1, 1.1, 1000));

            _tradeEngine.UpdateSymbolOrders(_symbol);

            Assert.IsTrue(!_tradeEngine.Orders.Contains(result.Order));
        }

        [TestMethod()]
        public void CloseMarketOrderTest()
        {
            var orderParameters = new MarketOrderParameters(_symbol)
            {
                Volume = 1000,
                TradeType = TradeType.Buy,
                StopLossPrice = .95,
                TakeProfitPrice = 1.05,
            };

            var result = _tradeEngine.Execute(orderParameters);

            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(_tradeEngine.Orders.Contains(result.Order));

            _tradeEngine.CloseMarketOrder(result.Order as MarketOrder);

            Assert.IsTrue(!_tradeEngine.Orders.Contains(result.Order));
        }

        [TestMethod()]
        public void CancelPendingOrderTest()
        {
            (_symbol as Symbol).PublishBar(new Bar(DateTimeOffset.Now, 1, 1, 1, 1.5, 1000));

            var orderParameters = new PendingOrderParameters(OrderType.Limit, _symbol)
            {
                Volume = 1000,
                TargetPrice = 1,
                TradeType = TradeType.Buy,
                StopLossPrice = .95,
                TakeProfitPrice = 1.05,
            };

            var result = _tradeEngine.Execute(orderParameters);

            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(_tradeEngine.Orders.Contains(result.Order));

            _tradeEngine.CancelPendingOrder(result.Order as PendingOrder);

            Assert.IsTrue(!_tradeEngine.Orders.Contains(result.Order));
        }
    }
}