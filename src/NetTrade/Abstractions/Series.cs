using NetTrade.Interfaces;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NetTrade.Abstractions
{
    public abstract class Series<T> : ISeries<T>
    {
        public Series()
        {
            Storage = GetStorage();
        }

        private ConcurrentDictionary<int, T> Storage { get; }
        public T this[int index] => Storage[index];

        public int Count => Storage.Count;

        public IEnumerator<T> GetEnumerator() => Storage.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Storage.Values.GetEnumerator();

        protected abstract ConcurrentDictionary<int, T> GetStorage();
    }
}