using NetTrade.Enums;
using System.Collections.Generic;

namespace NetTrade.Interfaces
{
    public interface IRobotSettings
    {
        Mode Mode { get; set; }

        ISymbol MainSymbol { get; set; }

        List<ISymbol> OtherSymbols { get; set; }

        IBacktester Backtester { get; set; }

        IBacktestSettings BacktestSettings { get; set; }

        IAccount Account { get; set; }

        ITradeEngine TradeEngine { get; set; }

        IServer Server { get; set; }
    }
}