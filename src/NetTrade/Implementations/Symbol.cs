using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Interfaces;
using NetTrade.Models;
using System;
using System.Collections.Generic;

namespace NetTrade.Implementations
{
    public class Symbol : ISymbol
    {
        public Symbol(TimeSpan timeFrame)
        {
            Bars = new Bars(timeFrame);

            Bars.OnBar += Bars_OnBar;
        }

        public List<Bar> Data { get; set; }

        public string Name { get; set; }

        public double TickSize { get; set; }

        public double Commission { get; set; }

        public int Digits { get; set; }

        public long MinVolume { get; set; }

        public long MaxVolume { get; set; }

        public long VolumeStep { get; set; }

        public double Slippage { get; set; }

        public Bars Bars { get; }

        public double Bid => Bars.Close.LastValue;

        public double Ask => Bars.Close.LastValue;

        public double Spread => Ask - Bid;

        public event OnTickHandler OnTickEvent;

        public double GetPrice(TradeType tradeType) => tradeType == TradeType.Buy ? Ask : Bid;

        private void Bars_OnBar(object sender, int index) => OnTickEvent?.Invoke(this);

        #region Equality methods

        public override bool Equals(object obj)
        {
            return Equals(obj as Symbol);
        }

        public bool Equals(ISymbol other)
        {
            return other != null && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public static bool operator ==(Symbol left, Symbol right)
        {
            return EqualityComparer<Symbol>.Default.Equals(left, right);
        }

        public static bool operator !=(Symbol left, Symbol right)
        {
            return !(left == right);
        }

        #endregion Equality methods
    }
}