using System;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.Bot.Core.Caching;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.GoogleApi;
using Microsoft.EntityFrameworkCore;

namespace DevChatter.Bot.Infra.Ef
{
    public class EfCacheLayer : ICacheLayer
    {
        private readonly AppDataContext _db;

        public EfCacheLayer(AppDataContext db)
        {
            _db = db;
        }

        public async Task<TimezoneLookupResult> GetOrInsertTimezone(
            string lookup, Func<TimezoneLookupResult> fallback)
        {
            var timezone = await _db.Timezones.SingleOrDefaultAsync(
                x => x.LookupString.EqualsIns(lookup));

            if (timezone == null)
            {
                TimezoneLookupResult timezoneLookupResult = fallback.Invoke();
                timezone = new TimezoneEntity
                {
                    Offset = timezoneLookupResult.Offset,
                    TimezoneName = timezoneLookupResult.TimezoneName,
                };
                await _db.Timezones.AddAsync(timezone);
                await _db.SaveChangesAsync();
            }

            return timezone.ToTimezoneLookupResult();
        }
    }
}
