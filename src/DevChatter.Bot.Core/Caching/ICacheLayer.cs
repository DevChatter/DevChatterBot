using System.Collections.Generic;
using DevChatter.Bot.Core.Data;

namespace DevChatter.Bot.Core.Caching
{
    public interface ICacheLayer
    {
        T TryGet<T>(string cacheKey) where T : DataItem;
        void Insert<T>(T item, string cacheKey) where T : DataItem;
    }

    public class InMemoryCacheLayer : ICacheLayer
    {
        private readonly Dictionary<string, DataItem> _cache = new Dictionary<string, DataItem>();

        public T TryGet<T>(string cacheKey) where T : DataItem
        {
            if (_cache.ContainsKey(cacheKey))
            {
                return _cache[cacheKey] as T;
            }
            return null;
        }

        public void Insert<T>(T item, string cacheKey) where T : DataItem
        {
            _cache[cacheKey] = item;
        }
    }
}