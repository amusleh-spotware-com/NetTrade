using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NetTrade.Helpers
{
    public static class SharpeRatioCalculator
    {
        public static double GetSharpeRatio(double initialDeposit, IEnumerable<double> data)
        {
            if (!data.Any())
            {
                return 0;
            }

            var returns = data.Select(iDataPoint => iDataPoint / initialDeposit * 100).ToList();

            var variance = returns.Select(iValue => Math.Pow(iValue - returns.Average(), 2)).Sum() / returns.Count > 1 ? (returns.Count - 1) : 1;

            var standardDeviation = Math.Sqrt(variance);

            return returns.Sum() / standardDeviation;
        }
    }
}
