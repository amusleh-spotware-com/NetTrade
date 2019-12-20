using NetTrade.Enums;
using NetTrade.Helpers;
using NetTrade.Interfaces;
using NetTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetTrade.Implementations
{
    public class Symbol : ISymbol
    {
        private readonly List<IBar> _barsData;

        public Symbol(List<IBar> barsData, IBars bars)
        {
            _barsData = barsData;

            Bars = bars;
        }

        public string Name { get; set; }

        public double TickSize { get; set; }

        public double Commission { get; set; }

        public int Digits { get; set; }

        public long MinVolume { get; set; }

        public long MaxVolume { get; set; }

        public long VolumeStep { get; set; }

        public double Slippage { get; set; }

        public IBars Bars { get; }

        public double Bid { get; private set; }

        public double Ask { get; private set; }

        public double Spread => Ask - Bid;

        public bool IsSubscribedToDataFeed { get; private set; }

        public event OnTickHandler OnTickEvent;

        public double GetPrice(TradeType tradeType) => tradeType == TradeType.Buy ? Ask : Bid;

        private void SetBidAsk(double bid, double ask)
        {
            Bid = bid;
            Ask = ask;

            OnTickEvent?.Invoke(this);
        }

        public void SubscribeToDataFeed()
        {
            IsSubscribedToDataFeed = true;

            var barsDataOrdered = _barsData.OrderBy(iBar => iBar.Time);

            foreach (var bar in barsDataOrdered)
            {
                if (!IsSubscribedToDataFeed)
                {
                    break;
                }

                SetBidAsk(bar.Close, bar.Close);

                var index = Bars.AddBar(bar);

                _barsData.Remove(bar);
            }
        }

        public void UnsubscribeFromDataFeed() => IsSubscribedToDataFeed = false;

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