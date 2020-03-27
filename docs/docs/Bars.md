<h1>Bars</h1>

This is the object that allows you to access a symbol OHLC bars data. It's used by the "ISymbol" interface. You have to use the NetTrade "IBars" interface or the "Bars" abstract class to create your own "Bars" class or use the default implementations of "IBars" such as "TimeBasedBars" which are included in NetTrade.

You can use "TimeBasedBars" for normal OHLC data, but for tick-based ones such as Renko, Range, or any other bars type you will have to create your own implementations.

## IBars Properties

Time: Contains the bars open times data.

Open: Contains the bars open prices data.

High: Contains the bars high prices data.

Low: Contains the bars low prices data.

Volume: Contains the bars volume data.

You can access each bar data with its index; you will receive the index via an OnBar event.

## IBars Methods

AddBar: Adds a new bar data to the collections and returns back to the bar index.

GetData: Returns bar-based data on your given "DataSourceType".

TimeBasedBars Properties

TimeFrame: The time frame of bars.