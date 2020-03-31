<h1>IRobotParameters</h1>

This is the configuration object that is passed to a robot start method. The robot will use this object to know its mode (backtest or live), as well as all the necessary data it needs to operate. Below you can find the descriptions for each property of this object.

## Properties

Mode: You set it to either backtest or live. When you optimize a robot, this property value is set by an optimizer to backtest.

Symbols: The robot symbols; these are symbols that the robot can trade and use to receive their market data.

Backtester: In the case where you set the mode to "Backtest", you have to provide a backtester for the robot.

BacktestSettings: These are the settings that will be used by the Backtester, like the start and end times.

Account: The trading account for the robot.

TradeEngine: This is the trade engine that will be used by the robot. When executing an order inside your robot, you should use this trade engine.

Server: A server object allows you to have access to some general data like the current time.

TimerContainer: This is an "ITimerContainer" object, it will manage your robot timers if it had any, if you don't provdie any value for this property the robot itself will instantiate a "NetTrade.Models.TimerContainer" object and use it instead 

SymbolsBacktestData: The symbols data; for each of the robot symbols you have to provide its backtest data. In live mode this property will not be used and you can ignore it.

For most of the above properties NetTrade has a defualt implementation. You don't have to create your own implementations unless you need to.

There is a default implementation of an 'IRobotParameters' interface called the 'RobotParameters' class in 'NetTrade.Models'. You can use this instead of creating your own implementation, but if your robot needs any kind of custom data then you will have to create your own implementation.

Below is an example of RobotParameters:

```c#
            var startTime = data.Min(iBar => iBar.Time); // Setting backtest start/end time based on symbol available data
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
            };

            robotParmeters.TradeEngine = new BacktestTradeEngine(robotParmeters.Server, robotParmeters.Account);

            robotParmeters.Account.AddTransaction(new Transaction(10000, startTime));

            robotParmeters.Backtester.OnBacktestStopEvent += Backtester_OnBacktestStopEvent;
            robotParmeters.Backtester.OnBacktestProgressChangedEvent += Backtester_OnBacktestProgressChangedEvent;
```