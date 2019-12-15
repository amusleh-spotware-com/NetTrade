using NetTrade.Models;
using NetTrade.Interfaces;

namespace NetTrade.Helpers
{
    public delegate void OnBarHandler(object sender, int index);

    public delegate void OnBacktestStart(object sender, IRobot robot);

    public delegate void OnBacktestStop(object sender, IRobot robot);

    public delegate void OnBacktestPause(object sender, IRobot robot);

    public delegate void OnBacktestFinished(object sender, IBacktestResult result);
}