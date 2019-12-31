using NetTrade.Enums;
using NetTrade.Interfaces;
using System.Collections.Generic;

namespace NetTrade.Implementations
{
    public class RobotSettings : IRobotSettings
    {
        public Mode Mode { get; set; }

        public ISymbol MainSymbol { get; set; }

        public List<ISymbol> OtherSymbols { get; set; }

        public IBacktester Backtester { get; set; }

        public IBacktestSettings BacktestSettings { get; set; }

        public IAccount Account { get; set; }

        public IServer Server { get; set; }

        public ITradeEngine TradeEngine { get; set; }
    }
}