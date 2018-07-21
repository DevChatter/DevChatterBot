using System.Net.Http;
using System.Threading.Tasks;

namespace DevChatter.Bot.Core.GoogleApi
{
    public interface ITimezoneLookup
    {
        Task<(float latitude, float longitude, bool success)>
            GetLatitudeAndLongitude(HttpClient client, string lookup);

        Task<(int, string)> GetTimezoneInfo(HttpClient client, float latitude, float longitude);
    }
}
