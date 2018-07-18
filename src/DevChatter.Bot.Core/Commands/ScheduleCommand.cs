using System;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using NodaTime;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Specifications;
using System.Net.Http;
using Newtonsoft.Json;
using DevChatter.Bot.Core.GoogleApi;

namespace DevChatter.Bot.Core.Commands
{
    public class ScheduleCommand : BaseCommand
    {
        private readonly GoogleCloudSettings _settings;
        public ScheduleCommand(IRepository repository, GoogleCloudSettings settings) : base(repository, UserRole.Everyone)
        {
            HelpText = "To see our schedule just type !schedule followed by either a timezone offset of a city name. Example !schedule -4 or !schedule Cleveland ";
            _settings = settings;
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var lookup = eventArgs?.Arguments?.ElementAtOrDefault(0);
            int offset;
            string timezoneDisplay = "";
            if (eventArgs.Arguments.Count == 0)
            {
                offset = 0;
                timezoneDisplay = $"at UTC {offset:+#;-#;+0}";
            }
            else
            {
                bool isValidInteger = int.TryParse(lookup, out int chatUserOffset);
                if (isValidInteger && chatUserOffset < 18 && chatUserOffset > -18)
                {
                    offset = chatUserOffset;
                    timezoneDisplay = $"at UTC {offset:+#;-#;+0}";
                }
                else if (isValidInteger && (chatUserOffset > 18 || chatUserOffset < -18))
                {
                    chatClient.SendMessage(Messages.OUT_OF_RANGE);
                    return;
                }
                else
                {
                    var client = new HttpClient();

                    var (latitude, longitude, success) = GetLatitudeAndLongitude(client, lookup);
                    if (success)
                    {
                        var timezoneLookupUrl = $"https://maps.googleapis.com/maps/api/timezone/json?location={latitude},{longitude}&timestamp={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}&key={_settings.ApiKey}";
                        var timezoneResponse = JsonConvert.DeserializeObject<TimezoneResponse>(client.GetAsync(timezoneLookupUrl).Result.Content.ReadAsStringAsync().Result); // 😞 This code makes me cry...
                        var offsetTimespan = TimeSpan.FromSeconds(timezoneResponse.rawOffset + timezoneResponse.dstOffset);
                        offset = offsetTimespan.Hours;
                        timezoneDisplay = $"in {timezoneResponse.timeZoneName}";
                    }
                    else
                    {
                        chatClient.SendMessage(Messages.UNKNOWN_CITY);
                        return;
                    }
                }
            }

            DateTimeZone timeZone = DateTimeZone.ForOffset(Offset.FromHours(offset));

            List<Instant> streamTimes = Repository.List(DataItemPolicy<ScheduleEntity>.All()).Select(x => x.Instant).ToList();

            string message = $"Our usual schedule ({timezoneDisplay}) is: " + string.Join(", ", streamTimes.Select(x => GetTimeDisplay(x, timeZone)));

            chatClient.SendMessage(message);
        }

        private (float latitude, float longitude, bool success)
            GetLatitudeAndLongitude(HttpClient client, string lookup)
        {
            var placeLookupUrl = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={lookup}&key={_settings.ApiKey}";
            var latitudeLongitudeResponse = JsonConvert.DeserializeObject<PlaceResponse>(client.GetAsync(placeLookupUrl).Result.Content.ReadAsStringAsync().Result);
            var latitude = latitudeLongitudeResponse.results.FirstOrDefault()?.geometry.location.lat;
            var longitude = latitudeLongitudeResponse.results.FirstOrDefault()?.geometry.location.lng;

            if (latitude.HasValue && longitude.HasValue)
            {
                return (latitude.Value, longitude.Value, true);
            }

            return (default(float), default(float), false);
        }

        private static string GetTimeDisplay(Instant instant, DateTimeZone timeZone)
        {
            return $"{instant.InZone(timeZone).DayOfWeek}s at {instant.InZone(timeZone).TimeOfDay:h:mm tt}";
        }

        public static class Messages
        {
            public const string OUT_OF_RANGE = "UTC offset must be a whole number between -18 and +18";
            public const string UNKNOWN_CITY = "The given location is unknown to Google, well done!";
        }
    }
}
