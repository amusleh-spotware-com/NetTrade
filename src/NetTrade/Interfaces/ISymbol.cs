using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Models;
using System;
using System.Collections.Generic;

namespace NetTrade.Interfaces
{
    public interface ISymbol : IEquatable<ISymbol>
    {
        List<Bar> Data { get; set; }

        string Name { get; }

        double TickSize { get; }

        double Commission { get; }

        int Digits { get; }

        long MinVolume { get; }

        long MaxVolume { get; }

        long VolumeStep { get; }

        double Bid { get; }

        double Ask { get; }

        Bars Bars { get; }

        double Slippage { get; }

        double Spread { get; }

        event OnTickHandler OnTickEvent;

        double GetPrice(TradeType tradeType);
    }
}