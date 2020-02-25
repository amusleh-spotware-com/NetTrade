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
            var changesCopy = new List<IAccountChange>();

            double maxDrawDown = 0;

            foreach (var change in changes)
            {
                changesCopy.Add(change);

                var peakChange = GetPeakChange(changesCopy);

                var troughChange = GetTroughChange(changesCopy, peakChange);

                double currentDrawDown  = 0;

                if (troughChange != null)
                {
                    currentDrawDown = (troughChange.NewValue - peakChange.NewValue) / peakChange.NewValue * 100;
                }

                if (currentDrawDown < maxDrawDown)
                {
                    maxDrawDown = currentDrawDown;
                }
            }

            return maxDrawDown;
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