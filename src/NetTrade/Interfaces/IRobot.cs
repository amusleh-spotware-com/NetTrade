using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Models;

namespace NetTrade.Interfaces
{
    public interface IRobot
    {
        RobotSettings Settings { get; }

        bool IsRunning { get; }

        void Start();

        void Stop();

        void Pause();

        void OnStart();

        void OnBar(int index);

        void OnStop();
    }
}
