using System;
using System.Collections.Generic;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IOptimizerSettings
    {
        IEnumerable<ISymbolBacktestData> SymbolsData { get; }

        Type RobotType { get; }

        Type RobotSettingsType { get; }

        object[] RobotSettingsParameters { get; }

        Type BacktesterType { get; }

        object[] BacktesterParameters { get; }

        Type BacktestSettingsType { get; }

        object[] BacktestSettingsParameters { get; }

        TimeSpan BacktesterInterval { get; }

        Type TradeEngineType { get; }

        object[] TradeEngineParameters { get; }

        long AccountBalance { get; }

        long AccountLeverage { get; }

        IReadOnlyList<IOptimizeParameter> Parameters { get; }

        Type ServerType { get; }

        object[] ServerParameters { get; }

        Type TimerType { get; }

        object[] TimerParameters { get; }

        int MaxProcessorNumber { get; }
    }
}