using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Abstractions
{
    public interface IRobot
    {
        IBars Bars { get; }

        void OnStart();

        void OnBar();

        void OnStop();
    }
}
