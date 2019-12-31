using System;
using System.Collections.Generic;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IOptimizerSettings
    {
        ISymbol MainSymbol { get; }

        List<ISymbol> OtherSymbols { get; }

        Type RobotType { get; }

        Type RobotSettingsType { get; }

        Type BacktesterType { get; }

        object[] BacktesterParameters { get; }

        Type BacktestSettingsType { get; }

        object[] BacktestSettingsParameters { get; }

        Type TradeEngineType { get; }

        object[] TradeEngineParameters { get; }

        long AccountBalance { get; }

        long AccountLeverage { get; }

        IReadOnlyList<IOptimizeParameter> Parameters { get; }

        Type ServerType { get; }

        object[] ServerParameters { get; }

        int MaxProcessorNumber { get; }
    }
}