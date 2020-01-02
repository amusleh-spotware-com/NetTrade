using NetTrade.Abstractions.Interfaces;
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

        public T LastValue => this[Count - 1];

        public IEnumerator<T> GetEnumerator() => Storage.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Storage.Values.GetEnumerator();

        protected abstract ConcurrentDictionary<int, T> GetStorage();

        public T Last(int index)
        {
            int lastIndex = (Count - 1) - index;

            return this[lastIndex];
        }
    }
}