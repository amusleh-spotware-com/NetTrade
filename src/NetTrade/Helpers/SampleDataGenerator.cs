using NetTrade.Abstractions.Interfaces;
using NetTrade.Models;
using System;
using System.Collections.Generic;

namespace NetTrade.Helpers
{
    public static class SampleDataGenerator
    {
        public static List<IBar> GetSampleData(double startPrice, DateTimeOffset startTime, DateTimeOffset endTime,
            TimeSpan interval, int randomSeed = 1)
        {
            var random = new Random(randomSeed);

            var result = new List<IBar>();

            double previousBarClose = startPrice;

            for (var iCurrentTime = startTime; iCurrentTime <= endTime; iCurrentTime = iCurrentTime.Add(interval))
            {
                var open = previousBarClose;
                var high = open + random.NextDouble();
                var low = open - random.NextDouble();

                double close;

                if (random.Next(0, 1) > 0)
                {
                    var highBasedClose = high - random.NextDouble();

                    close = highBasedClose < low ? low : highBasedClose;
                }
                else
                {
                    var lowBasedClose = low + random.NextDouble();

                    close = lowBasedClose > high ? high : lowBasedClose;
                }

                long volume = (long)((high - low) * 1000);

                var bar = new Bar(iCurrentTime, open, high, low, close, volume);

                result.Add(bar);

                previousBarClose = close;
            }

            return result;
        }
    }
}