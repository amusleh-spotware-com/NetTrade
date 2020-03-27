<h1>Robot</h1>

NetTrade has a main abstract "Robot" class, this class represents a trading system and/or algorithm; you can backtest or optimize any class that is driven from the "Robot" class or the "IRobot" interface with NetTrade.

A robot can be as simple as using a few technical analysis indicators to get trading decisions, or as complicated as using ML models to get trading decisions, an external service or an API.

All you need to do is create a class that's driven from the "Robot" abstract class or for more customization from the "IRobot" interface, give it a name, add some robot parameters, implement the interface methods, and you will have a trading robot.

A simple robot example:


```c#
    [Robot(Name = "Simple Bot", Group = "Sample")]
    public class SimpleBot : Robot
    {
        [Parameter("Volume", DefaultValue = 1)]
        public double Volume { get; set; }

        public override void OnStart()
        {
        }

        public override void OnBar(ISymbol symbol, int index)
        {
            if (symbol.Bars.Close[index] > symbol.Bars.Open[index])
            {
                ClosePositions(TradeType.Sell);

                var buyOrderParameters = new MarketOrderParameters(symbol)
                {
                    Volume = Volume,
                    TradeType = TradeType.Buy,
                };

                Trade.Execute(buyOrderParameters);
            }
            else if (symbol.Bars.Close[index] < symbol.Bars.Open[index])
            {
                ClosePositions(TradeType.Buy);

                var sellOrderParameters = new MarketOrderParameters(symbol)
                {
                    Volume = Volume,
                    TradeType = TradeType.Sell,
                };

                Trade.Execute(sellOrderParameters);
            }
        }

        private void ClosePositions(TradeType tradeType)
        {
            foreach (var order in Trade.Orders)
            {
                if (order.OrderType != OrderType.Market || order.TradeType != tradeType)
                {
                    continue;
                }

                Trade.CloseMarketOrder(order as MarketOrder);
            }
        }
    }
```

The above robot opens a buy market order whenever there is an up/bullish bar, and a sell order whenever there is a down/bearish bar. The robot closes all positions of opposite direction before opening a new one.

As you can see in the robot code we have a "Volume" parameter; this parameter value will be used for setting the volume of the orders, and the NetTrade optimizer can optimize the value of this parameter or of any other robot parameter.

Now let's start from the first line of the robot code which is "RobotAttribute". There we set a name for the robot and for a group. This data can be used by your app to display a user friendly name and to categorize your robots.

After that we have a robot parameter. A robot can contain as many parameters as is needed. These parameters are recognized by the NetTrade optimizer and you will be able to optimize the values for the robot parameters. You can set the min/max/step and the default values for numeric parameters.

The parameter type can be any .Net object, but the NetTrade default "GridOptimizer" can only optimize numeric (int, long, double), enum, string, TimeSpan, DateTime, and DateTimeOffset types.

In the case where you would want to optimize a custom type parameter, you will have to create your own optimizer based on your requirements.

The "OnStart" robot method is its initialization method. You have to input some form of an initialization code, and it will be called once when the robot "Start" method is called.

The "OnBar" method is called whenever a new OHLC bar opens for that specific symbol. You will get the symbol and the bar index from the method parameters. To access the bar data you can use "symbol.Bars.Open/High/Low/Close/Volume[index]".

To start a robot, you can call its "StartAsync" method. There are other methods such as the Stop, Pause, and Resume methods that allow you to manage the running mode of a robot.

You have to pass an IRobotParameters object to StartAsync; this object will have all the necessary data that is needed for the robot to operate. You can find more about this on its own page. 

## Implementable Robot General Methods

OnStart: This method is called when you call the robot "StartAsync" method.

OnTick: This method is called on each new upcoming tick.

OnTimer: This method is called every time the robot timer interval elapses. The robot class has a timer which you can start using the OnStart method and you can use a "TimeSpane" to set its interval value. Don't use the conventional .Net timers because it will not work on backtest/optimizer.

OnPause: This method is called when you call the robot "Pause" method.

OnResume: This method is called when you call the robot "Resume" method. You should only call a robot "Resume" method if it's in "Paused" mode otherwise you will get an "InvalidOperationException" exception.

OnStop: This method is called when you call the robot "Stop" method. You should only call a robot "Stop" method if it's in "Running" or "Paused" modes otherwise you will get an "InvalidOperationException" exception.

## Backtester Event Handler Methods

If you start a robot in backtesting/optimization mode, the robot will use its backtester to get symbol data, and each backtester has several event handler methods that you can override to get data about the backtest operation status.

Backtester_OnBacktestStartEvent: This method is called when the backtest operation starts.

Backtester_OnBacktestPauseEvent: This method is called when the backtest operation is paused.

Backtester_OnBacktestStopEvent: This method is called when the backtest operation stops or is finished.

## Account Event Handler Methods

For a trading account there are some methods that you can override.

Account_OnMarginCallEvent: This method is called whenever you get a margin called.

Robot Properties

Trade: This property allows you to access the trade engine for the purpose of executing orders.

RunningMode: The current running robot mode.

Account: The trading account.

Symbols: The symbols for the robot; the robot will receive only this symbol data.

Mode: This is either a back test for backtesting and optimization, or live for live trading.

Backtester: If you are backtesting/optimizing a robot you can access the back tester with this property.

BacktestSettings: If you are backtesting/optimizing a robot you can access the backtest setting with this property.

Server: The server object that is connected to the robot. It's used to get the current time or some other server-related data.

Timer: Robot timer; with this you can start/stop/configure the timer.

