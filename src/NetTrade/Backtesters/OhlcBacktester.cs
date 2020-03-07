using NetTrade.Abstractions;
using NetTrade.Symbols;
using System.Threading.Tasks;

namespace NetTrade.Backtesters
{
    public class OhlcBacktester : Backtester
    {
        protected override async Task StartDataFeed()
        {
            var startTime = Settings.StartTime;
            var endTime = Settings.EndTime;
            var interval = Interval;

            for (var currentTime = startTime; currentTime <= endTime; currentTime = currentTime.Add(interval))
            {
                bool shouldContinueDataFeed = await ShouldContinueDataFeed();

                if (!shouldContinueDataFeed)
                {
                    break;
                }

                Robot.SetTimeByBacktester(this, currentTime);

                foreach (var symbolData in SymbolsData)
                {
                    var bar = symbolData.GetBar(currentTime);

                    if (bar != null)
                    {
                        (symbolData.Symbol as OhlcSymbol).PublishBar(bar);
                    }
                }

                InvokeOnBacktestProgressChangedEvent(currentTime);
            }

            InvokeOnBacktestStopEvent();
        }
    }
}