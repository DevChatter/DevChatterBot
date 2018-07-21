using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using NodaTime;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DevChatter.Bot.Core.GoogleApi;

namespace DevChatter.Bot.Core.Commands
{
    public class ScheduleCommand : BaseCommand
    {
        private readonly ITimezoneLookup _timezoneLookup;

        public ScheduleCommand(IRepository repository, ITimezoneLookup timezoneLookup) : base(repository, UserRole.Everyone)
        {
            _timezoneLookup = timezoneLookup;
            HelpText = "To see our schedule just type !schedule followed by either a timezone offset of a city name. Example !schedule -4 or !schedule Cleveland ";
        }

        protected override async void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
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

            string message = $"Our usual schedule ({timezoneDisplay}) is: " + string.Join(", ", streamTimes.Select(x => GetTimeDisplay(x, timeZone)));

            chatClient.SendMessage(message);
        }


        private static string GetTimeDisplay(Instant instant, DateTimeZone timeZone)
        {
            return $"{instant.InZone(timeZone).DayOfWeek}s at {instant.InZone(timeZone).TimeOfDay:h:mm tt}";
        }

        public static class Messages
        {
            public const string OUT_OF_RANGE = "UTC offset must be a whole number between -18 and +18";
        }
    }
}
