using System;
using System.Threading.Tasks;
using DevChatter.Bot.Core.GoogleApi;

namespace DevChatter.Bot.Core.Caching
{
    public interface ICacheLayer
    {
        Task<TimezoneLookupResult> GetOrInsertTimezone(string lookup, Func<TimezoneLookupResult> fallback);
    }
}
