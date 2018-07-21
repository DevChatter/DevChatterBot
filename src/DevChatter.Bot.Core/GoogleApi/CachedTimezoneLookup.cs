using DevChatter.Bot.Core.Caching;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.GoogleApi
{
    public class CachedTimezoneLookup : ITimezoneLookup
    {
        private readonly ITimezoneLookup _internalLookup;
        private readonly ICacheLayer _cacheLayer;

        public CachedTimezoneLookup(ITimezoneLookup internalLookup, ICacheLayer cacheLayer)
        {
            _internalLookup = internalLookup;
            _cacheLayer = cacheLayer;
        }

        public async Task<TimezoneLookupResult> GetTimezoneInfoAsync(
            HttpClient client, string lookup)
        {
            return await _cacheLayer.GetOrInsertTimezone(lookup, Fallback);

            TimezoneLookupResult Fallback() => _internalLookup.GetTimezoneInfoAsync(client, lookup).Result;
        }
    }
}
