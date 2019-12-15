using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;
using NetTrade.Enums;
using System.Globalization;
using NetTrade.Implementations;

namespace NetTrade.Models
{
    public class RobotSettings
    {
        public Mode Mode { get; set; }

        public ISymbol MainSymbol { get; set; }

        public List<ISymbol> OtherSymbols { get; set; }

        public IBacktester Backtester { get; set; } = new Backtester();
    }
}
