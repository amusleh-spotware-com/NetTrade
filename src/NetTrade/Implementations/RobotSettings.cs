using NetTrade.Enums;
using NetTrade.Interfaces;
using System.Collections.Generic;

namespace NetTrade.Implementations
{
    public class RobotSettings : IRobotSettings
    {
        public RobotSettings(Mode mode, ISymbol mainSymbol, List<ISymbol> otherSymbols, IBacktester backtester, IAccount account)
        {
            Mode = mode;

            MainSymbol = mainSymbol;

            OtherSymbols = otherSymbols;

            Backtester = backtester;

            Account = account;
        }

        public Mode Mode { get; }

        public ISymbol MainSymbol { get; }

        public List<ISymbol> OtherSymbols { get; }

        public IBacktester Backtester { get; }

        public IAccount Account { get; }
    }
}