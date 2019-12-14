using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Interfaces
{
    public interface IRobot
    {
        ISymbol Symbol { get; }

        List<ISymbol> OtherSymbols { get; }

        bool IsRunning { get; }

        void Start();

        void Stop();

        void Pause();

        void OnStart();

        void OnBar();

        void OnStop();
    }
}
