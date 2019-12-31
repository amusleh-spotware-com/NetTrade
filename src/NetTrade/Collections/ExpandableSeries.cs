using System.Collections.Concurrent;

namespace NetTrade.Collections
{
    internal class ExpandableSeries<T> : Series<T>
    {
        private readonly ConcurrentDictionary<int, T> _storage = new ConcurrentDictionary<int, T>();

        protected override ConcurrentDictionary<int, T> GetStorage() => _storage;

        public void Add(int index, T value)
        {
            _storage.AddOrUpdate(index, value, (iIndex, iValue) => value);
        }

        public void Add(T value)
        {
            int index = _storage.Keys.Count;

            Add(index, value);
        }
    }
}