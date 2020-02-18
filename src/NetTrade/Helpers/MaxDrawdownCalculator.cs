using NetTrade.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Helpers
{
    public static class MaxDrawdownCalculator
    {
        public static double GetMaxDrawdown(IEnumerable<IAccountChange> changes)
        {
            var changesCopy = changes.ToList();

            var peakChange = GetPeakChange(changesCopy);

            var troughChange = GetTroughChange(changesCopy, peakChange);

            double result = 0;

            if (troughChange != null)
            {
                result = (troughChange.NewValue - peakChange.NewValue) / peakChange.NewValue;
            }

            return result > 0 ? 0 : Math.Round(result, 3) * 100;
        }

        private static IAccountChange GetTroughChange(IEnumerable<IAccountChange> changes, IAccountChange peakChange)
        {
            var troughValue = double.MaxValue;

            IAccountChange troughChange = null;

            var changesOrdered = changes.Where(iChange => iChange.Time > peakChange.Time).OrderBy(iChange => iChange.Time);

            foreach (var change in changesOrdered)
            {
                if (change.NewValue >= troughValue)
                {
                    continue;
                }

                troughValue = change.NewValue;

                troughChange = change;
            }

            return troughChange;
        }

        private static IAccountChange GetPeakChange(IEnumerable<IAccountChange> changes)
        {
            var peakValue = double.MinValue;

            IAccountChange result = null;

            var changesOrdered = changes.OrderBy(iChange => iChange.Time);

            foreach (var change in changesOrdered)
            {
                if (change.NewValue < peakValue)
                {
                    continue;
                }

                peakValue = change.NewValue;

                result = change;
            }

            return result;
        }
    }
}