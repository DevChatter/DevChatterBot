using DevChatter.Bot.Core.Data;

namespace DevChatter.Bot.Core.Caching
{
    public interface ICacheLayer
    {
        T TryGet<T>(string cacheKey) where T : DataItem;
        void Insert<T>(T item, string cacheKey) where T : DataItem;
    }
}