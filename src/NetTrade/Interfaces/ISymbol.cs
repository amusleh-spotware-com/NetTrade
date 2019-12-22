using NetTrade.Enums;
using NetTrade.Helpers;
using System;

namespace NetTrade.Interfaces
{
    public interface ISymbol : IEquatable<ISymbol>
    {
        string Name { get; }

        double TickSize { get; }

        double TickValue { get; }

        double Commission { get; }

        int Digits { get; }

        long MinVolume { get; }

        long MaxVolume { get; }

        long VolumeStep { get; }

        double Bid { get; }

        double Ask { get; }

        IBars Bars { get; }

        double Slippage { get; }

        double Spread { get; }

        event OnTickHandler OnTickEvent;

        double GetPrice(TradeType tradeType);
    }
}