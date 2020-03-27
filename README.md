# NetTrade

This is a .Net library for developing trading algorithms. You will be able to write your algorithm in C# and backtest/optimize it with just a few lines of code.

NetTrade is very adaptive, you can customize it based on your needs or you can develop a fully functional algo trading platform on top of it.

The library itself contains a default backtester/optimizer, but you can create your own backtester/optimizer based on your requirements.

It is written on .Net Standard and you will be able to use it throughout all of the .Net family.

The idea behind NetTrade was to develop a lightweight library that performs all the hard work of backtesting, optimization, and live trading of a trading algorithm without being dependent on any outside .Net built-in libraries.

NetTrade makes the development of trading algorithms very simple, and if you have any experience with popular trading platforms like MetaTrader 4/5, cTrader, or NinjaTrader you will find NetTrade very similar to those platforms.

## Quick Start

Install NetTrade on your .Net project via Nuget: 

Let's create a simple moving average crossover trading system:

```c# 
[Robot(Name = "Single Symbol Ma Cross Over Bot", Group = "Sample")]
    public class SingleSymbolMaCrossOverBot : Robot
    {
        private SimpleMovingAverage _fastMa, _slowMa;

        [Parameter("Fast MA Period", DefaultValue = 5)]
        public int FastMaPeriod { get; set; }

        [Parameter("Slow MA Period", DefaultValue = 10)]
        public int SlowMaPeriod { get; set; }

        [Parameter("Volume", DefaultValue = 1)]
        public double Volume { get; set; }

        public override void OnStart()
        {
            if (Symbols.Count() > 1)
            {
                throw new InvalidOperationException("This robot is only for single symbol use, not multi symbol");
            }

            _fastMa = new SimpleMovingAverage(Symbols.First()) { DataSourceType = DataSourceType.Close, Periods = FastMaPeriod };

            _slowMa = new SimpleMovingAverage(Symbols.First()) { DataSourceType = DataSourceType.Close, Periods = SlowMaPeriod };
        }

        public override void OnBar(ISymbol symbol, int index)
        {
            if (_fastMa.Data[index] > _slowMa.Data[index])
            {
                Trade.CloseAllMarketOrders(TradeType.Sell);

                if (_fastMa.Data[index - 1] <= _slowMa.Data[index - 1] && !Trade.Orders.Any(iOrder => iOrder.OrderType == OrderType.Market && iOrder.TradeType == TradeType.Buy))
                {
                    var marketOrderParameters = new MarketOrderParameters(symbol)
                    {
                        Volume = Volume,
                        TradeType = TradeType.Buy,
                    };

                    Trade.Execute(marketOrderParameters);
                }
            }
            else if (_fastMa.Data[index] < _slowMa.Data[index])
            {
                Trade.CloseAllMarketOrders(TradeType.Buy);

                if (_fastMa.Data[index - 1] >= _slowMa.Data[index - 1] && !Trade.Orders.Any(iOrder => iOrder.OrderType == OrderType.Market && iOrder.TradeType == TradeType.Sell))
                {
                    var marketOrderParameters = new MarketOrderParameters(symbol)
                    {
                        Volume = Volume,
                        TradeType = TradeType.Sell,
                    };

                    Trade.Execute(marketOrderParameters);
                }
            }
        }
    }
```

As you can see the code is very clean and compact, you just have to create a class that's driven from the NetTrade abstract Robot class, and you will be able to use all of the NetTrade features like backtesting and optimization.

Code for backtesting the simple moving average crossover system:

```c# 
        private async static void Backtest(ISymbol symbol, IEnumerable<IBar> data)
        {
            var startTime = data.Min(iBar => iBar.Time);
            var endTime = data.Max(iBar => iBar.Time);

            var robotParmeters = new RobotParameters
            {
                Account = new BacktestAccount(1, 1, string.Empty, 500, "ConsoleTester"),
                Backtester = new OhlcBacktester { Interval = TimeSpan.FromHours(1) },
                BacktestSettings = new BacktestSettings(startTime, endTime),
                Mode = Mode.Backtest,
                Server = new Server(),
                Symbols = new List<ISymbol> { symbol },
                SymbolsBacktestData = new List<ISymbolBacktestData> { new SymbolBacktestData(symbol, data) },
                Timer = new DefaultTimer(),
            };

            robotParmeters.TradeEngine = new BacktestTradeEngine(robotParmeters.Server, robotParmeters.Account);

            robotParmeters.Account.AddTransaction(new Transaction(10000, startTime));

            robotParmeters.Backtester.OnBacktestStopEvent += Backtester_OnBacktestStopEvent;
            robotParmeters.Backtester.OnBacktestProgressChangedEvent += Backtester_OnBacktestProgressChangedEvent;

            var robot = new SingleSymbolMaCrossOverBot();

            await robot.StartAsync(robotParmeters);
        }
```

Code for optimizing the simple moving average crossover system:

```c# 
        private static void Optimize(ISymbol symbol, IEnumerable<IBar> data)
        {
            var startTime = data.Min(iBar => iBar.Time);
            var endTime = data.Max(iBar => iBar.Time);

            var symbolsData = new List<ISymbolBacktestData> { new SymbolBacktestData(symbol, data) };

            var optimizerSettings = new OptimizerSettings
            {
                AccountBalance = 10000,
                AccountLeverage = 500,
                BacktesterType = typeof(OhlcBacktester),
                BacktestSettingsType = typeof(BacktestSettings),
                BacktesterInterval = TimeSpan.FromHours(1),
            };

            optimizerSettings.SymbolsData = symbolsData;
            optimizerSettings.BacktestSettingsParameters = new List<object>
            {
                startTime,
                endTime,
            }.ToArray();
            optimizerSettings.TradeEngineType = typeof(BacktestTradeEngine);
            optimizerSettings.TimerType = typeof(DefaultTimer);
            optimizerSettings.ServerType = typeof(Server);
            optimizerSettings.RobotSettingsType = typeof(RobotParameters);
            optimizerSettings.RobotType = typeof(SingleSymbolMaCrossOverBot);
            optimizerSettings.Parameters = new List<OptimizeParameter>()
            {
                new OptimizeParameter("Fast MA Period", 5, 15, 5),
                new OptimizeParameter("Slow MA Period", 20),
                new OptimizeParameter("Volume", 1)
            };

            var optimizer = new GridOptimizer(optimizerSettings);

            optimizer.OnOptimizationPassCompletionEvent += Optimizer_OnOptimizationPassCompletionEvent;
            optimizer.OnOptimizationStoppedEvent += Optimizer_OnOptimizationStoppedEvent;
            optimizer.OnOptimizationStartedEvent += Optimizer_OnOptimizationStartedEvent;

            optimizer.Start();
        }
```

## Live Trading

NetTrade manages trading with its "TradeEngine" class. By default it contains a "BacktestTradeEngine" which is used by backtesters and optimizers. You can build or modify the default BacktestTradeEngine based on your needs.

To live trade a NetTrade robot you have to create an implementation of the TradeEngine that connects to a specific broker API and sends the robot orders to the broker. For now NetTrade doesn't contain any live trading "TradeEngine" and we need your contribution to add more trading engines on NetTrade. 

For detailed documentation please check project Wiki.