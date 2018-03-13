using System.Collections.Generic;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Data.Caching
{
    // TODO: Create a better ICacheLayer implementation (have this wrap a real one, move to infra)
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