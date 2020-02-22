using System;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IBacktestResult
    {
        long TotalTradesNumber { get; }

        long LongTradesNumber { get; }

        long ShortTradesNumber { get; }

        double MaxBalanceDrawdown { get; }

        double MaxEquityDrawdown { get; }

        double WinningRate { get; }

        double NetProfit { get; }

        double ProfitFactor { get; }

        double Commission { get; }

        double SharpeRatio { get; }

        double SortinoRatio { get; }

        double AverageProfit { get; }

        double AverageLoss { get; }

        double AverageReturn { get; }

        double VsBuyHoldRatio { get; }

        TimeSpan AverageTradeDuration { get; }
    }
}