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
                var negativeData = data.Where(iData => iData < 0);

                if (negativeData.Any())
                {
                    var dataStd = StdCalculator.GetStd(data);

                    if (dataStd != 0)
                    {
                        result = (negativeData.Sum() / data.Count()) / dataStd;
                    }
                }
            }

            return result;
        }
    }
}