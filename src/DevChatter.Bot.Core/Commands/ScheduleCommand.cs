using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;
using NodaTime;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Specifications;

namespace DevChatter.Bot.Core.Commands
{
    public class ScheduleCommand : BaseCommand
    {
        public ScheduleCommand(IRepository repository) : base(repository, UserRole.Everyone)
        {
            HelpText = "To see our schedule just type !schedule and to see it in another timezone, pass your UTC offset. Example: !schedule -4";
        }

        protected override void HandleCommand(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            if (!int.TryParse(eventArgs?.Arguments?.ElementAtOrDefault(0), out int offset) || offset > 18 || offset < -18)
            {
                return;
            }

            DateTimeZone timeZone = DateTimeZone.ForOffset(Offset.FromHours(offset));

            List<Instant> streamTimes = Repository.List(DataItemPolicy<ScheduleEntity>.All()).Select(x => x.Instant).ToList();

            string message = "Our usual schedule is: " + string.Join(", ", streamTimes.Select(x => GetTimeDisplay(x, timeZone)));

            chatClient.SendMessage(message);
        }

        private static string GetTimeDisplay(Instant instant, DateTimeZone timeZone)
        {
            return $"{instant.InZone(timeZone).DayOfWeek}s at {instant.InZone(timeZone).TimeOfDay:h:mm tt}";
        }
    }
}
