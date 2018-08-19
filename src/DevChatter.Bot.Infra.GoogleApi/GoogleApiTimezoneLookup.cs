using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.GoogleApi;
using Newtonsoft.Json;

namespace DevChatter.Bot.Infra.GoogleApi
{
    public class GoogleApiTimezoneLookup : ITimezoneLookup
    {
        private const string UNKNOWN_LOCATION = "The given location is unknown to Google, well done!";
        private readonly GoogleCloudSettings _settings;

        public GoogleApiTimezoneLookup(GoogleCloudSettings settings)
        {
            _settings = settings;
        }

        public async Task<(float latitude, float longitude, bool success)>
            GetLatitudeAndLongitude(HttpClient client, string lookup)
        {
            var placeLookupUrl = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={lookup}&key={_settings.ApiKey}";
            string result = await client.GetAsync(placeLookupUrl).Result.Content.ReadAsStringAsync();
            var latitudeLongitudeResponse = JsonConvert.DeserializeObject<PlaceResponse>(result);
            var latitude = latitudeLongitudeResponse.results.FirstOrDefault()?.geometry.location.lat;
            var longitude = latitudeLongitudeResponse.results.FirstOrDefault()?.geometry.location.lng;

            if (latitude.HasValue && longitude.HasValue)
            {
                return (latitude.Value, longitude.Value, true);
            }

            return (default(float), default(float), false);
        }

        public async Task<(int, string)> GetTimezoneInfo(HttpClient client, float latitude, float longitude)
        {
            long unixTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var timezoneLookupUrl = $"https://maps.googleapis.com/maps/api/timezone/json?location={latitude},{longitude}&timestamp={unixTimeSeconds}&key={_settings.ApiKey}";
            string result = await client.GetAsync(timezoneLookupUrl).Result.Content.ReadAsStringAsync();
            var timezoneResponse = JsonConvert.DeserializeObject<TimezoneResponse>(result);
            var offsetTimespan = TimeSpan.FromSeconds(timezoneResponse.rawOffset + timezoneResponse.dstOffset);
            return (offsetTimespan.Hours, timezoneResponse.timeZoneName);
        }


        public async Task<TimezoneLookupResult> GetTimezoneInfoAsync(HttpClient client, string lookup)
        {
            var result = new TimezoneLookupResult();
            var (latitude, longitude, success) =
                await GetLatitudeAndLongitude(client, lookup);

            if (success)
            {
                (result.Offset, result.TimezoneName) =
                    await GetTimezoneInfo(client, latitude, longitude);
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.Message = UNKNOWN_LOCATION;
            }

            return result;
        }
    }
}
