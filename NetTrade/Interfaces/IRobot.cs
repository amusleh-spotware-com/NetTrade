using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Interfaces
{
    public interface IRobot
    {
        void OnStart();

        void OnBar();

        void OnStop();
    }
}
