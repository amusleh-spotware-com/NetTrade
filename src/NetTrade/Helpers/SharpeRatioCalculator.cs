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
                var positiveData = data.Where(iData => iData > 0);

                if (positiveData.Any())
                {
                    var dataStd = StdCalculator.GetStd(data);

                    if (dataStd != 0)
                    {
                        result = (positiveData.Sum() / data.Count()) / dataStd;
                    }
                }
            }

            return result;
        }
    }
}
