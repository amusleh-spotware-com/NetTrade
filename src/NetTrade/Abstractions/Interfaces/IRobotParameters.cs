using NetTrade.Enums;
using System.Collections.Generic;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IRobotParameters
    {
        Mode Mode { get; set; }

        IEnumerable<ISymbol> Symbols { get; set; }

        IBacktester Backtester { get; set; }

        IBacktestSettings BacktestSettings { get; set; }

        IAccount Account { get; set; }

        ITradeEngine TradeEngine { get; set; }

        IServer Server { get; set; }

        ITimer Timer { get; set; }

        IEnumerable<ISymbolBacktestData> SymbolsBacktestData { get; set; }
    }
}