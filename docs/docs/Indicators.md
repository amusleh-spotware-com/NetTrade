<h1>Indicators</h1>

Technical indicators are very important for algo trading, and NetTrade allows you to develop indicators and use them on your NetTrade robots. It comes with several built-in indicators that you can use to learn about how to develop an indicator based on the functions provided by NetTrade.

You can find the built-in indicators inside the NetTrade indicators directory. Below is an example of a simple moving average indicator:

```c#
    public class SimpleMovingAverage : Indicator
    {
        private readonly ExpandableSeries<double> _data = new ExpandableSeries<double>();

        public SimpleMovingAverage(ISymbol symbol)
        {
            Symbol = symbol;

            Symbol.IndicatorOnBarEvent += Symbol_OnBarEvent;
        }

        public ISymbol Symbol { get; }

        public int Periods { get; set; } = 14;

        public DataSourceType DataSourceType { get; set; } = DataSourceType.Close;

        public ISeries<double> Data => _data;

        private void Symbol_OnBarEvent(object sender, int index)
        {
            var symbolData = Symbol.Bars.GetData(DataSourceType);

            var dataPoint = double.NaN;

            if (symbolData.Count >= Periods)
            {
                var dataWindow = symbolData.Skip(symbolData.Count - Periods);

                dataPoint = dataWindow.Sum() / dataWindow.Count();
            }

            _data.Add(dataPoint);

            OnNewValue(index);
        }
    }

```

As you can see all the indicators must be implemented in the "IIndicator" interface or in the "Indicator" abstract class.

You can use one indicator inside another indicator; "Average True Range" is a good example:

```c#
    public class AverageTrueRange : Indicator
    {
        private readonly TrueRange _trueRange;

        private readonly ExpandableSeries<double> _data = new ExpandableSeries<double>();

        public AverageTrueRange(ISymbol symbol)
        {
            Symbol = symbol;

            _trueRange = new TrueRange(Symbol);

            _trueRange.OnNewValueEvent += _trueRange_OnNewValueEvent;
        }

        public int Periods { get; set; } = 14;

        public ISymbol Symbol { get; }

        public ISeries<double> Data => _data;

        private void _trueRange_OnNewValueEvent(object sender, int index)
        {
            var dataPoint = double.NaN;

            if (index + 1 >= Periods)
            {
                var dataWindow = _trueRange.Data.Skip(_trueRange.Data.Count - Periods);

                dataPoint = dataWindow.Sum() / dataWindow.Count();
            }

            _data.Add(dataPoint);

            OnNewValue(index);
        }
    }

```

Inside "Average True Range" we have used the "True Range" indicator.