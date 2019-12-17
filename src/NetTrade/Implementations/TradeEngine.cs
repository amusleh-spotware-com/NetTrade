using System;
using System.Collections.Generic;
using System.Text;
using NetTrade.Interfaces;
using NetTrade.Models;

namespace NetTrade.Implementations
{
    public class TradeEngine : ITradeEngine
    {
        #region Fields

        private readonly CustomSeries<IOrder> _orders = new CustomSeries<IOrder>();
        private readonly CustomSeries<ITrade> _trades = new CustomSeries<ITrade>();

        #endregion

        public ISeries<IOrder> Orders => _orders;

        public ISeries<ITrade> Trades => _trades;

        public TradeResult PlaceOrder(IOrderParameters parameters) => parameters.Execute(this);

        public void UpdateSymbolOrders(ISymbol symbol)
        {
        }

        public void AddOrder(IOrder order)
        {
            _orders.Add(order);
        }
    }
}
