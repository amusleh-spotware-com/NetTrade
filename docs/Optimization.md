<h1>Optimization</h1>

NetTrade allows you to optimize your robot parameters. The way this works is that when you define the parameters for your robot the optimizer discovers these parameters and allows you to set the minimum/maximum/step values for each parameter. The optimizer will then run a backtest for all the possible combinations of the parameter values. You will receive each optimization pass after completion by subscribing to the optimizer "OnOptimizationPassCompletionEvent".

The backtester NetTrade has a default optimizer called "GridOptimizer". This optimizer runs a backtest for all the possible combinations of the defined parameter values of your robot. In a case where you would want to use any other type of an optimization algorithm, you have to create your own optimizer. The optimizer classes must implement either the abstract "Optimizer" class or the "IOptimizer" interface.

The optimizer only allows you to optimize a specific number of parameters, not all of them.

Below is a code example of using the default NetTrade "GridOptimizer" to optimize "SingleSymbolMaCrossOverBot" moving average period parameters:

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

As you can see the optimizer requires an "IOptimizerSettings" object. This object will inform the optimizer which robot to optimize, which parameters and which backtester to use, as well as all the other necessary data that is required by the optimizer to operate.

There is a default implementation of the "IOptimizerSettings" interface called "OptimizerSettings" in the NetTrade models. You can use this implementation for "GridOptimizer" as can be seen in the code example, but if you create any other custom optimizers you can either use these or create a new implementation of "IOptimizerSettings" based on your optimizer needs.

## IOptimizerSettings Properties

SymbolsData: The symbols data that will be used for optimization. As in backtesting you have to provide the symbols data.

RobotType: The type of robot that you want to optimize.

RobotSettingsType: This is the "IRobotParameters" class type that will be used to start your robot.

RobotSettingsParameters: The default "IRobotParameters" constructor parameters. If the constructor does not require any parameters you can ignore this property.

BacktesterType: The type of backtester that will be used for optimization.

BacktesterParameters: The default "Backtester" constructor parameters. If the constructor does not require any parameters you can ignore this property.

BacktestSettingsType: The type of "IBacktestSettings" that will be used for starting the backtester.

BacktestSettingsParameters: The default "IBacktestSettings" constructor parameters. If the constructor does not require any parameters you can ignore this property. The default "BacktetsSettings" class requires a start and an end time of the backtest.

BacktesterInterval: The backtester interval.

TradeEngineType: The type of trade engine that will be used.

TradeEngineParameters: The trade engine constructor parameters. If there are no parameters then you can ignore this property.

AccountBalance: The initial trading account balance.

AccountLeverage: The trading account leverage.

Parameters: The robot parameters that you can optimize. For each parameter you have to create an "IOptimizeParameter". The min/max values of "IOptimizeParameter" must either match or be lower than the parameters attributed to the min/max values.

ServerType: The type of "IServer" class that will be used for the robot.

ServerParameters: The "IServer" class constructor parameters, if there are any.

TimerType: The robot "ITimer" class type.

TimerParameters: The "ITimer" class constructor parameters, if there are any.

MaxProcessorNumber: The maximum number of CPU processors that will be used for optimization. If you don't set any value then all processors will be used.

Once you create an "IOptimizerSettings" object, all you have to do is pass it to the optimizer constructor after which you can start your optimization.

Before starting your optimization be sure to assign a handler for the optimizer "OnOptimizationPassCompletionEvent" event. After each finished backtest or pass that event will be triggered and you will be able to handle it and check the backtest statistics.

## OptimizeParameter

This is the NetTrade default implementation for the "IOptimizeParameter" interface. It has several constructors which allow you to use it based on your parameter types.

It has three numeric constructors for "int, long, and double" parameter types, a constructor for "DateTimeOffset" parameters, a constructor for "TimeSpan" parameters, and another constructor for parameters that cannot fit all the previous constructs. For the last constructor you have to provide the parameter values manually.

If the default "OptimizeParameter" class does not fulfill your requirements, you can always create your own implementation for the "IOptimizeParameter" interface.