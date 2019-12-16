using NetTrade.Interfaces;
using NetTrade.Models;
using System.Collections.Generic;
using System;

namespace NetTrade.Implementations
{
    public class Symbol : ISymbol
    {
        public Symbol(TimeSpan timeFrame)
        {
            Bars = new Bars(timeFrame);
        }
        public List<Bar> Data { get; set; }

        public string Name { get; set; }

        public double TickSize { get; set; }

        public double Commission { get; set; }

        public int Digits { get; set; }

        public long MinVolume { get; set; }

        public long MaxVolume { get; set; }

        public long VolumeStep { get; set; }

        public Bars Bars { get; }
    }
}