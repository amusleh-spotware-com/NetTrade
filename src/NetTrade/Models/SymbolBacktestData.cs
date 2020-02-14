using NetTrade.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Models
{
    public class SymbolBacktestData : ISymbolBacktestData
    {
        private readonly IEnumerable<IBar> _data;

        public SymbolBacktestData(ISymbol symbol, IEnumerable<IBar> data)
        {
            Symbol = symbol;

            _data = data.OrderBy(iBar => iBar.Time).ToList();
        }

        public ISymbol Symbol { get; }

        public IBar GetBar(DateTimeOffset time) => _data.FirstOrDefault(iBar => iBar.Time == time);

        public IBar GetNearestBar(DateTimeOffset time)
        {
            var minDistance = double.MaxValue;

            IBar result = null;

            foreach (var bar in _data)
            {
                var distance = Math.Abs((bar.Time - time).TotalHours);

                if (distance < minDistance)
                {
                    minDistance = distance;

                    result = bar;

                    if (distance == 0)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        public object Clone() => new SymbolBacktestData(Symbol.Clone() as ISymbol, _data.ToList());
    }
}