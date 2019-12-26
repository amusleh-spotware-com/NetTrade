using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Interfaces
{
    public interface IOptimizeParameter
    {
        string Name { get; }

        object MinValue { get; }

        object MaxValue { get; }

        object Step { get; }
    }
}
