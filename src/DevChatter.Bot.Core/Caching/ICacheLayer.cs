using DevChatter.Bot.Core.GoogleApi;
using System;

namespace DevChatter.Bot.Core.Caching
{
    public interface ICacheLayer
    {
        TimezoneLookupResult GetOrInsertTimezone(string lookup, Func<TimezoneLookupResult> fallback);
    }
}
