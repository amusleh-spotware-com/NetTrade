using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Interfaces
{
    public interface ISymbol
    {
        string DataFilePath { get; }

        string Name { get; }

        double TickSize { get; }

        double Commission { get; }

        int Digits { get; }

        long MinVolume { get; }

        long MaxVolume { get; }

        long VolumeStep { get; }

        IBars Bars { get; }
    }
}
