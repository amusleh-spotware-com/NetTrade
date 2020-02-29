using NetTrade.Helpers;

namespace NetTrade.Abstractions.Interfaces
{
    public interface IIndicator
    {
        event OnNewValueHandler OnNewValueEvent;
    }
}