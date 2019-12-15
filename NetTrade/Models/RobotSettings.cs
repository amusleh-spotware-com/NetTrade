using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;
using NetTrade.Enums;
using CsvHelper.Configuration;
using System.Globalization;
using NetTrade.Implementations;

namespace NetTrade.Models
{
    public class RobotSettings
    {
        public RobotSettings()
        {
            CsvConfiguration = new Configuration();

            CsvConfiguration.CultureInfo = CultureInfo.InvariantCulture;

            Backtester = new Backtester();
        }

        public Mode Mode { get; set; }

        public ISymbol MainSymbol { get; set; }

        public List<ISymbol> OtherSymbols { get; set; }

        public Configuration CsvConfiguration { get; }

        public IBacktester Backtester { get; }
    }
}
