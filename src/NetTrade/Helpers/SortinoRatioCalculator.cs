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
                var negativeReturns = data.Where(iData => iData < 0);

                if (negativeReturns.Any())
                {
                    var negativeReturnsStd = StdCalculator.GetStd(negativeReturns);

                    if (negativeReturnsStd != 0)
                    {
                        result = data.Sum() / negativeReturnsStd;
                    }
                }
            }

            return result;
        }
    }
}