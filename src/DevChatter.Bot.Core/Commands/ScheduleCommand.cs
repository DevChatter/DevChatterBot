using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.GoogleApi;
using DevChatter.Bot.Core.Systems.Chat;
using NodaTime;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace DevChatter.Bot.Core.Commands
{
    public class ScheduleCommand : BaseCommand
    {
        private readonly ITimezoneLookup _timezoneLookup;

        public ScheduleCommand(IRepository repository, ITimezoneLookup timezoneLookup) : base(repository)
        {
            _timezoneLookup = timezoneLookup;
        }

        protected override async void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            var lookup = eventArgs?.Arguments?.ElementAtOrDefault(0);
            int offset;
            string timezoneDisplay;
            bool useTwentyFourHourTime = false;
            if (eventArgs?.Arguments?.Count == 0)
            {
                offset = 0;
                useTwentyFourHourTime = true;
                timezoneDisplay = $"at UTC {offset:+#;-#;+0}";
            }
            else
            {
                bool isValidInteger = int.TryParse(lookup, out int chatUserOffset);
                if (isValidInteger && chatUserOffset < 18 && chatUserOffset > -18)
                {
                    useTwentyFourHourTime = true;
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

                    TimezoneLookupResult lookupResult =
                        await _timezoneLookup.GetTimezoneInfoAsync(client, lookup);

                    if (!lookupResult.Success)
                    {
                        chatClient.SendMessage(lookupResult.Message);
                        return;
                    }

                    offset = lookupResult.Offset;
                    timezoneDisplay = $"in {lookupResult.TimezoneName}";
                }
            }

            DateTimeZone timeZone = DateTimeZone.ForOffset(Offset.FromHours(offset));

            List<Instant> streamTimes = Repository.List(DataItemPolicy<ScheduleEntity>.All()).Select(x => x.Instant).ToList();

            string message = $"Our usual schedule ({timezoneDisplay}) is: " + string.Join(", ", streamTimes.Select(x => GetTimeDisplay(x, timeZone, useTwentyFourHourTime)));

            chatClient.SendMessage(message);
        }


        private static string GetTimeDisplay(Instant instant, DateTimeZone timeZone, bool isTwentyFourTime = false)
        {
            ZonedDateTime zonedDateTime = instant.InZone(timeZone);
            string timeFormatString = isTwentyFourTime ? "HH:mm" : "h:mm tt";
            string timeOfDay = zonedDateTime.TimeOfDay.ToString(timeFormatString, CultureInfo.InvariantCulture);
            return $"{zonedDateTime.DayOfWeek}s at {timeOfDay}";
        }

        public static class Messages
        {
            public const string OUT_OF_RANGE = "UTC offset must be a whole number between -18 and +18";
        }
    }
}
