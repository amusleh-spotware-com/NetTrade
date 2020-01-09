using NetTrade.Abstractions.Interfaces;
using NetTrade.Enums;
using NetTrade.Helpers;
using System.Collections.Generic;

namespace NetTrade.Symbols
{
    public class OhlcSymbol : ISymbol
    {
        public OhlcSymbol(IBars bars)
        {
            Bars = bars;
        }

        public string Name { get; set; }

        public double TickSize { get; set; }

        public double TickValue { get; set; }

        public double Commission { get; set; }

        public int Digits { get; set; }

        public double MinVolume { get; set; }

        public double MaxVolume { get; set; }

        public double VolumeStep { get; set; }

        public double VolumeUnitValue { get; set; }

        public double Slippage { get; set; }

        public IBars Bars { get; }

        public double Bid { get; private set; }

        public double Ask { get; private set; }

        public double Spread => Ask - Bid;

        public event OnTickHandler OnTickEvent;

        public double GetPrice(TradeType tradeType) => tradeType == TradeType.Buy ? Ask : Bid;

        private void SetBidAsk(double bid, double ask)
        {
            Bid = bid;
            Ask = ask;

            OnTickEvent?.Invoke(this);
        }

        public void PublishBar(IBar bar)
        {
            SetBidAsk(bar.Close, bar.Close);

            Bars.AddBar(bar);
        }

        #region Equality methods

        public override bool Equals(object obj)
        {
            return Equals(obj as OhlcSymbol);
        }

        public bool Equals(ISymbol other)
        {
            return other != null && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public static bool operator ==(OhlcSymbol left, OhlcSymbol right)
        {
            return EqualityComparer<OhlcSymbol>.Default.Equals(left, right);
        }

        public static bool operator !=(OhlcSymbol left, OhlcSymbol right)
        {
            return !(left == right);
        }

        #endregion Equality methods

        #region Clone method

        public object Clone()
        {
            var clone = new OhlcSymbol(Bars.Clone() as IBars);

            ObjectCopy.CopyProperties(this, clone);

            return clone;
        }

        #endregion Clone method
    }
}