using DevChatter.Bot.Core.Caching;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.GoogleApi;
using System;

namespace DevChatter.Bot.Infra.Ef
{
    public class EfCacheLayer : ICacheLayer
    {
        private readonly IRepository _repo;

        public EfCacheLayer(IRepository repo)
        {
            _repo = repo;
        }

        public TimezoneLookupResult GetOrInsertTimezone(
            string lookup, Func<TimezoneLookupResult> fallback)
        {
            var timezone = _repo.Single(TimezonePolicy.ByLookup(lookup));

            if (timezone == null)
            {
                TimezoneLookupResult timezoneLookupResult = fallback.Invoke();
                timezone = new TimezoneEntity
                {
                    LookupString = lookup,
                    Offset = timezoneLookupResult.Offset,
                    TimezoneName = timezoneLookupResult.TimezoneName,
                };
                _repo.Create(timezone);
            }

            return timezone.ToTimezoneLookupResult();
        }
    }
}
