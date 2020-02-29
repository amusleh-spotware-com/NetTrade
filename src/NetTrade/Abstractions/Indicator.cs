using NetTrade.Abstractions.Interfaces;
using NetTrade.Helpers;

namespace NetTrade.Abstractions
{
    public abstract class Indicator : IIndicator
    {
        public event OnNewValueHandler OnNewValueEvent;

        protected void OnNewValue(int index) => OnNewValueEvent?.Invoke(this, index);
    }
}