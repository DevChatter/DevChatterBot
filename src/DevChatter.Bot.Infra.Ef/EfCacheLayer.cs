using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Caching;

namespace DevChatter.Bot.Infra.Ef
{
    public class EfCacheLayer : ICacheLayer
    {
        private readonly AppDataContext _db;

        public EfCacheLayer(AppDataContext db)
        {
            _db = db;
        }

        public Task<T> GetOrInsert<T>(string cacheKey, Func<T> fallback)
        {
            // Check to see if the value is in the cache

            // if not, use the fallback method to get the value

            // insert the fallback value into the cache

            // return the value from the cache

            throw new NotImplementedException();
        }
    }
}
