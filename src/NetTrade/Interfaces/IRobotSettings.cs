using NetTrade.Enums;
using System.Collections.Generic;

namespace NetTrade.Interfaces
{
    public interface IRobotSettings
    {
        Mode Mode { get; }

        ISymbol MainSymbol { get; }

        List<ISymbol> OtherSymbols { get; }

        IBacktester Backtester { get; }

        IBacktestSettings BacktestSettings { get; }

        IAccount Account { get; }
    }
}