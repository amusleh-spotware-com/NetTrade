using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Helpers
{
    public static class StdCalculator
    {
        public static double GetStd(IEnumerable<double> data, bool isSample)
        {
            var average = data.Average();

            var deviationsSum = data.Select(iValue => Math.Pow(iValue - average, 2)).Sum();

            var dataCount = data.Count();

            var count = isSample && dataCount > 1 ? (dataCount - 1) : dataCount;

            var variance = deviationsSum / count;

            return Math.Sqrt(variance);
        }
    }
}