using NetTrade.Interfaces;

namespace NetTrade.Helpers
{
    public delegate void OnBarHandler(object sender, int index);

    public delegate void OnTickHandler(object sender);

    public delegate void OnBacktestProgressChangedHandler(object sender, double progress);

    public delegate void OnBacktestStartHandler(object sender, IRobot robot);

    public delegate void OnBacktestStopHandler(object sender, IRobot robot);

    public delegate void OnBacktestPauseHandler(object sender, IRobot robot);
}