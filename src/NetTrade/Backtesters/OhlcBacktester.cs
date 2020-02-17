using NetTrade.Abstractions;
using NetTrade.Symbols;
using System.Threading.Tasks;

namespace NetTrade.Backtesters
{
    public class OhlcBacktester : Backtester
    {
        protected override async Task StartDataFeed()
        {
            for (var currentTime = Settings.StartTime; currentTime <= Settings.EndTime; currentTime = currentTime.Add(Interval))
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