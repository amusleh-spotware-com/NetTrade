using NetTrade.Enums;
using NetTrade.Interfaces;

namespace NetTrade.Models
{
    public class TradeResult
    {
        public TradeResult(OrderErrorCode orderErrorCode)
        {
            IsSuccessful = false;

            ErrorCode = orderErrorCode;
        }

        public TradeResult(IOrder order)
        {
            IsSuccessful = true;

            Order = order;
        }

        public bool IsSuccessful { get; }

        public OrderErrorCode ErrorCode { get; }

        public IOrder Order { get; }
    }
}