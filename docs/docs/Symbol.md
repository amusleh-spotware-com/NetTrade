<h1>Symbol</h1>

A symbol is an asset that you can trade in NetTrade. Each symbol can have different settings based on the real asset. NetTrade came with a default "OhlcSymbol" implementation of the "ISymbol" interface, but you can also create your own implementation and use it instead, according to your requirements.

## ISymbol Properties

Name: A symbol code name, for example EURUSD or AAPL.

TickSize: The tick size of a symbol in its price term. For EURUSD it's 0.00001 or for Stocks it's one cent or 0.01.

TickValue: The monetary value of one symbol tick movement, or how much it will cost if you trade one unit of symbol volume, as seen by the price moving by one tick.

Commission: The amount of commission your broker charges you upon opening or closing a position.

Digits: The number of decimal digit points in the symbol price. For EURUSD it's 5 or for Stocks it's 2.

MinVolume: The minimum volume that your broker allows you to trade for the symbol, for EURUSD it's 1000 unit (0.01 lots).

MaxVolume: The maximum volume that your broker allows you to trade for the symbol.

VolumeStep: The step or interval that your broker allows you to increase or decrease in terms of volume.

VolumeUnitValue: One volume unit of monetary vlaue.

Bid: The latest bid price of the symbol.

Ask: The lastest ask price of the symbol.

Bars: The price data of the symbol OHLC.

Slippage: The slippage symbol; this will only be used for the backtest.

Spread: The current spread of the symbol.

## ISymbol Events

RobotOnTickEvent: This event is used by the robots. It is triggered whenever the symbol price changes by one tick.

IndicatorOnTickEvent: Similar to the RobotOnTickEvent, but this is used by indicators and it is triggered before the RobotOnTickEvent.

RobotOnBarEvent: This event is used by the robots. It is triggered whenever a new OHLC bar opens.

IndicatorOnBarEvent: Similar to the RobotOnBarEvent, but this is used by indicators and it is triggered before the RobotOnBarEvent.

## ISymbol Methods

GetPrice: You can use this method to get the price of a symbol based on the trade type. For buy orders it returns the ask price and for sell orders it returns the bid price.

## OhlcSymbol Methods

PublishBar: This method is used to publish a new OHLC price bar for the symbol. It adds the bar to the symbol Bars data collection and triggers the symbol OnTick and OnBar events.

Symbol objects must be cloneable.