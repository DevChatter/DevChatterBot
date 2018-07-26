using System.Net.Http;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.GoogleApi
{
    public interface ITimezoneLookup
    {
        Task<TimezoneLookupResult> GetTimezoneInfoAsync(HttpClient client, string lookup);
    }
}
