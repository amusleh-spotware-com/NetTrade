using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NetTrade.Helpers
{
    public static class SharpeRatioCalculator
    {
        public static double GetSharpeRatio(IEnumerable<double> data)
        {
            if (!data.Any())
            {
                return double.NaN;
            }

            var standardDeviation = StdCalculator.GetStd(data);

            return data.Sum() / standardDeviation;
        }
    }
}
