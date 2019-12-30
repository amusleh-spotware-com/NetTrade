using System.Collections.Generic;
using System;

namespace NetTrade.Interfaces
{
    public interface IOptimizerSettings
    {
        ISymbol MainSymbol { get; }

        List<ISymbol> OtherSymbols { get; }

        Type RobotType { get; }

        Type RobotSettingsType { get; }

        Type BacktesterType { get; }

        object[] BacktesterParameters { get; }

        IBacktestSettings BacktestSettings { get; }

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