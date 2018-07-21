using System;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.Caching
{
    public interface ICacheLayer
    {
        Task<T> GetOrInsert<T>(string cacheKey, Func<T> fallback);
    }
}
