using System;
using System.Collections.Generic;
using NetTrade.Interfaces;
using System.Linq;

namespace NetTrade.Abstractions
{
    public abstract class Robot : IRobot
    {
        public Robot(ISymbol symbol, IEnumerable<ISymbol> otherSymbols)
        {
            Symbol = symbol;

            OtherSymbols = otherSymbols.ToList();
        }

        public ISymbol Symbol { get; }

        public List<ISymbol> OtherSymbols { get; }

        public bool IsRunning { get; private set; }

        public void Start()
        {
            IsRunning = true;

            OnStart();
        }

        public void Stop()
        {
            IsRunning = false;

            OnStop();
        }

        public void Pause()
        {
            IsRunning = false;
        }

        public abstract void OnBar();

        public abstract void OnStart();

        public abstract void OnStop();
    }
}
