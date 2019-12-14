using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;

namespace NetTrade.Implementations
{
    public class Symbol : ISymbol
    {
        public Symbol()
        {
            Bars = new Bars();
        }

        public string DataFilePath { get; set; }

        public string Name { get; set; }

        public double TickSize { get; set; }

        public double Commission { get; set; }

        public int Digits { get; set; }

        public long MinVolume { get; set; }

        public long MaxVolume { get; set; }

        public long VolumeStep { get; set; }

        public IBars Bars { get; }
    }
}
