using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Models;
using NetTrade.Enums;

namespace NetTrade.Interfaces
{
    public interface IRobot
    {
        IRobotSettings Settings { get; }

        RunningMode RunningMode { get; }

        void Start();

        void Stop();

        void Pause();

        void Resume();

        void OnStart();

        void OnBar(int index);

        void OnStop();
    }
}
