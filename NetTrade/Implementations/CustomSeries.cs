using NetTrade.Abstractions;
using System.Collections.Concurrent;

namespace NetTrade.Implementations
{
    internal class CustomSeries<T> : Series<T>
    {
        private readonly ConcurrentDictionary<int, T> _storage = new ConcurrentDictionary<int, T>();

        protected override ConcurrentDictionary<int, T> GetStorage() => _storage;

        internal void Add(int index, T value)
        {
            _storage.AddOrUpdate(index, value, (iIndex, iValue) => value);
        }
    }
}