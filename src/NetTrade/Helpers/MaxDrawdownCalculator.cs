using NetTrade.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace NetTrade.Helpers
{
    public static class MaxDrawdownCalculator
    {
        public static double GetMaxDrawdown(IEnumerable<IAccountChange> changes)
        {
            var changesCopy = changes.ToList();

            var troughChange = GetTroughChange(changesCopy);

            double result = 0;

            if (troughChange != null)
            {
                var peakValue = changesCopy.Where(iChange => iChange.Time <= troughChange.Time).Max(iChange => iChange.NewValue);

                result = (troughChange.NewValue - peakValue) / peakValue;
            }

            return result > 0 ? 0 : Math.Round(result, 3) * 100;
        }

        private static IAccountChange GetTroughChange(IEnumerable<IAccountChange> changes)
        {
            var troughValue = double.MaxValue;

            IAccountChange troughChange = null;

            var changesOrdered = changes.OrderBy(iChange => iChange.Time);

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
    }
}