<h1>TradeEngine</h1>

A trade engine is the object that will be used by the robot to execute orders. A trade engine can be created to simulate the market like the NetTrade default "BacktestTradeEngine", or it can be connected to a brokerage API to execute your orders on the real market.

NetTrade allows you to easily switch between simulation and the real market by using its "ITradeEngine" interface. You just have to change the trade engine from simulation to a real market trade engine and everything will be managed by NetTrade itself.

For backtesting or optimization you can use NetTrade "BacktestTradeEngine", but for live trading you will have create your own trade engine class either by implementing the "ITradeEngine" interface or a "TradeEngine" abstract class.

If the "BacktestTradeEngine" does not meet your requirements you can create your own simulation trade engine or extend it.

## ITradeEngine Properties

Orders: This collection contains all of your open orders.

Trades: This collection contains your historical trades, or closed market orders.

Journal: This collection contains all of your trading activities.

Server: This is the "IServer" object that is used on your Robot; it must be the same object.

Account: This is the trading account that is connected to your Robot.

## ITradeEngine Methods

Execute: With this method you can execute an order. You have to pass an "IOrderParameters" object; you can use the default implementations such as "MarketOrderParameters" and "PendingOrderParameters", or you can create your own implementations. It returns a TradeResult object which you can use to check if your order has been successfully executed or not.

UpdateSymbolOrders: Whenever a symbol price changes this method must be called. The trade engine will update the symbol open orders data.

CloseMarketOrder:  This closes a market order. It has two overrides, the default close reason is manual, while the other one allows you to specify the close reason.

CloseAllMarketOrders: This method closes all open market orders. It has 4 overrides which allow you to set the orders trade type and the close reason.

CancelPendingOrder: Cancels a pending order.

When you are creating a trade engine all of the above methods and properties must work properly.
