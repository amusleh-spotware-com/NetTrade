using NetTrade.Abstractions.Interfaces;
using System.Collections.Generic;

namespace NetTrade.Helpers
{
    public static class MaxDrawdownCalculator
    {
        public static double GetMaxDrawdown(IEnumerable<IAccountChange> changes)
        {
            IAccountChange peakChange = null;

            double maxDrop = double.MinValue, maxDrawdown = double.MaxValue;

            foreach (var change in changes)
            {
                if (peakChange == null)
                {
                    peakChange = change;

                    continue;
                }

                var diff = peakChange.NewValue - change.NewValue;

                peakChange = diff < 0 ? change : peakChange;

                maxDrop = maxDrop > diff ? maxDrop : diff;

                var newDrawdown = maxDrop / peakChange.NewValue * -100;

                maxDrawdown = newDrawdown < maxDrawdown ? newDrawdown : maxDrawdown;
            }

            return maxDrawdown < 0 ? maxDrawdown : 0;
        }
    }
}