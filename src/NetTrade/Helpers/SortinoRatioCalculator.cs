using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Helpers
{
    public static class SortinoRatioCalculator
    {
        public static double GetSortinoRatio(IEnumerable<double> data)
        {
            double result = double.NaN;

            if (data.Any())
            {
                var returns = data.Take(data.Count() - 1).Zip(data.Skip(1), (oldValue, newValue) => (newValue - oldValue) / oldValue * 100);

                var negativeReturns = returns.Where(iReturn => iReturn < 0);

                if (negativeReturns.Any())
                {
                    var negativeReturnsStd = StdCalculator.GetStd(negativeReturns);

                    result = returns.Average() / negativeReturnsStd;
                }
            }

            return result;
        }
    }
}