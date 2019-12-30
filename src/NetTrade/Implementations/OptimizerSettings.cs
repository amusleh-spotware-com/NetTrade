using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;

namespace NetTrade.Implementations
{
    public class OptimizerSettings : IOptimizerSettings
    {
        public ISymbol MainSymbol { get; set; }

        public List<ISymbol> OtherSymbols { get; set; }

        public Type BacktesterType { get; set; }

        public object[] BacktesterParameters { get; set; }

        public IBacktestSettings BacktestSettings { get; set; }

        public IReadOnlyList<IOptimizeParameter> Parameters { get; set; }

        public Type TradeEngineType { get; set; }

        public object[] TradeEngineParameters { get; set; }

        public long AccountBalance { get; set; }

        public long AccountLeverage { get; set; }

        public Type RobotType { get; set; }

        public Type RobotSettingsType { get; set; }

        public Type ServerType { get; set; }

        public object[] ServerParameters { get; set; }

        public int MaxProcessorNumber { get; set; } = -1;
    }
}
