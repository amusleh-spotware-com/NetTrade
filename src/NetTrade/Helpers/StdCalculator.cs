using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Helpers
{
    public static class StdCalculator
    {
        public static double GetStd(IEnumerable<double> data)
        {
            var average = data.Average();

            var deviationsSum = data.Select(iValue => Math.Pow(iValue - average, 2)).Sum();

            var count = data.Count() > 1 ? (data.Count() - 1) : 1;

            var variance = deviationsSum / count;

            return Math.Sqrt(variance);
        }
    }
}