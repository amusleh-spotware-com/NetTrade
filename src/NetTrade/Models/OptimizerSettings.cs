using NetTrade.Abstractions.Interfaces;
using System;
using System.Collections.Generic;

namespace NetTrade.Models
{
    public class OptimizerSettings : IOptimizerSettings
    {
        public IEnumerable<ISymbolBacktestData> SymbolsData { get; set; }

        public Type BacktesterType { get; set; }

        public object[] BacktesterParameters { get; set; }

        public Type BacktestSettingsType { get; set; }

        public object[] BacktestSettingsParameters { get; set; }

        public TimeSpan BacktesterInterval { get; set; }

        public IReadOnlyList<IOptimizeParameter> Parameters { get; set; }

        public Type TradeEngineType { get; set; }

        public object[] TradeEngineParameters { get; set; }

        public long AccountBalance { get; set; }

        public long AccountLeverage { get; set; }

        public Type RobotType { get; set; }

        public Type RobotSettingsType { get; set; }

        public object[] RobotSettingsParameters { get; set; }

        public Type ServerType { get; set; }

        public object[] ServerParameters { get; set; }

        public Type TimerType { get; set; }

        public object[] TimerParameters { get; set; }

        public int MaxProcessorNumber { get; set; } = -1;
    }
}