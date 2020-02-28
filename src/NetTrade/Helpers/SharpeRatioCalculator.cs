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
            double result = double.NaN;

            if (data.Any())
            {
                var returns = data.Take(data.Count() - 1).Zip(data.Skip(1), (oldValue, newValue) => (newValue - oldValue) / oldValue * 100);

                if (returns.Any())
                {
                    var returnsStd = StdCalculator.GetStd(returns, true);

                    result = returns.Average() / returnsStd;
                }
            }

            return result;
        }
    }
}
