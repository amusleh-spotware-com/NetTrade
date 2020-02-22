using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Helpers
{
    public static class SortinoRatioCalculator
    {
        public static double GetSortinoRatio(double initialDeposit, IEnumerable<double> data)
        {
            if (!data.Any())
            {
                return 0;
            }

            var returns = data.Select(iDataPoint => iDataPoint / initialDeposit * 100).ToList();

            var negativeReturns = returns.Where(iReturn => iReturn < 0);

            if (negativeReturns.Any())
            {
                var negativeReturnsStd = negativeReturns.Select(iReturn => Math.Sqrt(Math.Pow(iReturn, 2))).Average();

                return returns.Average() / negativeReturnsStd;
            }
            else
            {
                return 0;
            }
        }
    }
}