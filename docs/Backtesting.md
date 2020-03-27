<h1>Backtesting</h1>

One of the main features of NetTrade is that it allows you to easily backtest a NetTrade robot. In order to backtest a NetTrade robot you have to create a backtester or use one of the available default backtesters in NetTrade.

To create a backtester you have to implement the "IBacktester" interface or the "Backtester" abstract class.

NetTrade comes with a default backtester called "OhlcBacktester"; this backtester allows you to backtest a robot on OHLC bars data. It's a very simple backtester that feeds each symbol bar to your robot.

OhlcBacktester code:

```c#
    public class OhlcBacktester : Backtester
    {
        protected override async Task StartDataFeed()
        {
            var startTime = Settings.StartTime;
            var endTime = Settings.EndTime;
            var interval = Interval;

            for (var currentTime = startTime; currentTime <= endTime; currentTime = currentTime.Add(interval))
            {
                bool shouldContinueDataFeed = await ShouldContinueDataFeed();

                if (!shouldContinueDataFeed)
                {
                    break;
                }

                Robot.SetTimeByBacktester(this, currentTime);

                foreach (var symbolData in SymbolsData)
                {
                    var bar = symbolData.GetBar(currentTime);

                    if (bar != null)
                    {
                        (symbolData.Symbol as OhlcSymbol).PublishBar(bar);
                    }
                }

                InvokeOnBacktestProgressChangedEvent(currentTime);
            }

            InvokeOnBacktestStopEvent();
        }
    }
```

Usage:

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

            await robot.StartAsync(robotParmeters); // As the robot mode is set to "Backtest", when you start the robot it starts the backtester
        }
```

As you can see you have to provide IBacktestSettings to the backtester. This object will contain some data that is needed for the backtester to operate, for example the start and end times.

There is a simple default implementation for this at "NetTrade.Models" called "BacktestSettings".

To create your own backtester you should use a "Backtester" abstract class instead of the "IBacktester" interface. You should only use the "IBacktester" interface if you wish to make some major changes.

## IBacktester Properties

Robot: The robot that is currenly under backtest.

Interval: The time interval for the backtester; this is the amount of time taken between each backtester loop iteration.

Settings: The current backtester settings.

SymbolsData: The symbols data for the backtester.

IBacktester Events

OnBacktestStartEvent: This event is triggered after you call the backtester StartAsync method.

OnBacktestPauseEvent: This event is triggered when you pause the backtester.

OnBacktestStopEvent: This event is triggered when you stop the backtester.

OnBacktestProgressChangedEvent: This event is triggered after each loop iteration, based on the defined time interval.

## IBacktester Methods

StartAsync: To start the backtester you have to call this method. It requires three parameters: the first one is the robot that you want to backtest, the second one is the backtest settings object, and the third one is the symbols data.

GetResult: Once the backtest is finished you can call this method to get the backtest statistics.