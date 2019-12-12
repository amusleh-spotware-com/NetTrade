using System;
using NetTrade.Interfaces;

namespace NetTrade.Abstractions
{
    public abstract class Robot : IRobot
    {
        public abstract void OnBar();

        public abstract void OnStart();

        public abstract void OnStop();
    }
}
