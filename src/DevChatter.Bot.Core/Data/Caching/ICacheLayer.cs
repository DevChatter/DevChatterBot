using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Data.Caching
{
    public interface ICacheLayer
    {
        T TryGet<T>(string cacheKey) where T : DataItem;
        void Insert<T>(T item, string cacheKey) where T : DataItem;
    }
}